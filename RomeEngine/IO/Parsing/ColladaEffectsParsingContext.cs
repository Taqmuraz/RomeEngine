using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaEffectsParsingContext : ColladaParsingContext<ColladaEffectsParsingContext, ColladaEffect>
    {
        ReadOnlyArrayList<ColladaMaterial> previousStageMaterials;

        public ColladaEffectsParsingContext(ReadOnlyArrayList<ColladaMaterial> previousStageMaterials, ColladaSemanticModel semanticModel) : base(semanticModel)
        {
            this.previousStageMaterials = previousStageMaterials;
        }

        public override string RootNodeName => "library_effects";

        protected override IEnumerable<IColladaNodeHandler<ColladaEffectsParsingContext>> CreateHandlers()
        {
            yield return new ColladaDelegateHandler<ColladaEffectsParsingContext>("effect", (context, node) => context.PushElement(new ColladaEffect(node.GetAttribute("id"))), (context, node) => context.PopElement());
            yield return new ColladaDelegateHandler<ColladaEffectsParsingContext>("init_from", (context, node) =>
            {
                context.SemanticModel.AddSemantic(new ColladaSemantic(node.GetValue(), context.CurrentElement.EffectName));
            }, null);
        }
        public override void UpdateGameObject(GameObject gameObject, IColladaParsingInfo parsingInfo)
        {
            var skinnedMeshes = gameObject.GetComponentsOfType<MeshRenderer>();
            for (int i = 0; i < skinnedMeshes.Length; i++)
            {
                var material = (SingleTextureMaterial)skinnedMeshes[i].Material;

                if (material.MaterialName == null) continue;

                var materialName = SemanticModel.GetSemantic(material.MaterialName).Value;
                var effectName = SemanticModel.GetSemantic(materialName).Value;
                var imageName = SemanticModel.GetSemantic(effectName).Value;
                var textureFileName = SemanticModel.GetSemantic(imageName).Value;

                var fs = Engine.Instance.Runtime.FileSystem;
                material.TextureFileName = fs.CombinePath(fs.GetParentDirectory(parsingInfo.SourceFile), textureFileName);
            }
        }
        protected override ColladaEffectsParsingContext GetContext() => this;
    }
}
