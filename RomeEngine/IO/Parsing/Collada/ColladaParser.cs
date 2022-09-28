using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RomeEngine.IO
{
    public sealed class ColladaParser : IParser
    {
        public bool CanParse(string fileName)
        {
            return Engine.Instance.Runtime.FileSystem.GetFileExtension(fileName).ToLower() == ".dae";
        }

        public ISerializable ParseObject(string fileName)
        {
            Stack<ColladaNode> nodesStack = new Stack<ColladaNode>();
            ColladaNode rootNode = null;
            ColladaContext context = new ColladaContext();

            void PushNode(ColladaNode node)
            {
                if (rootNode == null) rootNode = node;
                nodesStack.Push(node);
            }
            void PopNode()
            {
                nodesStack.Pop();
            }
            int Depth () => nodesStack.Count;
            ColladaNode CurrentNode () => nodesStack.Peek();

            IColladaNodeHandler defaultHandler = new ColladaDelegateNodeHandler(_ => true, node =>
            {
                var newNode = new ColladaNode(node);

                if (node.HasAttribute("id"))
                {
                    context.AddSource(node.GetAttribute("id").Value, newNode);
                }

                if (Depth() > 0) CurrentNode().AddChild(newNode);
                PushNode(newNode);
            },
            node => PopNode());

            IColladaNodeHandler[] handlers = new IColladaNodeHandler[]
            {
                new ColladaDelegateNodeHandler("input", node => CurrentNode().AddChild(context.GetSource(node.GetAttribute("source").Value).ChangeType(node.GetAttribute("semantic").Value.ToLower()).AddAttributes(node.Attributes)), null),

                defaultHandler
            };

            var xmlDocument = new XmlDocument();
            var result = new GameObject("Collada Model");
            xmlDocument.Load(fileName);

            void TraceNode(XmlNode node)
            {
                void Further()
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        TraceNode(child);
                    }
                }

                if (node.NodeType == XmlNodeType.Element)
                {
                    var colladaNode = new ColladaXmlDocumentNode(node);
                    var handler = handlers.First(h => h.CanParse(colladaNode));
                    handler.ParseStart(colladaNode);
                    Further();
                    handler.ParseEnd(colladaNode);
                }
                else
                {
                    Further();
                }
            }

            TraceNode(xmlDocument.DocumentElement);

            var colladaEntity = rootNode.BuildEntity();

            IColladaBuilder[] builders = new IColladaBuilder[]
            {
                new ColladaMeshBuilder()
            };

            foreach (var builder in builders)
            {
                builder.BuildGameObject(result, colladaEntity);
            }

            return result;
        }
    }
}
