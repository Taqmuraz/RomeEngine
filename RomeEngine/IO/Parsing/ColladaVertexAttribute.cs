using System.Collections.Generic;

namespace RomeEngine.IO
{
    public sealed class ColladaVertexAttribute
    {
        List<ColladaVertexAttributeProperty> properties = new List<ColladaVertexAttributeProperty>();

        public ColladaVertexAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public int Size => properties.Count;
        public IEnumerable<ColladaVertexAttributeProperty> Properties => properties;

        public void AddProperty(string name, string type)
        {
            properties.Add(new ColladaVertexAttributeProperty(name, type));
        }

        public override string ToString() => $"Attribute : name = {Name}, size = {Size}";
    }
}
