namespace RomeEngine.IO
{
    public sealed class ColladaMaterial
    {
        public ColladaMaterial(string materialName)
        {
            MaterialName = materialName;
        }

        public string MaterialName { get; }
        public string EffectName { get; set; }
    }
}
