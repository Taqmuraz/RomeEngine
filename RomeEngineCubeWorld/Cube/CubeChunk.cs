namespace RomeEngineCubeWorld
{
    public sealed class CubeChunk
    {
        int standardChunkWidth = 16;
        int standardChunkHeight = 256;
        Cube[,,] cubes;

        public CubeChunk()
        {
            int length = standardChunkWidth * standardChunkHeight * standardChunkWidth;
            cubes = new Cube[standardChunkWidth, standardChunkHeight, standardChunkWidth];
            for (int i = 0; i < length; i++)
            {
                int x = i % standardChunkWidth;
                int z = i / standardChunkWidth;
                int y = (i / (standardChunkWidth * standardChunkWidth));
                cubes[x, y, z] = new Cube();
            }
        }
        public void ModifyCube(ICubeModifier modifier, int x, int y, int z)
        {
            cubes[x, y, z] = modifier.ModifyCube(cubes[x, y, z]);
        }
    }
}
