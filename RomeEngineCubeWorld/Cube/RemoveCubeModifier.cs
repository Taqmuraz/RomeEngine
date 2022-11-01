namespace RomeEngineCubeWorld
{
    public sealed class RemoveCubeModifier : ICubeModifier
    {
        public static ICubeModifier Instance { get; } = new RemoveCubeModifier();

        RemoveCubeModifier()
        {
        }

        public void ModifyCube(ICube cube)
        {
            cube.ChangeId(0);
        }
    }
}
