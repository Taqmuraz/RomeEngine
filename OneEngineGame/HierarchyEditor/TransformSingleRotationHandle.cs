using OneEngine;
using OneEngine.UI;
using System.Collections.Generic;

namespace OneEngineGame
{
    public sealed class TransformSingleRotationHandle : ITransformHandle
    {
        Dictionary<Transform, float> transforms = new Dictionary<Transform, float>();

        public bool Draw(Transform transform, Canvas canvas, Camera camera, bool accurateMode)
        {
            var handlables = transform.GameObject.GetComponentsOfType<IHandlable>();

            if (handlables.Length == 0) return false;

            var lineTransforms = new List<Matrix3x3>();

            foreach (var handlable in handlables)
            {
                var lines = handlable.GetHandleLines();
                foreach (var line in lines)
                {
                    Vector2 right = line.b - line.a;
                    lineTransforms.Add(Matrix3x3.WorldTransform(right, Vector2.Cross(right), line.a));
                }
            }

            Vector2 mouseWorld = camera.ScreenToWorldMatrix.MultiplyPoint(Input.MousePosition);
            Matrix3x3 w2s = camera.WorldToScreenMatrix;

            bool mouseOnLine = false;
            bool checkMouse = Input.GetKeyDown(KeyCode.MouseL);

            if (Input.GetKeyUp(KeyCode.MouseL))
            {
                transforms.Remove(transform);
                checkMouse = false;
            }

            float lineWidth = 0.2f;

            foreach (var line in lineTransforms)
            {
                Vector2 aWorld, bWorld;
                Vector2 a = w2s.MultiplyPoint(aWorld = (Vector2)line.Column_2);
                Vector2 b = w2s.MultiplyPoint(bWorld = line.MultiplyPoint(Vector2.right));
                if (!mouseOnLine)
                {
                    Vector2 mouseInLineSpace = line.GetInversed().MultiplyPoint(mouseWorld);
                    if (new Rect(0f, -lineWidth * 0.5f, 1f, lineWidth).Contains(mouseInLineSpace))
                    {
                        mouseOnLine = true;
                        if (checkMouse && transforms.Count == 0) transforms[transform] = transform.LocalRotation - transform.ParentToWorld.GetInversed().MultiplyDirection((mouseWorld - aWorld).normalized).ToAngle();
                    }
                }
                canvas.DrawLine(a, b, mouseOnLine ? Color32.red : Color32.gray, 3);
            }

            bool rotating = transforms.TryGetValue(transform, out float value);
            if (rotating)
            {
                transform.LocalRotation = (transform.ParentToWorld.GetInversed().MultiplyPoint(mouseWorld) - transform.LocalPosition).ToAngle() + value;
            }

            return rotating;
        }
    }
}