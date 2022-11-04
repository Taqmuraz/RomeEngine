namespace RomeEngineCubeWorld
{
    public sealed class CubeIdModifier : ICubeModifier
    {
        int id;

        public CubeIdModifier(int id)
        {
            this.id = id;
        }

        public void ModifyCube(ICube cube)
        {
            cube.ChangeId(id);
        }
    }
}
