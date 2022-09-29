using System.Drawing;
using RomeEngine;

namespace RomeEngineWindowsFormsApplication
{
    class BitmapTexture : Texture
    {
        public BitmapTexture(Bitmap image)
        {
            Image = image;
        }

        public Bitmap Image { get; }

        public override int Width => Image.Width;
        public override int Height => Image.Height;
    }
}
