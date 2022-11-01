namespace RomeEngineCubeWorld
{
    public interface ICubeChunk
    {
        bool TryGetCube(CubeCoords coords, out Cube cube);
    }
}
