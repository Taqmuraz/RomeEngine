using RomeEngine;

namespace RomeEngineCubeWorld
{
    public interface ICubeSystem
    {
        void ModifyCube(ICubeModifier modifier, CubeCoords coords);
        bool RaycastCube(Ray ray, out CubeCoords coords);
        CubeCoords[] RaycastCubes(Ray ray);
    }
}
