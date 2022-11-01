﻿using RomeEngine;
using RomeEngineMeshGeneration;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngineCubeWorld
{
    public struct CubeCoords
    {
        public int x;
        public int y;
        public int z;

        public CubeCoords(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator Vector3 (CubeCoords coords) => new Vector3(coords.x, coords.y, coords.z);
    }
    public sealed class CubeChunk : ICubeChunk, IMeshGenerationProvider, IMeshBuilder, IMeshDataDescriptor
    {
        int standardChunkWidth = 16;
        int standardChunkHeight = 256;
        Cube[,,] cubes;

        public CubeChunk()
        {
            int length = standardChunkWidth * standardChunkHeight * standardChunkWidth;
            cubes = new Cube[standardChunkWidth, standardChunkHeight, standardChunkWidth];
            for (int i = 0; i < length; i++)
            {
                GetCorrdsFromIndex(i, out CubeCoords coords);
                cubes[coords.x, coords.y, coords.z] = new Cube(this);
            }
        }
        public void ModifyCube(ICubeModifier modifier, CubeCoords coords)
        {
            cubes[coords.x, coords.y, coords.z] = modifier.ModifyCube(cubes[coords.x, coords.y, coords.z]);
        }

        void GetCorrdsFromIndex(int index, out CubeCoords coords)
        {
            coords = new CubeCoords()
            {
                x = index % standardChunkWidth,
                z = (index / standardChunkWidth) % standardChunkWidth,
                y = (index / (standardChunkWidth * standardChunkWidth)),
            };
        }

        IEnumerable<Cube> EnumerateCubes()
        {
            int length = standardChunkWidth * standardChunkHeight * standardChunkWidth;

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
                coords.x >= 0 && coords.x < standardChunkWidth &&
                coords.y >= 0 && coords.y < standardChunkHeight &&
                coords.z >= 0 && coords.z < standardChunkWidth;
        }

        bool ICubeChunk.TryGetCube(CubeCoords coords, out Cube cube)
        {
            bool check = CheckCoords(coords);
            cube = check ? cubes[coords.x, coords.y, coords.z] : null;
            return check;
        }
    }
}
