using RomeEngine;

namespace RomeEngineCubeWorld
{
    public interface ICubeChunk : ICubeInfoProvider, ILocatable
    {
        void ModifyCube(ICubeModifier modifier, CubeCoords coords);
        bool TryGetCube(CubeCoords coords, out Cube cube);
        Bounds Bounds { get; }
        CubeCoords Position { get; }
        void RebuildMesh();
    }
}
