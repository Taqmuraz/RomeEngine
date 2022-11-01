using RomeEngine;
using RomeEngine.IO;
using RomeEngineMeshGeneration;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngineCubeWorld
{
    public sealed class CubeChunk : ICubeChunk, IMeshGenerationProvider, IMeshBuilder, IMeshDataDescriptor
    {
        int standardChunkWidth = 16;
        int standardChunkHeight = 256;
        ICube[,,] cubes;
        ICubeTextureProvider defaultProvider = new CubeDefaultTextureProvider(4, 4);
        CubeCoords position;
        CubeCoords size;
        CubeChunkMeshRenderer chunkRenderer;
        Octotree<ICube> cubesTree;

        public int Width => standardChunkWidth;
        public int Height => standardChunkHeight;

        public CubeChunk(CubeCoords position)
        {
            chunkRenderer = new GameObject($"Chunk {position}").ActivateForActiveScene().AddComponent<CubeChunkMeshRenderer>();
            chunkRenderer.Material = new SingleTextureMaterial("Grass") { TextureFileName = "./Resources/Textures/BlocksMap.jpg" };

            this.position = position;
            this.size = new CubeCoords(standardChunkWidth, standardChunkHeight, standardChunkWidth);

            int length = size.x * size.y * size.z;
            cubes = new Cube[size.x, size.y, size.z];
            for (int i = 0; i < length; i++)
            {
                GetCorrdsFromIndex(i, out CubeCoords coords);
                cubes[coords.x, coords.y, coords.z] = new Cube(this, coords);
            }

            cubesTree = new Octotree<ICube>(Bounds, 4, 4);
            foreach (ICube cube in EnumerateCubes()) cubesTree.AddLocatable(cube);
        }

        public void ModifyCube(ICubeModifier modifier, CubeCoords coords)
        {
            modifier.ModifyCube(cubes[coords.x, coords.y, coords.z]);
        }

        void GetCorrdsFromIndex(int index, out CubeCoords coords)
        {
            coords = new CubeCoords()
            {
                x = index % size.x,
                z = (index / size.x) % size.z,
                y = (index / (size.x * size.z)),
            };
        }

        IEnumerable<ICube> EnumerateCubes()
        {
            int length = size.x * size.y * size.z;

            for (int i = 0; i < length; i++)
            {
                GetCorrdsFromIndex(i, out CubeCoords coords);
                yield return cubes[coords.x, coords.y, coords.z];
            }
        }

        IMesh IMeshBuilder.Build(int[] indices, IEnumerable<IVertexBuffer> buffers)
        {
            return new CubeChunkMesh(indices, buffers.ToArray());
        }

        IEnumerable<IMeshAttributeInfo> IMeshDataDescriptor.Attributes => CubeChunkMesh.ChunkMeshAttributes;

        IMeshDataDescriptor IMeshGenerationProvider.Descriptor => this;
        IMeshBuilder IMeshGenerationProvider.Builder => this;
        IEnumerable<IMeshElementGenerator> IMeshGenerationProvider.Elements => EnumerateCubes();

        bool CheckCoords(CubeCoords coords)
        {
            return 
                coords.x >= 0 && coords.x < size.x &&
                coords.y >= 0 && coords.y < size.y &&
                coords.z >= 0 && coords.z < size.z;
        }

        bool ICubeChunk.TryGetCube(CubeCoords coords, out ICube cube)
        {
            bool check = CheckCoords(coords);
            cube = check ? cubes[coords.x, coords.y, coords.z] : null;
            return check;
        }

        ICubeTextureProvider ICubeInfoProvider.TextureProvider => defaultProvider;

        bool ILocatable.IsInsideBox(Bounds box)
        {
            return Bounds.IntersectsWith(box);
        }

        public Bounds Bounds => Bounds.FromMinSize(position, size);

        Bounds ICubeChunk.Bounds { get; }

        IAsyncProcessHandle lastMeshBuildProcess;
        void ICubeChunk.Rebuild()
        {
            if (lastMeshBuildProcess != null && lastMeshBuildProcess.IsRunning) lastMeshBuildProcess.Abort();

            var process = new AsyncProcess<int>(() =>
            {
                chunkRenderer.UpdateMesh(MeshGenerator.GenerateMesh(this));

                return 0;
            }, _ => { });

            lastMeshBuildProcess = process.Start();
        }

        CubeCoords ICubeChunk.Position => position;

        public void RaycastCubesNonAlloc(Ray ray, IBuffer<ICube> buffer)
        {
            cubesTree.VisitTree(new CustomTreeAcceptor<ICube>(cubes =>
            {
                foreach (var cube in cubes)
                {
                    if (cube.Id == Cube.AirCubeId) continue;
                    if (cube.Bounds.IntersectsRay(ray))
                    {
                        if (!buffer.Write(cube)) return;
                    }
                }
            }, box => box.IntersectsRay(ray)));
        }
    }
}
