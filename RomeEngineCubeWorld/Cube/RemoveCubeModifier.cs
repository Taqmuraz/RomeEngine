namespace RomeEngineCubeWorld
{
    public sealed class RemoveCubeModifier : ICubeModifier
    {
        public static ICubeModifier Instance { get; } = new RemoveCubeModifier();

        RemoveCubeModifier()
        {
        }

        public Cube ModifyCube(Cube cube)
        {
            return cube.WithId(0);
        }
    }
}
