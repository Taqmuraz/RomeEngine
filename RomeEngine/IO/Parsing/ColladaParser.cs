using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RomeEngine.IO
{
    public sealed class ColladaParser : IParser
    {
        Dictionary<string, IColladaParsingStage> stages = new Dictionary<string, IColladaParsingStage>();

        public ColladaParser()
        {
            stages = new IColladaParsingStage[]
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
                    if (stages.TryGetValue(xmlReader.Name, out var context))
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

            foreach (var context in stages.Values)
            {
                context.UpdateGameObject(result);
            }

            return result;
        }
    }
}
