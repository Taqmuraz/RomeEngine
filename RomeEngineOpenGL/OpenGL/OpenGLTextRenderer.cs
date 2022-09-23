using OpenTK.Graphics.OpenGL;
using RomeEngine;
using System.Text;

namespace RomeEngineOpenGL
{
    class OpenGLTextRenderer : OpenGLCommonGraphics
    {
        OpenGLTexture fontTexture;
        Encoding encoding;
        OpenGLShader textShader;
        IMeshIdentifier boxMesh;
        Rect boxRect;
        Rect uvRect;
        Color32 textColor;

        protected override OpenGLShader ActiveShader => textShader;

        protected override void SetupShader(OpenGLShader shader)
        {
            shader.SetVector4("vertexRect", new Vector4(boxRect.X, boxRect.Y, boxRect.Width, boxRect.Height));
            shader.SetVector4("textRect", new Vector4(uvRect.X, uvRect.Y, uvRect.Width, uvRect.Height));
            textShader.SetVector4("textureColor", textColor.ToVector4());
        }

        public OpenGLTextRenderer(IGraphicsContext context)
        {
            fontTexture = (OpenGLTexture)context.LoadTexture("./Resources/Fonts/CenturyCP855.png");
            encoding = Encoding.GetEncoding("CP855");
            textShader = new OpenGLShader("Text");

            var mesh = new StaticMesh
                (
                    new Vertex[]
                    {
                        new Vertex
                        (
                            new Vector3(0f, 0f, 0f),
                            new Vector3(0f, 0f, -1f),
                            new Vector2(0f, 1f)
                        ),
                        new Vertex
                        (
                            new Vector3(0f, 1f, 0f),
                            new Vector3(0f, 0f, -1f),
                            new Vector2(0f, 0f)
                        ),
                        new Vertex
                        (
                            new Vector3(1f, 1f, 0f),
                            new Vector3(0f, 0f, -1f),
                            new Vector2(1f, 0f)
                        ),
                        new Vertex
                        (
                            new Vector3(1f, 0f, 0f),
                            new Vector3(0f, 0f, -1f),
                            new Vector2(1f, 1f)
                        ),
                    },
                    new int[]
                    {
                        0, 1, 2,
                        2, 3, 0
                    }
                );
            boxMesh = context.LoadMesh(mesh);
        }

        public void DrawText(string text, Rect rect, Color32 color)
        {
            float totalWidth = rect.Width * 0.9f;
            float totalHeight = rect.Height * 0.9f;
            float positionX = rect.X + rect.Width * 0.05f;
            float positionY = rect.Y + rect.Height * 0.05f;
            char lineSeparator = '\n';

            int linesCount = 1;
            int maxWidth = 0;
            int currentWidth = 0;

            for (int i = 0; i < text.Length; i++)
            {
                char symbol = text[i];
                if (symbol == lineSeparator)
                {
                    currentWidth = 0;
                    linesCount++;
                    continue;
                }
                currentWidth++;
                if (currentWidth > maxWidth) maxWidth = currentWidth;
            }

            float defaultWidth = 0.03f;
            float defaultHeight = 0.05f;
            float symbolWidth = Mathf.Min(totalWidth / maxWidth, defaultWidth);
            float symbolHeight = Mathf.Min(totalHeight / linesCount, defaultHeight);

            int x = 0;
            int y = 0;

            if (maxWidth == 0) return;

            byte[] textBytes = encoding.GetBytes(text);

            for (int i = 0; i < text.Length; i++)
            {
                char symbol = text[i];

                if (symbol == lineSeparator)
                {
                    x = 0;
                    y++;
                    continue;
                }

                float symbolCoordSize = 1f / 16f;
                float symbolCoordX = (textBytes[i] % 16) * symbolCoordSize;
                float symbolCoordY = (textBytes[i] / 16) * symbolCoordSize;

                SetTexture(fontTexture, TextureType.Albedo);

                boxRect = new Rect(positionX + symbolWidth * x, positionY + symbolHeight * y, symbolWidth, symbolHeight);
                uvRect = new Rect(symbolCoordX, symbolCoordY, symbolCoordSize, symbolCoordSize);
                textColor = color;

                DrawMesh(boxMesh);

                x++;
            }
        }
    }
}
