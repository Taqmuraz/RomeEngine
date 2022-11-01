namespace RomeEngineCubeWorld
{
    public interface ICubeWorld
    {
        void ModifyCube(ICubeModifier modifier, CubeCoords coords);
    }
}
