namespace RomeEngine.IO
{
    public struct ColladaVertexAttributeProperty
    {
        public ColladaVertexAttributeProperty(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public string Type { get; }

        public override string ToString() => $"Attribute property : name = {Name}, type = {Type}";
    }
}
