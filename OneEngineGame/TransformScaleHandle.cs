using OneEngine;

namespace OneEngineGame
{
    public sealed class TransformScaleHandle : TransformHandle
    {
        protected override Color32 Color => Color32.green;

        protected override float Radius => 10f;

        protected override Vector2 HandleLocalPosition => Vector2.right;

        protected override Vector2 TextLocalPosition => new Vector2(0.5f, 0f);

        protected override void OnDragHandle(Transform transform, Vector2 worldMousePosition)
        {
            float scale = transform.ParentToWorld.GetInversed().MultiplyPoint(worldMousePosition).length;
            if (IsAccurateMode)
            {
                scale = (int)(scale * 10f) * 0.1f;
            }
            transform.LocalScale = Vector2.one * scale;
        }
    }
}