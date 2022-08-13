using OneEngine;

namespace OneEngineGame
{
    public sealed class TransformScaleHandle : TransformHandle
    {
        public int Axis { get; set; } = 0;

        protected override Color32 Color => Color32.green;

        protected override float Radius => 10f;

        protected override Vector2 HandleLocalPosition => LocalAxis;

        protected override Vector2 TextLocalPosition => LocalAxis;

        protected override string Text
        {
            get
            {
                switch (Axis)
                {
                    case 1: return "Size X";
                    case 2: return "Size Y";
                    case 3: return "Size XY";
                    default: return "Wrong axis";
                }
            }
        }

        Vector2 LocalAxis
        {
            get
            {
                Vector2 axis = new Vector2();
                if ((Axis & 1) != 0) axis.x = 1f;
                if ((Axis & 2) != 0) axis.y = 1f;
                return axis;
            }
        }

        protected override void OnDragHandle(Transform transform, Vector2 worldMousePosition)
        {
            if (Axis == 0) return;

            Vector2 currentScale = transform.LocalScale;

            Vector2 mouseLocalOffset = transform.ParentToWorld.GetInversed().MultiplyPoint(worldMousePosition) - transform.LocalPosition;

            float scaleX = (Axis & 1) == 0 ? currentScale.x : Vector2.Dot(mouseLocalOffset, transform.LocalRight);
            float scaleY = (Axis & 2) == 0 ? currentScale.y : Vector2.Dot(mouseLocalOffset, transform.LocalUp);

            if (IsAccurateMode)
            {
                scaleX = ((int)(scaleX * 10f) * 0.1f);
                scaleY = ((int)(scaleY * 10f) * 0.1f);
            }
            scaleX = scaleX.Clamp(0.1f, scaleX);
            scaleY = scaleY.Clamp(0.1f, scaleY);

            transform.LocalScale = new Vector2(scaleX, scaleY);
        }
    }
}