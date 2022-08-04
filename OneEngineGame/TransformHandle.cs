using OneEngine;
using OneEngine.UI;

namespace OneEngineGame
{
    public abstract class TransformHandle : ITransformHandle
    {
        protected abstract Color32 Color { get; }
        protected abstract float Radius { get; }
        protected abstract Vector2 HandleLocalPosition { get; }
        protected abstract Vector2 TextLocalPosition { get; }
        protected virtual Vector2 TextScreenOffset => new Vector2(0f, 25f);

        protected bool IsAccurateMode { get; private set; }

        bool ITransformHandle.Draw(Transform transform, Canvas canvas, Camera camera, bool accurateMode, bool drawOnly)
        {
            IsAccurateMode = accurateMode;
            var screenToWorld = camera.ScreenToWorldMatrix;
            var worldToScreen = camera.WorldToScreenMatrix;

            var l2w = transform.LocalToWorld;
            var mouseWorld = screenToWorld.MultiplyPoint((Input.MousePosition));

            var handleWorld = l2w.MultiplyPoint(HandleLocalPosition);
            var handleScreen = worldToScreen.MultiplyPoint(handleWorld);

            float radius = Radius;

            canvas.DrawLine(worldToScreen.MultiplyPoint((Vector2)l2w.Column_2), worldToScreen.MultiplyPoint(l2w.MultiplyPoint(Vector2.right)), Color32.blue, 1);
            canvas.DrawText(transform.Name, Rect.FromCenterAndSize(worldToScreen.MultiplyPoint(l2w.MultiplyPoint(TextLocalPosition)) + TextScreenOffset, new Vector2(100f, 25f)), IsAccurateMode ? Color32.blue : Color32.red, new TextOptions() { FontSize = 12f });

            if (!drawOnly && canvas.DrawHandle(transform.GetHashCode() + GetHashCode(), handleScreen, radius, Color, Color32.white, Color32.gray))
            {
                OnDragHandle(transform, mouseWorld);
                return true;
            }
            return false;
        }
        protected abstract void OnDragHandle(Transform transform, Vector2 worldMousePosition);
    }
}