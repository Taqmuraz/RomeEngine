using OneEngine;

namespace OneEngineGame
{
    public class TransformRotationHandle : TransformHandle
    {
        protected override Color32 Color => Color32.red;

        protected override float Radius => 10f;

        protected override string Text => "Rotation";

        protected override Vector2 HandleLocalPosition => new Vector2(0.5f, 0f);

        protected override Vector2 TextLocalPosition => new Vector2(0.5f, 0f);

        protected override void OnDragHandle(Transform transform, Vector2 worldMousePosition)
        {
            var worldDelta = worldMousePosition - transform.Position;
            var localDelta = transform.ParentToWorld.GetInversed().MultiplyVector(worldDelta);
            var rotation = localDelta.ToAngle();
            if (IsAccurateMode) rotation = (int)(rotation * 0.1f) * 10f;
            transform.LocalRotation = rotation;
        }
    }
}