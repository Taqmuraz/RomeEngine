using RomeEngine;
using RomeEngine.UI;

namespace RomeEngineEditor
{
    public interface ITransformHandle
    {
        void DrawHandle(ITransform transform, Canvas canvas);
    }
    public abstract class TransformHandle : ITransformHandle
    {
        public abstract void DrawHandle(ITransform transform, Canvas canvas);

        protected void DrawLine(Canvas canvas, Vector3 a, Vector3 b, Color32 color)
        {
            Matrix4x4 l2s = Camera.ActiveCamera.WorldToScreenMatrix;
            canvas.DrawLine((Vector2)l2s.MultiplyPoint_With_WDivision(a), (Vector2)l2s.MultiplyPoint_With_WDivision(b), color, 10);
        }
    }
    public sealed class TransformPositionHandle : TransformHandle
    {
        public override void DrawHandle(ITransform transform, Canvas canvas)
        {
            DrawLine(canvas, transform.Position, transform.Position + transform.Right, Color32.red);
            DrawLine(canvas, transform.Position, transform.Position + transform.Up, Color32.green);
            DrawLine(canvas, transform.Position, transform.Position + transform.Forward, Color32.blue);
        }
    }
}