namespace RomeEngineCubeWorld
{
    public interface ICubeChunk : ICubeInfoProvider
    {
        bool TryGetCube(CubeCoords coords, out Cube cube);
    }
}
