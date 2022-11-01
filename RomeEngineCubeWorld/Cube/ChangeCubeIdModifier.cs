namespace RomeEngineCubeWorld
{
    public sealed class ChangeCubeIdModifier : ICubeModifier
    {
        int id;

        public ChangeCubeIdModifier(int id)
        {
            this.id = id;
        }

        public void ModifyCube(ICube cube)
        {
            cube.ChangeId(id);
        }
    }
}
