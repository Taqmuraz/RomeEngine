﻿using RomeEngine;
using RomeEngineMeshGeneration;

namespace RomeEngineCubeWorld
{
    public interface ICube : IMeshElementGenerator, ILocatable
    {
        CubeCoords Position { get; }
        Bounds Bounds { get; }
        int Id { get; }
        ICubeChunk Chunk { get; }

        void ChangeId(int id);
    }
}
