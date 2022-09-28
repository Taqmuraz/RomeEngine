namespace RomeEngine.IO
{
    public sealed class ColladaNodeAttribute
    {
        public ColladaNodeAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }
    }
}
