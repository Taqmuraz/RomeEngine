using System;
using System.Collections.Generic;
using System.Linq;

namespace OneEngine.UI
{
    public sealed class Canvas : Renderer
    {
        List<ICanvasElement> elements = new List<ICanvasElement>();
        HashSet<int> handles = new HashSet<int>();

        protected override bool IsInsideScreen(IGraphics graphics, Camera camera)
        {
            return true;
        }
        protected override IEnumerable<RendererPass> EnumeratePasses()
        {
            yield return OutlinePass;
            yield return StandardPass;
        }

        protected override void OnGraphicsUpdate(IGraphics graphics, Camera camera)
        {
            foreach (var element in elements) element.Draw(graphics, camera);
        }

        [BehaviourEvent]
        void OnPostRender()
        {
            elements.Clear();
        }

        protected override Matrix3x3 GetGraphicsTransform(Camera camera)
        {
            return Matrix3x3.identity;
        }

        public void DrawRect(Rect rect, Color32 color)
        {
            elements.Add(new CanvasRect(rect, color));
        }
        public void DrawText(string text, Rect rect, Color32 color, TextOptions options)
        {
            elements.Add(new CanvasText(text, rect, color, options));
        }
        public bool DrawButton(string text, Rect rect, Color32 textColor, Color32 buttonColor, Color32 buttonHoverColor, Color32 buttonDownColor, TextOptions options)
        {
            bool hold = Input.GetKey(KeyCode.MouseL);
            bool hover = rect.Contains(Input.MousePosition);
            Color32 color = hover ? (hold ? buttonDownColor : buttonHoverColor) : buttonColor;

            elements.Add(new CanvasButton(text, rect, textColor, color, options));
            return Input.GetKeyDown(KeyCode.MouseL) && hover;
        }
        public bool DrawHandle(int id, Vector2 position, float radius, Color32 color, Color32 hoverColor, Color32 holdColor)
        {
            bool hold = Input.GetKey(KeyCode.MouseL);
            bool hover = (Input.MousePosition - position).length <= radius;
            color = hover ? (hold ? holdColor : hoverColor) : color;

            elements.Add(new CanvasCircle(position, radius, color));
            
            if (Input.GetKeyDown(KeyCode.MouseL) && hover) handles.Add(id);
            if (Input.GetKeyUp(KeyCode.MouseL)) handles.Remove(id);
            return handles.Contains(id);
        }
        public void DrawLine(Vector2 a, Vector2 b, Color32 color, int width)
        {
            elements.Add(new CanvasLine(a, b, color, width));
        }
    }
}