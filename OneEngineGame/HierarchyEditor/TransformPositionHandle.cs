using OneEngine;

namespace OneEngineGame
{
    public sealed class TransformPositionHandle : TransformHandle
    {
        protected override Color32 Color => Color32.blue;

        protected override float Radius => 10f;

        protected override string Text => "Position";

        protected override Vector2 HandleLocalPosition => Vector2.zero;

        protected override Vector2 TextLocalPosition => Vector2.zero;

        protected override Vector2 TextScreenOffset => new Vector2(0f, 25f);

        protected override void OnDragHandle(Transform transform, Vector2 worldMousePosition)
        {
            var position = transform.ParentToWorld.GetInversed().MultiplyPoint(worldMousePosition);
            if (IsAccurateMode)
            {
                position = position * 10f;
                position = new Vector2((int)position.x, (int)position.y) * 0.1f;
            }
            transform.LocalPosition = position;
        }
    }
}