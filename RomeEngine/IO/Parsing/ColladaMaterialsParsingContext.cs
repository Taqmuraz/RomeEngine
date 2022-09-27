using System.Collections.Generic;

namespace RomeEngine.IO
{
    public sealed class ColladaMaterialsParsingContext : ColladaParsingContext<ColladaMaterialsParsingContext, ColladaMaterial>
    {
        public ColladaMaterialsParsingContext(ColladaSemanticModel semanticModel) : base(semanticModel)
        {
        }

        public ColladaEffectsParsingContext CreateEffectContext()
        {
            return new ColladaEffectsParsingContext(Elements, SemanticModel);
        }

        protected override IEnumerable<IColladaNodeHandler<ColladaMaterialsParsingContext>> CreateHandlers()
        {
            yield return new ColladaDelegateHandler<ColladaMaterialsParsingContext>("material", (context, node) => context.PushElement(new ColladaMaterial(node.GetAttribute("id"))), (context, node) => context.PopElement());
            yield return new ColladaDelegateHandler<ColladaMaterialsParsingContext>("instance_effect", (context, node) => context.SemanticModel.AddSemantic(new ColladaSemantic(node.GetAttribute("url"), context.CurrentElement.MaterialName)), null);
        }

        protected override ColladaMaterialsParsingContext GetContext() => this;

        public override void UpdateGameObject(GameObject gameObject, IColladaParsingInfo parsingInfo)
        {

        }

        public override string RootNodeName => "library_materials";
    }
}
