﻿using RomeEngine;

namespace RomeEngineCubeWorld
{
    public interface ICubeSystem
    {
        void ModifyCube(ICubeModifier modifier, CubeCoords coords);
        void RaycastCubesNonAlloc(Ray ray, IBuffer<ICube> buffer);
    }
}
