using RomeEngine;

namespace RomeEngineOpenGL
{
    class OpenGLTexture : Texture
    {
        int width;
        int height;

        public OpenGLTexture(int width, int height, int textureIndex)
        {
            this.width = width;
            this.height = height;
            TextureIndex = textureIndex;
        }

        public override int Width => width;
        public override int Height => height;
        public int TextureIndex { get; }
    }
}
