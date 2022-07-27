using OneEngine;
using OneEngine.IO;
using System.IO;

namespace OneEngineGame
{
    public sealed class FpsRenderer : Renderer
    {
        protected override void OnGraphicsUpdate(IGraphics graphics, Camera camera)
        {
            graphics.Brush = new SingleColorBrush(Color32.white, 10);
            //graphics.Transform = Matrix3x3.identity;
            graphics.DrawText(new Vector2(50f, 50f), $"Delta time : {Time.deltaTime}", 25);
        }

        protected override bool IsInsideScreen(IGraphics graphics, Camera camera)
        {
            return true;
        }
    }
}
