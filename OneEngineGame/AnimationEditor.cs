using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OneEngine;
using OneEngine.IO;
using OneEngine.UI;

namespace OneEngineGame
{
    public sealed class AnimationEditor : Component
    {
        IEnumerator routine;

        IEnumerator Routine()
        {
            var camera = Camera.Cameras[0];
            Canvas canvas = GameObject.AddComponent<Canvas>();
            FileSearchMenu fileSearch = new FileSearchMenu("./", "Select gameObject file");
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

                foreach (var bone in skeleton)
                {
                    var l2w = bone.LocalToWorld;
                    var p2w = bone.ParentToWorld;
                    var w2p = p2w.GetInversed();

                    var handlePos = worldToScreen.MultiplyPoint(l2w.MultiplyPoint(Vector2.right * 0.25f));

                    canvas.DrawLine(worldToScreen.MultiplyPoint((Vector2)l2w.Column_2), handlePos, Color32.red, 5);
                    if (canvas.DrawHandle(handlePos, 15f, Color32.red, Color32.green, Color32.gray))
                    {
                        var mouseWorld = screenToWorld.MultiplyPoint(Input.MousePosition);
                        var worldDelta = mouseWorld - bone.Position;
                        var localDelta = w2p.MultiplyVector(worldDelta);
                        bone.LocalRotation = localDelta.ToAngle();
                    }

                    handlePos = worldToScreen.MultiplyPoint((Vector2)l2w.Column_2);

                    if (canvas.DrawHandle(handlePos, 15f, Color32.blue, Color32.green, Color32.gray))
                    {
                        var mouseWorld = screenToWorld.MultiplyPoint((Input.MousePosition));
                        var mouseLocal = w2p.MultiplyPoint(mouseWorld);
                        
                        bone.LocalPosition = mouseLocal;
                    }
                }
            }
        }

        [BehaviourEvent]
        void Start()
        {
            routine = Routine();
        }
        [BehaviourEvent]
        void Update()
        {
            if (routine != null && !routine.MoveNext()) routine = null;
        }
    }
}