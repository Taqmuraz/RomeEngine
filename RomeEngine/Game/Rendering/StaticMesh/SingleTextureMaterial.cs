using RomeEngine.IO;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class SingleTextureMaterial : Material
    {
        public SingleTextureMaterial()
        {
        }
        public SingleTextureMaterial(string name)
        {
            MaterialName = name;
        }

        [SerializeField] public string MaterialName { get; }
        [SerializeField] public string TextureFileName { get; set; }
        Texture texture;

        public override void VisitContext(IGraphicsContext context)
        {
            if (TextureFileName != null) texture = context.LoadTexture(TextureFileName);
        }

        public override void PrepareDraw(IGraphics graphics)
        {
            graphics.SetTexture(texture, TextureType.Albedo);
        }
        public override IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new GenericSerializableField<string>(nameof(TextureFileName), TextureFileName, value => TextureFileName = value, true);
        }
    }
}
