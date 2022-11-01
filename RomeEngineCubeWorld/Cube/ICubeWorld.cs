using RomeEngine;
using System;
using System.Collections.Generic;

namespace RomeEngineCubeWorld
{
    public interface ICubeWorld : ICubeSystem
    {
        bool RaycastCube(Ray ray, out ICube coords);
        IAsyncProcessHandle RaycastCubeAsync(Ray ray, Action<ICube> callback);
        IAsyncProcessHandle RaycastCubesAsunc(Ray ray, Action<IEnumerable<ICube>> callback);
    }
}
