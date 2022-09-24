namespace RomeEngine.IO
{
    public sealed class ColladaEffect
    {
        public ColladaEffect(string effectName)
        {
            EffectName = effectName;
        }

        public string EffectName { get; }
        public string TextureFileName { get; set; }
    }
}
