using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaEffectsParsingContext : ColladaParsingContext<ColladaEffectsParsingContext, ColladaEffect>
    {
        ReadOnlyArrayList<ColladaMaterial> previousStageMaterials;

        public ColladaEffectsParsingContext(ReadOnlyArrayList<ColladaMaterial> previousStageMaterials)
        {
            this.previousStageMaterials = previousStageMaterials;
        }

        public override string RootNodeName => "library_effects";

        protected override IEnumerable<IColladaNodeHandler<ColladaEffectsParsingContext>> CreateHandlers()
        {
            yield return new ColladaDelegateHandler<ColladaEffectsParsingContext>("effect", (context, node) => context.PushElement(new ColladaEffect(node.GetAttribute("id"))), (context, node) => context.PopElement());
            yield return new ColladaDelegateHandler<ColladaEffectsParsingContext>("init_from", (context, node) => context.CurrentElement.TextureFileName = node.GetValue(), null);
        }
        public override void UpdateGameObject(GameObject gameObject, IColladaParsingInfo parsingInfo)
        {
            var effects = Elements;
            var skinnedMeshes = gameObject.GetComponentsOfType<MeshRenderer>();
            for (int i = 0; i < skinnedMeshes.Length; i++)
            {
                var material = (SingleTextureMaterial)skinnedMeshes[i].Material;
                var colladaMaterial = previousStageMaterials.FirstOrDefault(m => m.MaterialName == material.MaterialName);

                if (colladaMaterial == null) continue;
                var effect = effects.FirstOrDefault(e => e.EffectName == colladaMaterial.EffectName);

                var fs = Engine.Instance.Runtime.FileSystem;
                material.TextureFileName = fs.GetFiles(fs.GetParentDirectory(parsingInfo.SourceFile)).FirstOrDefault(p => fs.GetFileNameWithoutExtension(p).Equals(effect.TextureFileName.Replace("_png", string.Empty)));
                skinnedMeshes[i].Material = material;
            }
        }
        protected override ColladaEffectsParsingContext GetContext() => this;
    }
}
