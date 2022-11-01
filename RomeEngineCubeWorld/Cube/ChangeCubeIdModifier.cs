namespace RomeEngineCubeWorld
{
    public sealed class ChangeCubeIdModifier : ICubeModifier
    {
        int id;

        public ChangeCubeIdModifier(int id)
        {
            this.id = id;
        }

        public Cube ModifyCube(Cube cube)
        {
            return cube.WithId(id);
        }
    }
}
