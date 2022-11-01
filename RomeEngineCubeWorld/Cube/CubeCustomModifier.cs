using System;

namespace RomeEngineCubeWorld
{
    public sealed class CubeCustomModifier : ICubeModifier
    {
        Func<Cube, Cube> modifier;

        public CubeCustomModifier(Func<Cube, Cube> modifier)
        {
            this.modifier = modifier;
        }

        public Cube ModifyCube(Cube cube) => modifier(cube);
    }
}
