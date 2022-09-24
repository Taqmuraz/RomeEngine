using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RomeEngine.IO
{
    public sealed class ColladaParser : IParser
    {
        Dictionary<string, IColladaParsingContext> contexts = new Dictionary<string, IColladaParsingContext>();

        public ColladaParser()
        {
            contexts = new IColladaParsingContext[]
            {
               new ColladaGeometryParsingContext(),
            }
            .ToDictionary(h => h.RootNodeName);
        }

        public bool CanParse(string fileName)
        {
            return Engine.Instance.Runtime.FileSystem.GetFileExtension(fileName).ToLower() == ".dae";
        }

        public ISerializable ParseObject(string fileName)
        {
            IColladaParsingContext currentContext = null;
            var xmlReader = XmlReader.Create(fileName);
            var readerNode = new ColladaXmlReaderNode(xmlReader);
            var result = new GameObject("Collada Model");

            while (xmlReader.Read()) 
            {
                if (xmlReader.Depth == 1)
                {
                    if (contexts.TryGetValue(xmlReader.Name, out IColladaParsingContext context))
                    {
                        currentContext = xmlReader.IsStartElement() ? context : null;
                    }
                    else
                    {
                        currentContext = null;
                    }
                }
                else
                {
                    if (currentContext != null)
                    {
                        if (xmlReader.IsStartElement()) currentContext.ParseStart(readerNode);
                        else currentContext.ParseEnd(readerNode);
                    }
                }
            }
            return result;
        }
    }
}
