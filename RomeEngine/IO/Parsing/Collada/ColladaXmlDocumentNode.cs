using System.Linq;
using System.Xml;

namespace RomeEngine.IO
{
    public sealed class ColladaXmlDocumentNode : IColladaNode
    {
        XmlNode node;

        public ColladaXmlDocumentNode(XmlNode node)
        {
            this.node = node;
            Attributes = node.Attributes == null ? new ColladaNodeAttribute[0] : node.Attributes.Cast<XmlAttribute>().Select(a => new ColladaNodeAttribute(a.Name, a.Value)).ToArray();
        }

        public ReadOnlyArray<ColladaNodeAttribute> Attributes { get; }
        public string NodeType => node.Name;

        public ColladaNodeAttribute GetAttribute(string name) => Attributes.First(a => a.Name == name);

        public string GetValue() => node.ChildNodes.Cast<XmlNode>().FirstOrDefault(c => c.NodeType == XmlNodeType.Text)?.Value;

        public bool HasAttribute(string name) => Attributes.Any(a => a.Name == name);
    }
}
