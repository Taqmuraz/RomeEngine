using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaMaterialBuilder : IColladaBuilder
    {
        public void BuildGameObject(GameObject gameObject, ColladaEntity rootEntity, ColladaParsingInfo info)
        {
            var materialBindings = rootEntity["library_visual_scenes"].Single().Search("instance_material");
            var materials = rootEntity["library_materials"]["material"];
            var effects = rootEntity["library_effects"]["effect"];
            var images = rootEntity["library_images"]["image"];

            var renderers = gameObject.GetComponentsOfType<MeshRenderer>();
            foreach (var renderer in renderers)
            {
                var material = renderer.Material as SingleTextureMaterial;
                if (material == null) continue;

                var materianBindingEntity = materialBindings.FirstOrDefault(b => b.Properties["symbol"].Value == material.MaterialName);
                if (materianBindingEntity == null) continue;

                var materialTarget = materianBindingEntity.Properties["target"].Value;
                var materialEntity = materials.FirstOrDefault(m => m.Properties["id"].Value == materialTarget);
                if (materialEntity == null) continue;

                var effectName = materialEntity["instance_effect"].Single().Properties["url"];
                var effectEntity = effects.FirstOrDefault(e => e.Properties["id"] == effectName);
                if (effectEntity == null) continue;

                var surfaceEntity = effectEntity["profile_common"]["newparam"]["surface"]["init_from"].FirstOrDefault();
                if (surfaceEntity == null) continue;
                var imageName = surfaceEntity.Value;
                var imageEntity = images.FirstOrDefault(i => i.Properties["id"].Value == imageName);
                if (imageEntity == null) continue;

                var fs = info.FileSystem;
                material.TextureFileName = fs.RelativePath(fs.CombinePath(fs.GetParentDirectory(info.SourceFilePath), imageEntity["init_from"].Single().Value));
            }
        }
    }
}
