using System.Collections.Generic;

namespace RomeEngine.IO
{
    public sealed class ColladaImageParsingContext : ColladaParsingContext<ColladaImageParsingContext, ColladaImage>
    {
        public ColladaImageParsingContext(ColladaSemanticModel semanticModel) : base(semanticModel)
        {
        }

        public override string RootNodeName => "library_images";

        protected override IEnumerable<IColladaNodeHandler<ColladaImageParsingContext>> CreateHandlers()
        {
            yield return new ColladaDelegateHandler<ColladaImageParsingContext>("image", (context, node) => context.PushElement(new ColladaImage(node.GetAttribute("id"))), (context, node) => context.PopElement());
            yield return new ColladaDelegateHandler<ColladaImageParsingContext>("init_from", (context, node) => context.SemanticModel.AddSemantic(new ColladaSemantic(node.GetValue(), context.CurrentElement.Id)), null);
        }

        protected override ColladaImageParsingContext GetContext() => this;

        public override void UpdateGameObject(GameObject gameObject, IColladaParsingInfo parsingInfo)
        {

        }
    }
}
