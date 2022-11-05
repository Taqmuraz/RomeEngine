using RomeEngine;
using RomeEngine.IO;
using RomeEngineMeshGeneration;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngineCubeWorld
{
    public sealed class CubeChunk : ICubeChunk, IMeshGenerationProvider, IMeshBuilder, IMeshDataDescriptor
    {
        sealed class CubeLocator : ILocatable
        {
            CubeCoords cubeCoords;

            public CubeLocator(CubeCoords cubeCoords)
            {
                this.cubeCoords = cubeCoords;
            }

            public bool IsInsideBox(Bounds box)
            {
                return box.IntersectsWith(Bounds.FromMinSize(cubeCoords, Vector3.one));
            }

            public ICube GetCube(CubeChunk chunk) => chunk.cubes[cubeCoords.x, cubeCoords.y, cubeCoords.z];
        }

        const int standardChunkWidth = 16;
        const int standardChunkHeight = 256;
        ICube[,,] cubes;
        ICubeTextureProvider defaultProvider = new CubeDefaultTextureProvider(4, 4);
        CubeCoords position;
        CubeCoords size;
        CubeChunkMeshRenderer chunkRenderer;
        static Octotree<CubeLocator> cubesTree;
        ICubeWorld world;
        bool hasChanges;

        public CubeCoords Size => size;
        ICubeWorld ICubeChunk.World => world;

        static CubeChunk()
        {
            var size = new CubeCoords(standardChunkWidth, standardChunkHeight, standardChunkWidth);
            cubesTree = new Octotree<CubeLocator>(Bounds.FromMinMax(new Vector3(), size), 4, 4);
            
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int z = 0; z < size.z; z++)
                    {
                        cubesTree.AddLocatable(new CubeLocator(new CubeCoords(x, y, z)));
                    }
                }
            }
        }

        public CubeChunk(CubeCoords position)
        {
            chunkRenderer = new GameObject($"Chunk {position}").ActivateForActiveScene().AddComponent<CubeChunkMeshRenderer>();
            chunkRenderer.Material = new SingleTextureMaterial("Grass") { TextureFileName = "./Resources/Textures/BlocksMap.jpg" };
            chunkRenderer.Transform.Position = position;

            this.position = position;
            this.size = new CubeCoords(standardChunkWidth, standardChunkHeight, standardChunkWidth);

            int length = size.x * size.y * size.z;
            cubes = new Cube[size.x, size.y, size.z];
            for (int i = 0; i < length; i++)
            {
                GetCorrdsFromIndex(i, out CubeCoords coords);
                cubes[coords.x, coords.y, coords.z] = new Cube(this, coords);
            }
        }

        void ICubeChunk.Initialize(ICubeWorld world)
        {
            this.world = world;
        }

        public void ModifyCube(ICubeModifier modifier, CubeCoords coords)
        {
            if (CheckCoords(coords))
            {
                modifier.ModifyCube(cubes[coords.x, coords.y, coords.z]);
                hasChanges = true;
            }
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

        bool ICubeSystem.TryGetCube(CubeCoords coords, out ICube cube)
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

        IAsyncProcessHandle lastMeshBuildProcess;
        void ICubeChunk.Rebuild()
        {
            if (hasChanges)
            {
                hasChanges = false;

                if (lastMeshBuildProcess != null && lastMeshBuildProcess.IsRunning) lastMeshBuildProcess.Abort();

                var process = new AsyncProcess<int>(() =>
                {
                    chunkRenderer.UpdateMesh(MeshGenerator.GenerateMesh(this));

                    return 0;
                }, _ => { });

                lastMeshBuildProcess = process.Start();
            }
        }

        CubeCoords ICubeChunk.Position => position;

        public void RaycastCubesNonAlloc(Ray ray, IBuffer<ICube> buffer)
        {
            cubesTree.VisitTree(new CustomTreeAcceptor<CubeLocator>(locators =>
            {
                foreach (var locator in locators)
                {
                    var cube = locator.GetCube(this);
                    if (cube.Id == Cube.AirCubeId) continue;
                    if (cube.Bounds.IntersectsRay(ray))
                    {
                        if (!buffer.Write(cube)) return;
                    }
                }
            }, box => box.IntersectsRay(new Ray(ray.origin - position, ray.direction))));
        }
    }
}
