using RomeEngine.UI;
using System.Linq;

namespace RomeEngine
{
    public sealed class LineRenderer : Renderer2D, IHandlable
    {
        [SerializeField] public bool SmoothEnding { get; set; }

        [SerializeField] Line[] lines;
        public void SetLines(params Line[] lines)
        {
            this.lines = lines;
        }
        protected override void OnGraphicsUpdate(IGraphics2D graphics, Camera2D camera)
        {
            if (lines == null) return;
            foreach (var line in lines)
            {
                graphics.Brush = new SingleColorBrush(line.Color);
                graphics.DrawLine(line.PointA, line.PointB, line.WidthA, line.WidthB, SmoothEnding);
            }
        }

        protected override bool IsInsideScreen(IGraphics2D graphics, Camera2D camera)
        {
            if (lines != null && lines.Length != 0)
            {
                var cameraVolume = camera.Volume;
                var l2w = Transform.LocalToWorld;
                var a = l2w.MultiplyPoint(lines[0].PointA);
                var b = l2w.MultiplyPoint(lines[0].PointB);
                Rect lineBounds = new Rect(a - Vector2.one * lines[0].WidthA * 0.5f, b + Vector2.one * lines[0].WidthB * 0.5f);
                lineBounds.Spread(lines[0].PointB);
                for (int i = 1; i < lines.Length; i++)
                {
                    lineBounds.Spread(l2w.MultiplyPoint(lines[i].PointA - Vector2.one * lines[i].WidthA * 0.5f));
                    lineBounds.Spread(l2w.MultiplyPoint(lines[i].PointB + Vector2.one * lines[i].WidthB * 0.5f));
                }
                if (cameraVolume.IntersectsWith(lineBounds)) return true;
            }
            return false;
        }

        public (Vector2, Vector2)[] GetHandleLines()
        {
            var l2w = Transform.LocalToWorld;
            return lines == null ? new (Vector2, Vector2)[0] : lines.Select(l => (l2w.MultiplyPoint(l.PointA), l2w.MultiplyPoint(l.PointB))).ToArray();
        }
    }
}