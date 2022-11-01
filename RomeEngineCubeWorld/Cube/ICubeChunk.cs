using RomeEngine;

namespace RomeEngineCubeWorld
{
    public interface ICubeChunk : ICubeSystem, ICubeInfoProvider, ILocatable
    {
        bool TryGetCube(CubeCoords coords, out ICube cube);
        Bounds Bounds { get; }
        CubeCoords Position { get; }
        void Rebuild();
    }
}
