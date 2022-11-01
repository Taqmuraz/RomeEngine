using System;
using System.Collections.Generic;
using System.Linq;
using RomeEngine;
using RomeEngineMeshGeneration;

namespace RomeEngineCubeWorld
{
    public class Cube : IMeshElementGenerator
    {
        sealed class CubeVertex : IMeshVertex
        {
            Vector3 position;
            Vector3 normal;
            Vector2 uv;

            public CubeVertex(Vector3 position, Vector3 normal, Vector2 uv)
            {
                this.position = position;
                this.normal = normal;
                this.uv = uv;
            }

            public void WriteElement(int index, IVertexBuffer buffer)
            {
                switch (index)
                {
                    case 0: 
                        buffer.Write(position.x);
                        buffer.Write(position.y);
                        buffer.Write(position.z);
                        break;
                    case 1:
                        buffer.Write(normal.x);
                        buffer.Write(normal.y);
                        buffer.Write(normal.z);
                        break;
                    case 2:
                        buffer.Write(uv.x);
                        buffer.Write(uv.y);
                        break;
                }
            }
        }

        static Vector3[] cubeVertices = new Vector3[]
        {
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 1, 1),
            new Vector3(0, 1, 1),

            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
            new Vector3(0, 0, 1),
        };
        static Vector2[] cubeUVs = new Vector2[]
        {
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0f, 1f),
        };

        ICubeChunk chunk;
        CubeCoords coords;
        int cubeId;
        const int AirCubeId = 0;

        public Cube(ICubeChunk chunk, CubeCoords cubeCoords)
        {
            this.chunk = chunk;
            this.coords = cubeCoords;
            cubeId = AirCubeId;
        }

        public Cube(int cubeId, ICubeChunk chunk, CubeCoords cubeCoords)
        {
            this.chunk = chunk;
            this.cubeId = cubeId;
            this.coords = cubeCoords;
        }

        static void WriteSide(IMeshStream stream, CubeCoords cubeCoords, int a, int b, int c, int d)
        {
            stream.PushStartIndex();
            int[] indices = new int[] { a, b, c, b, c, d };
            stream.WriteIndices(indices);
            Vector3 normal = Vector3.Cross(cubeVertices[b] - cubeVertices[a], cubeVertices[d] - cubeVertices[a]).normalized;
            int uvIndex = 0;
            stream.WriteVertices(indices.Select(i => new CubeVertex(cubeVertices[i] + cubeCoords, normal, cubeUVs[uvIndex++])));
        }

        void IMeshElementGenerator.WriteElement(IMeshStream stream)
        {
            if (cubeId == AirCubeId) return;

            WriteSide(stream, coords, 0, 1, 2, 3);
            WriteSide(stream, coords, 0, 4, 5, 1);
            WriteSide(stream, coords, 1, 5, 6, 2);
            WriteSide(stream, coords, 2, 6, 7, 3);
            WriteSide(stream, coords, 3, 7, 4, 0);
            WriteSide(stream, coords, 4, 5, 6, 7);
        }
    }
}
