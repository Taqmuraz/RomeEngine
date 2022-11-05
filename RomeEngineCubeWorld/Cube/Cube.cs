using System;
using System.Collections.Generic;
using System.Linq;
using RomeEngine;
using RomeEngineMeshGeneration;

namespace RomeEngineCubeWorld
{
    public class Cube : ICube, IMeshElementGenerator
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
                        buffer.Write(uv.x);
                        buffer.Write(uv.y);
                        break;
                    case 2:
                        buffer.Write(normal.x);
                        buffer.Write(normal.y);
                        buffer.Write(normal.z);
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

            new Vector2(1f, 1f),
            new Vector2(0f, 1f),
            new Vector2(0f, 0f),
        };

        ICubeChunk chunk;
        CubeCoords coords;
        int cubeId;
        public const int AirCubeId = 0;

        int ICube.Id => cubeId;
        ICubeChunk ICube.Chunk => chunk;
        public CubeCoords Position => coords + chunk.Position;
        public Bounds Bounds => Bounds.FromMinSize(Position, Vector3.one);

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

        public void SetId(int id)
        {
            cubeId = id;
        }

        static Vector2 TransformUv(Cube cube, Vector2 uv)
        {
            Rect rect = cube.chunk.TextureProvider.GetUvRect(cube.cubeId);
            return rect.min + rect.Size * uv;
        }

        static void WriteSide(IMeshStream stream, Cube cube, int a, int b, int c, int d)
        {
            stream.PushStartIndex();
            int[] rawIndices = new int[] { a, b, c, c, d, a };
            stream.WriteIndices(Enumerable.Range(0, rawIndices.Length));
            Vector3 normal = Vector3.Cross(cubeVertices[b] - cubeVertices[a], cubeVertices[d] - cubeVertices[a]).Normalized;
            int uvIndex = 0;
            stream.WriteVertices(rawIndices.Select(i => new CubeVertex(cubeVertices[i] + cube.coords, normal, TransformUv(cube, cubeUVs[uvIndex++]))));
        }

        static bool IsEmptySpace(ICubeChunk chunk, CubeCoords coords)
        {
            ICubeSystem system = chunk;
            if (coords.x == -1 || coords.y == -1 || coords.z == -1 || coords.x == chunk.Size.x || coords.y == chunk.Size.y || coords.z == chunk.Size.z)
            {
                system = chunk.World;
                coords += chunk.Position;
            }

            return !system.TryGetCube(coords, out ICube cube) || cube.Id == AirCubeId;
        }

        void IMeshElementGenerator.WriteElement(IMeshStream stream)
        {
            if (cubeId == AirCubeId) return;

            if (IsEmptySpace(chunk, coords + new CubeCoords(0, 1, 0))) WriteSide(stream, this, 0, 3, 2, 1);
            if (IsEmptySpace(chunk, coords + new CubeCoords(0, -1, 0))) WriteSide(stream, this, 4, 5, 6, 7);
            if (IsEmptySpace(chunk, coords + new CubeCoords(0, 0, -1))) WriteSide(stream, this, 0, 1, 5, 4);
            if (IsEmptySpace(chunk, coords + new CubeCoords(1, 0, 0))) WriteSide(stream, this, 1, 2, 6, 5);
            if (IsEmptySpace(chunk, coords + new CubeCoords(0, 0, 1))) WriteSide(stream, this, 2, 3, 7, 6);
            if (IsEmptySpace(chunk, coords + new CubeCoords(-1, 0, 0))) WriteSide(stream, this, 3, 0, 4, 7);
        }

        void ICube.ChangeId(int id)
        {
            this.cubeId = id; 
        }
    }
}
