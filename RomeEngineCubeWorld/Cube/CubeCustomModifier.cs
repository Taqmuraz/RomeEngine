using System;

namespace RomeEngineCubeWorld
{
    public sealed class CubeCustomModifier : ICubeModifier
    {
        Action<ICube> modifier;

        public CubeCustomModifier(Action<ICube> modifier)
        {
            this.modifier = modifier;
        }

        public void ModifyCube(ICube cube) => modifier(cube);
    }
}
