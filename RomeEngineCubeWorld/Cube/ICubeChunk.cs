using RomeEngine;

namespace RomeEngineCubeWorld
{
    public interface ICubeChunk : ICubeSystem, ICubeInfoProvider, ILocatable
    {
        Bounds Bounds { get; }
        CubeCoords Position { get; }
        CubeCoords Size { get; }
        ICubeWorld World { get; }
        void Initialize(ICubeWorld world);
        void Rebuild();
    }
}
