using RomeEngine;

namespace RomeEngineCubeWorld
{
    public interface ICube : ILocatable
    {
        CubeCoords Location { get; }
        Bounds Bounds { get; }
        int Id { get; }
        ICubeChunk Chunk { get; }
    }
}
