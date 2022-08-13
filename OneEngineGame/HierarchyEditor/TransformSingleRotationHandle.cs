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
            Matrix3x3[] lineTransforms;

            if (transform.Children.Count == 0)
            {
                lineTransforms = new Matrix3x3[] { transform.LocalToWorld };
            }
            else
            {
                lineTransforms = new Matrix3x3[transform.Children.Count];
                Matrix3x3 l2w = transform.LocalToWorld;
                Vector2 pos = (Vector2)l2w.Column_2;
                for (int i = 0; i < lineTransforms.Length; i++)
                {
                    var child = transform.Children[i];
                    Vector2 right = l2w.MultiplyVector(child.LocalPosition);
                    lineTransforms[i] = Matrix3x3.WorldTransform(right, Vector2.Cross(right), pos);
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

            float lineWidth = 0.1f;

            foreach (var line in lineTransforms)
            {
                Vector2 aWorld, bWorld;
                Vector2 a = w2s.MultiplyPoint(aWorld = (Vector2)line.Column_2);
                Vector2 b = w2s.MultiplyPoint(bWorld = line.MultiplyPoint(Vector2.right));
                if (checkMouse && !mouseOnLine)
                {
                    Vector2 mouseInLineSpace = line.GetInversed().MultiplyPoint(mouseWorld);
                    if (new Rect(-0.5f, -lineWidth * 0.5f, 1f, lineWidth).Contains(mouseInLineSpace))
                    {
                        mouseOnLine = true;
                        transforms[transform] = transform.ParentToWorld.GetInversed().MultiplyDirection((bWorld - aWorld).normalized).ToAngle();
                    }
                }
                canvas.DrawLine(a, b, mouseOnLine ? Color32.red : Color32.gray, 3);
            }

            if (transforms.TryGetValue(transform, out float value))
            {
                transform.LocalRotation = (transform.ParentToWorld.GetInversed().MultiplyPoint(mouseWorld) - transform.LocalPosition).ToAngle() - value;
            }

            return true;
        }
    }
}