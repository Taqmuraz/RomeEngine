using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OneEngine;
using OneEngine.IO;
using OneEngine.UI;

namespace OneEngineGame
{
    public sealed class AnimationEditor : Editor
    {
        protected override IEnumerator Routine()
        {
            yield return null;
            /*var camera = Camera.Cameras[0];
            EditorCanvas canvas = GameObject.AddComponent<EditorCanvas>();
            while (string.IsNullOrEmpty(fileSearch.File))
            {
                fileSearch.Draw(canvas);
                yield return null;
            }
            IEnumerable<Transform> skeleton;
            using (FileStream file = File.OpenRead(fileSearch.File))
            {
                GameObject skeletonRoot = (GameObject)new Serializer().Deserialize(new BinarySerializationStream(file));
                skeleton = skeletonRoot.Transform.TraceElement(c => c.Children);
            }
            while (true)
            {
                yield return null;

                var worldToScreen = camera.WorldToScreenMatrix;
                var screenToWorld = camera.ScreenToWorldMatrix;
                int hash = 0;

                foreach (var bone in skeleton)
                {
                    var l2w = bone.LocalToWorld;
                    var p2w = bone.ParentToWorld;
                    var w2p = p2w.GetInversed();
                    var mouseWorld = screenToWorld.MultiplyPoint((Input.MousePosition));

                    var handleWorld = l2w.MultiplyPoint(Vector2.right * 0.25f);
                    var handleScreen = worldToScreen.MultiplyPoint(handleWorld);

                    float radius = (handleWorld - mouseWorld).length < 0.1f ? 15f : 5f;

                    canvas.DrawLine(worldToScreen.MultiplyPoint((Vector2)l2w.Column_2), handleScreen, Color32.red, 5);
                    if (canvas.DrawHandle(hash++, handleScreen, radius, Color32.red, Color32.green, Color32.gray))
                    {
                        var worldDelta = mouseWorld - bone.Position;
                        var localDelta = w2p.MultiplyVector(worldDelta);
                        bone.LocalRotation = localDelta.ToAngle();
                    }

                    /*handlePos = worldToScreen.MultiplyPoint((Vector2)l2w.Column_2);

                    if (canvas.DrawHandle(hash++, handlePos, 15f, Color32.blue, Color32.green, Color32.gray))
                    {
                        var mouseLocal = w2p.MultiplyPoint(mouseWorld);
                        
                        bone.LocalPosition = mouseLocal;
                    }
                }
            }*/
        }
    }
}