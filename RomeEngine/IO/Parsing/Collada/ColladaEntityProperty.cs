namespace RomeEngine.IO
{
    public sealed class ColladaEntityProperty
    {
        public ColladaEntityProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }

        public override string ToString() => $"{Name}={Value}";
    }
}
