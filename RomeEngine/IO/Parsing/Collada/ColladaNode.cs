using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaNode
    {
        public ColladaNode(IColladaNode reader)
        {
            attributes = reader.Attributes;
            Id = attributes.FirstOrDefault(a => a.Name == "id")?.Value ?? string.Empty;
            NodeType = reader.NodeType;
            Value = reader.GetValue();
        }

        public ColladaNode ChangeType(string type) => ((NodeType, _) = (type, this)).Item2;

        public string NodeType { get; private set; }
        public string Id { get; }
        public string Value { get; }

        public override string ToString() => $"{NodeType} " + string.Join(", ", attributes.Select(a => $"{a.Name}={a.Value}"));

        List<ColladaNode> children = new List<ColladaNode>();
        ReadOnlyArray<ColladaNodeAttribute> attributes;

        public void AddChild(ColladaNode child)
        {
            children.Add(child);
        }

        public ColladaEntity BuildEntity()
        {
            return new ColladaEntity(NodeType, Value, attributes.ToArray(), children.Select(c => c.BuildEntity()).ToArray());
        }
    }
}
