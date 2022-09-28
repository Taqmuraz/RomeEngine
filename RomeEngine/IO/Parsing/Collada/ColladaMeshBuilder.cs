using System;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaMeshBuilder : IColladaBuilder
    {
        public void BuildGameObject(GameObject gameObject, ColladaEntity rootEntity)
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
            public Buffer(Array array, int stride, int offset)
            {
                Array = array;
                Stride = stride;
                Offset = offset;
            }

            public Array Array { get; }
            public int Stride { get; }
            public int Offset { get; }
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

                buffers.Add(new Buffer(floatParseFunc(positionsBuffer.SeparateString()), getStrideFunc(positions), vertices.Properties["offset"].GetInt()));
                buffers.Add(new Buffer(floatParseFunc(texcoordsBuffer.SeparateString()), getStrideFunc(texcoords), texcoords.Properties["offset"].GetInt()));
                buffers.Add(new Buffer(floatParseFunc(normalsBuffer.SeparateString()), getStrideFunc(normals), normals.Properties["offset"].GetInt()));

                if (strideCheck.Any(s => buffers[s.index].Stride != s.stride))
                {
                    throw new InvalidOperationException("Vertex buffer stride is not equal to expected");
                }

                var indices = submeshEntity["p"].Single().Value.SeparateString().Select(i => int.Parse(i)).ToArray();

                if (skinEntity != null)
                {
                    HandleSkinBuffers(skinEntity, out float[] weights, out int[] joints, out jointNames, ref indices, buffers.Count);
                    buffers.Add(new Buffer(weights, 3, 3));
                    buffers.Add(new Buffer(joints, 3, 4));
                }

                int vertexStride = buffers.Count;
                int verticesCount = indices.Length / vertexStride;

                Array[] resultBuffers = buffers.Select(b => Array.CreateInstance(b.Array.GetType().GetElementType(), b.Stride * verticesCount)).ToArray();
                int[] resultIndices = Enumerable.Range(0, verticesCount).ToArray();

                for (int bufferIndex = 0; bufferIndex < buffers.Count; bufferIndex++)
                {
                    var buffer = buffers[bufferIndex];

                    for (int i = 0; i < verticesCount; i++)
                    {
                        int index = indices[i * vertexStride + buffer.Offset];
                        for (int element = 0; element < buffer.Stride; element++)
                        {
                            resultBuffers[bufferIndex].SetValue(buffer.Array.GetValue(index * buffer.Stride + element), i * buffer.Stride + element);
                        }
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
                    renderer = skinnedMeshRenderer;
                    skinnedMeshRenderer.SkinnedMesh = new SkinnedMesh
                        (
                            (float[])resultBuffers[0],
                            (float[])resultBuffers[1],
                            (float[])resultBuffers[2],
                            (float[])resultBuffers[3],
                            (int[])resultBuffers[4],
                            resultIndices,
                            jointNames,
                            polygonFormat
                        );
                }
                else
                {
                    var staticBufferMeshRenderer = gameObject.AddComponent<StaticBufferMeshRenderer>();
                    renderer = staticBufferMeshRenderer;
                    staticBufferMeshRenderer.StaticBufferMesh = new StaticBufferMesh
                        (
                            (float[])resultBuffers[0],
                            (float[])resultBuffers[1],
                            (float[])resultBuffers[2],
                            resultIndices,
                            polygonFormat
                        );
                }
                if (submeshEntity.Properties.HasProperty("material")) renderer.Material = new SingleTextureMaterial(submeshEntity.Properties["material"].Value);
            }
        }
        static void HandleSkinBuffers(ColladaEntity skinEntity, out float[] weights, out int[] joints, out string[] jointNames, ref int[] indices, int indexStride)
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
            int maxJoints = 3;
            int stride = 2;
            weights = new float[numbers.Length * maxJoints];
            joints = new int[numbers.Length * maxJoints];

            for (int n = 0; n < numbers.Length; n++)
            {
                int number = numbers[n];
                for (int j = 0; j < maxJoints; j++)
                {
                    float weight;
                    int joint;
                    if (j < number)
                    {
                        joint = jwIndices[indicesOffset + stride * j];
                        weight = weightsRaw[jwIndices[indicesOffset + stride * j + 1]];
                        indicesOffset += stride;
                    }
                    else
                    {
                        joint = -1;
                        weight = 0f;
                    }
                    weights[n] = weight;
                    joints[n] = joint;
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
        }
    }
}
