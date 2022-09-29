﻿using System;
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

        class ColladaVertexEqualityProvider : IEqualityComparer<ColladaVertex>
        {
            public bool Equals(ColladaVertex a, ColladaVertex b)
            {
                return a.IsEqualTo(b);
            }

            public int GetHashCode(ColladaVertex vertex)
            {
                return vertex.CalculateHashCode();
            }
        }

        class ColladaVertex
        {
            int[] attributeIndices;
            List<Buffer> buffers;

            public ColladaVertex(List<Buffer> buffersInfo, int id)
            {
                attributeIndices = new int[buffersInfo.Count];
                this.buffers = buffersInfo;
                Id = id;
            }

            public int CalculateHashCode()
            {
                return attributeIndices[0];
            }

            public int Id { get; }

            public bool IsEqualTo(ColladaVertex vertex)
            {
                return vertex.attributeIndices[0] == attributeIndices[0];
            }

            public void SetIndex(int number, int index)
            {
                attributeIndices[number] = index;
            }

            public int PositionIndex => attributeIndices[0];

            public void HandleBuffer(int attribute, Array source, Array destination, int pointer)
            {
                int length = buffers[attribute].Stride;
                for (int i = 0; i < length; i++)
                {
                    destination.SetValue(source.GetValue(attributeIndices[attribute] * length + i), pointer + i);
                }
            }

            public override string ToString() => string.Join(", ", attributeIndices);
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
                    buffers.Add(new Buffer(weights, SkinnedMesh.MaxJointsSupported, 3));
                    buffers.Add(new Buffer(joints, SkinnedMesh.MaxJointsSupported, 4));
                }

                int vertexStride = buffers.Count;
                int verticesCount = indices.Length / vertexStride;

                Dictionary<int, ColladaVertex> verticesMap = new Dictionary<int, ColladaVertex>();
                List<int> newIndices = new List<int>();
                int vertexId = 0;

                for (int i = 0; i < verticesCount; i++)
                {
                    int positionIndex = indices[i * vertexStride];
                    if (verticesMap.TryGetValue(positionIndex, out ColladaVertex exist))
                    {
                        newIndices.Add(exist.Id);
                        continue;
                    }
                    var vertex = new ColladaVertex(buffers, vertexId++);
                    newIndices.Add(vertex.Id);
                    verticesMap.Add(positionIndex, vertex);
                    for (int bufferIndex = 0; bufferIndex < buffers.Count; bufferIndex++)
                    {
                        int index = indices[i * vertexStride + buffers[bufferIndex].Offset];
                        vertex.SetIndex(bufferIndex, index);
                    }
                }
                var verticesArray = verticesMap.Values.ToArray();

                var resultBuffers = buffers.Select(b => Array.CreateInstance(b.Array.GetType().GetElementType(), verticesArray.Length * b.Stride)).ToArray();

                for (int i = 0; i < verticesArray.Length; i++)
                {
                    for (int buffer = 0; buffer < buffers.Count; buffer++)
                    {
                        verticesArray[i].HandleBuffer(buffer, buffers[buffer].Array, resultBuffers[buffer], i * buffers[buffer].Stride);
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
                            (float[])resultBuffers[0],
                            (float[])resultBuffers[1],
                            (float[])resultBuffers[2],
                            (float[])resultBuffers[3],
                            (int[])resultBuffers[4],
                            newIndices.ToArray(),
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
                            (float[])resultBuffers[0],
                            (float[])resultBuffers[1],
                            (float[])resultBuffers[2],
                            newIndices.ToArray(),
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
            int maxJoints = SkinnedMesh.MaxJointsSupported;
            int stride = 2;
            weights = new float[numbers.Length * maxJoints];
            joints = new int[numbers.Length * maxJoints];

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
                    Vector3 vertexWeight = new Vector3(vertexWeights[0].weight, vertexWeights[1].weight, vertexWeights[2].weight);
                    float sum = vertexWeight.x + vertexWeight.y + vertexWeight.z;
                    if (sum != 0) vertexWeight /= sum;

                    vertexWeights[0].weight = vertexWeight.x;
                    vertexWeights[1].weight = vertexWeight.y;
                    vertexWeights[2].weight = vertexWeight.z;
                }

                for (int i = 0; i < maxJoints; i++)
                {
                    weights[n * maxJoints + i] = vertexWeights[i].weight;
                    joints[n * maxJoints + i] = vertexWeights[i].joint;
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
