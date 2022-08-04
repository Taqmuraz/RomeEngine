using OneEngine;

namespace OneEngineGame
{
    public sealed class TransformScaleHandle : TransformHandle
    {
        public int Axis { get; set; } = 0;

        protected override Color32 Color => Color32.green;

        protected override float Radius => 10f;

        protected override Vector2 HandleLocalPosition => new Vector2() { [Axis] = 1f };

        protected override Vector2 TextLocalPosition => new Vector2(0.5f, 0f);

        protected override void OnDragHandle(Transform transform, Vector2 worldMousePosition)
        {
            float scale = Vector2.Dot((transform.ParentToWorld.GetInversed().MultiplyPoint(worldMousePosition) - transform.LocalPosition), transform.LocalMatrix.MultiplyVector(new Vector2() { [Axis] = 1f }).normalized);
            if (IsAccurateMode)
            {
                scale = ((int)(scale * 10f) * 0.1f);
            }
            scale = scale.Clamp(0.1f, scale);
            Vector2 currentScale = transform.LocalScale;
            currentScale[Axis] = scale;
            transform.LocalScale = currentScale;
        }
    }
}