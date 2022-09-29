using OpenTK.Graphics.OpenGL;
using RomeEngine;

namespace RomeEngineOpenGL
{
    abstract class Style2D
    {
        static int depth;
        OpenGLTextRenderer textRenderer;

        public Style2D(IGraphicsContext context)
        {
            textRenderer = new OpenGLTextRenderer(context);
        }

        public static void Setup()
        {
            depth = 0;
        }
        public static float Depth => -depth * 0.0001f;
        public static void NextDepth() => depth++;

        protected void MatrixVertex(Vector2 vertex, ref Matrix3x3 matrix)
        {
            vertex = matrix.MultiplyPoint(vertex);
            GL.Vertex3(vertex.x, vertex.y, Depth);
        }

        public void DrawText(string text, Rect rect, Color32 color, TextOptions options)
        {
            var viewportInv = Matrix4x4.CreateViewport(Screen.Size.x, Screen.Size.y).GetInversed();
            rect = Rect.FromCenterAndSize(viewportInv.MultiplyPoint(rect.Center), viewportInv.MultiplyVector(rect.Size));
            textRenderer.DrawText(text, rect, color, options);
        }

        public void RenderScene()
        {
            textRenderer.RenderScene();
        }
    }
}
