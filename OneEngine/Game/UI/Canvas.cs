using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEngine.UI
{
    public class Canvas : Renderer
    {
        List<ICanvasElement> elements = new List<ICanvasElement>();

        protected override bool IsInsideScreen(IGraphics graphics, Camera camera)
        {
            return true;
        }
        protected override IEnumerable<RendererPass> EnumeratePasses()
        {
            yield return StandardPass;
        }

        protected override void OnGraphicsUpdate(IGraphics graphics, Camera camera)
        {
            foreach (var element in elements) element.Draw(graphics, camera);
        }

        protected override Matrix3x3 GetGraphicsTransform(Camera camera)
        {
            return Matrix3x3.identity;
        }
    }
    public interface ICanvasElement
    {
        void Draw(IGraphics graphics, Camera camera);
    }
    public sealed class CanvasRect : ICanvasElement
    {
        Rect rect;
        Color32 color;

        public void Draw(IGraphics graphics, Camera camera)
        {
            graphics.DrawRect(rect);
        }
    }
    public sealed class CanvasButton : ICanvasElement
    {
        Rect rect;
        string text;
        Color32 color;

        public CanvasButton(string text, Rect rect, Color32 color)
        {
            this.rect = rect;
            this.text = text;
            this.color = color;
        }

        public void Draw(IGraphics graphics, Camera camera)
        {
            graphics.Brush = new SingleColorBrush(color);
            graphics.DrawRect(rect);
            graphics.DrawText(text, rect);
        }
    }
}
