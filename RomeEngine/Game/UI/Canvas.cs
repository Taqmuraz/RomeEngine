using System;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.UI
{
    public class Canvas : Renderer
    {
        List<ICanvasElement> elements = new List<ICanvasElement>();
        HashSet<int> handles = new HashSet<int>();
        List<Rect> usedSpace = new List<Rect>();

        [BehaviourEvent]
        void Start()
        {
            Queue = -1000;
        }

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
            foreach (var element in elements)
            {
                graphics.Style = graphics.OutlineStyle;
                element.Draw(new OutlineRendererPass.BlackBrushGraphics(graphics, 2), camera);
                graphics.Style = graphics.FillStyle;
                element.Draw(graphics, camera);
            }
        }

        [BehaviourEvent]
        void OnPostRender()
        {
            elements.Clear();
            usedSpace.Clear();
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

            bool result = Input.GetKeyDown(KeyCode.MouseL) && hover && !usedSpace.Any(r => r.Contains(Input.MousePosition));
            usedSpace.Add(rect);
            return result;
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
        public float DrawScrollbar(int id, float minValue, float maxValue, float value, Rect rect, int axis, Color32 handleColor)
        {
            DrawRect(rect, Color32.white * 0.75f);
            Vector2 axisDir = new Vector2() { [axis] = 1f };
            int otherAxis = (axis + 1) % 2;
            float width = rect.Size[otherAxis];
            float length = rect.Size[axis];

            float valuableLength = length - width;
            Vector2 start = rect.min + new Vector2(width, width) * 0.5f;
            Vector2 handlePos = start + (axisDir * (value - minValue) / (maxValue - minValue)) * valuableLength;
            Rect handleRect = Rect.FromCenterAndSize(handlePos, new Vector2(width, width));
            DrawRect(handleRect, handleColor);

            if (Input.GetKeyDown(KeyCode.MouseL) && rect.Contains(Input.MousePosition)) handles.Add(id);
            if (Input.GetKeyUp(KeyCode.MouseL)) handles.Remove(id);

            if (handles.Contains(id))
            {
                value = ((Vector2.Dot(Input.MousePosition - start, axisDir) / valuableLength) * (maxValue - minValue) + minValue).Clamp(minValue, maxValue);
            }

            return value;
        }
    }
}