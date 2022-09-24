using System.Xml;

namespace RomeEngine.IO
{
    public sealed class ColladaXmlReaderNode : IColladaNode
    {
        XmlReader reader;

        public ColladaXmlReaderNode(XmlReader reader)
        {
            this.reader = reader;
        }

        public string GetAttribute(string name)
        {
            return reader.GetAttribute(name);
        }

        public string GetValue()
        {
            if (reader.Read()) return reader.Value;
            else throw new System.InvalidOperationException("Can't read value from end of XML stream");
        }

        public string GetName()
        {
            return reader.Name;
        }
    }
}
