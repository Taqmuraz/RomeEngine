using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaMeshBuilder : IColladaBuilder
    {
        public void BuildGameObject(GameObject gameObject, ColladaEntity rootEntity, ColladaParsingInfo info)
        {
            var geometries = rootEntity["library_geometries"]["geometry"];
            var skins = rootEntity["library_controllers"]["controller"]["skin"];
            foreach (var geometry in geometries)
            {
                var meshes = geometry["mesh"];
                var skin = skins.FirstOrDefault(s => s.Properties["source"] == geometry.Properties["id"]);
                foreach (var mesh in meshes)
                {
                    HandleMesh(gameObject, mesh, skin);
                }
            }
        }

        sealed class Buffer
        {
            public Buffer(Array array, int stride, int offset, MeshAttributeFormat type)
            {
                Array = array;
                Stride = stride;
                Offset = offset;
                Type = type;
            }

            public Array Array { get; }
            public int Stride { get; }
            public int Offset { get; }
            public MeshAttributeFormat Type { get; }

            public MeshAttribute CreateAttribute() => new MeshAttribute(Array, Stride, Type, Offset);
        }

        static void HandleMesh(GameObject gameObject, ColladaEntity meshEntity, ColladaEntity skinEntity)
        {
            var submeshes = meshEntity["triangles"].Concat(meshEntity["polylist"]);

            foreach (var submeshEntity in submeshes)
            {
                var vertices = submeshEntity["vertex"].Single();
                var positions = vertices["position"].Single();
                var texcoords = submeshEntity["texcoord"].Single();
                var normals = submeshEntity["normal"].Single();
                var positionsBuffer = positions["float_array"].Single().Value;
                var texcoordsBuffer = texcoords["float_array"].Single().Value;
                var normalsBuffer = normals["float_array"].Single().Value;
                List<Buffer> buffers = new List<Buffer>();
                Func<string[], float[]> floatParseFunc = p => p.Select(s => s.ToFloat()).ToArray();
                string[] jointNames = null;
                Func<ColladaEntity, int> getStrideFunc = e => e["technique_common"]["accessor"].Single().Properties["stride"].GetInt();
                (int index, int stride)[] strideCheck = new (int, int)[] { (0, 3), (1, 2), (2, 3) };

                buffers.Add(new Buffer(floatParseFunc(positionsBuffer.SeparateString()), getStrideFunc(positions), vertices.Properties["offset"].GetInt(), MeshAttributeFormat.Float8Bit));
                buffers.Add(new Buffer(floatParseFunc(texcoordsBuffer.SeparateString()), getStrideFunc(texcoords), texcoords.Properties["offset"].GetInt(), MeshAttributeFormat.UFloat8Bit));
                buffers.Add(new Buffer(floatParseFunc(normalsBuffer.SeparateString()), getStrideFunc(normals), normals.Properties["offset"].GetInt(), MeshAttributeFormat.Float8Bit));

                if (strideCheck.Any(s => buffers[s.index].Stride != s.stride))
                {
                    throw new InvalidOperationException("Vertex buffer stride is not equal to expected");
                }

                var indices = submeshEntity["p"].Single().Value.SeparateString().Select(i => int.Parse(i)).ToArray();

                if (skinEntity != null)
                {
                    HandleSkinBuffers(skinEntity, out Buffer weights, out Buffer joints, out jointNames, ref indices, buffers.Count);
                    buffers.Add(weights);
                    buffers.Add(joints);
                }

                int vertexStride = buffers.Count;
                int verticesCount = indices.Length / vertexStride;

                var newIndices = Enumerable.Range(0, verticesCount).ToArray();

                var resultBuffers = buffers.Select(b => new Buffer(Array.CreateInstance(b.Array.GetType().GetElementType(), verticesCount * b.Stride), b.Stride, b.Offset, b.Type)).ToArray();

                for (int i = 0; i < verticesCount; i++)
                {
                    for (int buffer = 0; buffer < buffers.Count; buffer++)
                    {
                        int index = indices[i * vertexStride + buffers[buffer].Offset];
                        int stride = buffers[buffer].Stride;

                        for (int element = 0; element < stride; element++) resultBuffers[buffer].Array.SetValue(buffers[buffer].Array.GetValue(index * stride + element), i * stride + element);
                    }
                }

                PolygonFormat polygonFormat;

                switch (submeshEntity.Type)
                {
                    case "triangles": polygonFormat = PolygonFormat.Triangles; break;
                    case "polylist": polygonFormat = PolygonFormat.Polygons; break;
                    default:
                        throw new InvalidOperationException($"Format {submeshEntity.Type} is not supported");
                }

                MeshRenderer renderer;

                if (skinEntity != null)
                {
                    var skinnedMeshRenderer = gameObject.AddComponent<SkinnedMeshRenderer>();
                    var bindingPoses = skinEntity["joints"]["inv_bind_matrix"]["float_array"].Single().Value.SeparateString().Select(s => s.ToFloat()).ToArray();

                    Matrix4x4[] matrices = new Matrix4x4[bindingPoses.Length / 16];

                    for (int i = 0; i < matrices.Length; i++)
                    {
                        matrices[i] = Matrix4x4.FromFloatsArray(bindingPoses, i * 16).GetTransponed().GetInversed();
                    }

                    renderer = skinnedMeshRenderer;
                    skinnedMeshRenderer.SkinnedMesh = new SkinnedMesh
                        (
                            resultBuffers.Select(r => r.CreateAttribute()).ToArray(),
                            newIndices,
                            jointNames,
                            matrices,
                            polygonFormat
                        );
                }
                else
                {
                    var staticBufferMeshRenderer = gameObject.AddComponent<StaticBufferMeshRenderer>();
                    renderer = staticBufferMeshRenderer;
                    staticBufferMeshRenderer.StaticBufferMesh = new StaticBufferMesh
                        (
                            resultBuffers.Select(r => r.CreateAttribute()).ToArray(),
                            newIndices,
                            polygonFormat
                        );
                }
                if (submeshEntity.Properties.HasProperty("material")) renderer.Material = new SingleTextureMaterial(submeshEntity.Properties["material"].Value);
            }
        }
        static void HandleSkinBuffers(ColladaEntity skinEntity, out Buffer weights, out Buffer joints, out string[] jointNames, ref int[] indices, int indexStride)
        {
            var weightsBuffer = skinEntity["vertex_weights"]["weight"]["float_array"].Single().Value;
            var jointsBuffer = skinEntity["vertex_weights"]["joint"]["name_array"].Single().Value;
            var numbersBuffer = skinEntity["vertex_weights"]["vcount"].Single().Value;
            var indicesBuffer = skinEntity["vertex_weights"]["v"].Single().Value;

            var weightsRaw = weightsBuffer.SeparateString().Select(w => w.ToFloat()).ToArray();
            jointNames = jointsBuffer.SeparateString();
            var numbers = numbersBuffer.SeparateString().Select(n => int.Parse(n)).ToArray();
            var jwIndices = indicesBuffer.SeparateString().Select(i => int.Parse(i)).ToArray();

            int indicesOffset = 0;
            int maxJoints = SkinnedMesh.MaxJointsSupported;
            int stride = 2;
            var weightsArray = new float[numbers.Length * maxJoints];
            var jointsArray = new int[numbers.Length * maxJoints];

            for (int n = 0; n < numbers.Length; n++)
            {
                int number = numbers[n];
                bool normalize = false;

                int maxNumber = Math.Max(number, maxJoints);

                (float weight, int joint)[] vertexWeights = new (float, int)[maxNumber];

                for (int i = 0; i < maxNumber; i++)
                {
                    vertexWeights[i] = (0f, -1);
                }

                for (int i = 0; i < number; i++)
                {
                    vertexWeights[i] = (weightsRaw[jwIndices[indicesOffset + 1]], jwIndices[indicesOffset]);
                    indicesOffset += stride;
                }

                if (number > maxJoints)
                {
                    vertexWeights = vertexWeights.OrderByDescending(w => w.weight).Take(maxJoints).ToArray();
                    normalize = true;
                }

                if (normalize)
                {
                    float sum = vertexWeights.Select(v => v.weight).Sum();
                    if (sum != 0)
                    {
                        for (int i = 0; i < vertexWeights.Length; i++) vertexWeights[i].weight /= sum;
                    }
                }

                for (int i = 0; i < maxJoints; i++)
                {
                    weightsArray[n * maxJoints + i] = vertexWeights[i].weight;
                    jointsArray[n * maxJoints + i] = vertexWeights[i].joint;
                }
            }

            int verticesCount = indices.Length / indexStride;
            int newIndexStride = indexStride + 2;
            int[] newIndices = new int[verticesCount * newIndexStride];

            for (int v = 0; v < verticesCount; v++)
            {
                for (int i = 0; i < indexStride; i++)
                {
                    newIndices[v * newIndexStride + i] = indices[v * indexStride + i];
                }
                newIndices[v * newIndexStride + 3] = indices[v * indexStride];
                newIndices[v * newIndexStride + 4] = indices[v * indexStride];
            }
            indices = newIndices;

            weights = new Buffer(weightsArray, 3, 3, MeshAttributeFormat.UFloat8Bit);
            joints = new Buffer(jointsArray, 3, 4, MeshAttributeFormat.UInt8Bit);
        }
    }
}
