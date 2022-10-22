using System.Collections;
using System.Linq;
using RomeEngine;
using RomeEngine.IO;
using RomeEngine.UI;

namespace RomeEngineGame
{
    public sealed class HierarchyEditor : Editor
    {
        protected override IEnumerator Routine()
        {
            var camera = Camera.ActiveCamera;
            Canvas sceneCanvas = GameObject.AddComponent<Canvas>();
            EditorCanvas canvas = GameObject.AddComponent<EditorCanvas>();
            var canvasRect = new Rect(Vector2.zero, Screen.Size);
            InspectorMenu inspectorMenu = new InspectorMenu() { Rect = new Rect(canvasRect.Width * 0.75f, 0f, canvasRect.Width * 0.25f, canvasRect.Height) };
            GameObject inspectedGameObject = null;
            bool accurateMode = false;
            int scroll = 0;

            while (true)
            {
                yield return null;

                camera.Transform.LocalPosition += camera.Transform.LocalToWorld.MultiplyDirection(Input.GetWASDQE()) * Time.DeltaTime * 5f;
                if (Input.GetKey(KeyCode.MouseR))
                {
                    Vector2 mouseDelta = Input.MouseDelta;
                    camera.Transform.LocalRotation += new Vector3(mouseDelta.y, mouseDelta.x, 0f) * 15f * Time.DeltaTime;
                }
                if (Input.GetKeyDown(KeyCode.Space)) camera.Transform.LocalPosition = camera.Transform.LocalRotation = Vector3.zero;

                inspectorMenu.Draw(canvas);
                float elementWidth = canvasRect.Size.x * 0.25f;
                float elementHeight = 30f;
                float indent = 30f;
                canvas.DrawRect(new Rect(0f, 0f, elementWidth, Screen.Size.y));

                if (canvas.DrawButton("Create root", new Rect(0f, 0f, elementWidth, elementHeight), TextOptions.Default))
                {
                    EditorMenu.ShowMenu<StringInputMenu>(canvas, menu => new GameObject(menu.InputString)).WithHeader("New transform name");
                }
                if (canvas.DrawButton("Import hierarchy", new Rect(0f, elementHeight, elementWidth, elementHeight), TextOptions.Default))
                {
                    Engine.Instance.Runtime.ShowFileOpenDialog("./", "Select GameObject file", file => Resources.LoadInstance<GameObject>(file));
                }

                int transformsCount = GameScene.ActiveScene.GameObjects.Count;
                int scrollMax = transformsCount - (int)((canvasRect.Height - elementHeight) / elementHeight);

                if (scrollMax > 0)
                {
                    var scrollRect = new Rect(elementWidth, elementHeight, 30f, canvasRect.Height - elementHeight);
                    scroll = (int)canvas.DrawScrollbar(GetHashCode(), 0, scrollMax, scroll, scrollRect, 1, Color32.gray);
                }

                int positionX = 0;
                int positionY = 3 - scroll;

                if (inspectedGameObject != null)
                {
                    positionY++;
                    if (canvas.DrawButton("Export hierarchy", new Rect(0f, elementHeight * 2f, elementWidth, elementHeight), TextOptions.Default))
                    {
                        Engine.Instance.Runtime.ShowFileWriteDialog("./", inspectedGameObject.Name + Serializer.BinaryFormatExtension, "Select export path", file => new Serializer().SerializeFile(inspectedGameObject, file));
                    }
                }

                if (Input.GetKeyDown(KeyCode.ShiftKey)) accurateMode = !accurateMode;

                Matrix4x4 worldToScreen = Matrix4x4.CreateViewport(Screen.Size.x, Screen.Size.y) * Camera.ActiveCamera.WorldToScreenMatrix;

                if (inspectedGameObject != null)
                {
                    DrawTransformLine(inspectedGameObject.Transform);
                }

                void DrawTransformLine(Transform transform)
                {
                    Vector3 start = transform.Parent == null ? Vector3.zero : transform.Parent.Position;
                    Vector3 end = transform.Position;
                    Vector2 rightEnd = (Vector2)worldToScreen.MultiplyPoint_With_WDivision(end + transform.Right);
                    Vector2 upEnd = (Vector2)worldToScreen.MultiplyPoint_With_WDivision(end + transform.Up);
                    Vector2 forwardEnd = (Vector2)worldToScreen.MultiplyPoint_With_WDivision(end + transform.Forward);
                    Vector2 screenStart = (Vector2)worldToScreen.MultiplyPoint_With_WDivision(start);
                    Vector2 screenEnd = (Vector2)worldToScreen.MultiplyPoint_With_WDivision(end);
                    //sceneCanvas.DrawText(transform.Name, Rect.FromCenterAndSize(screenEnd, new Vector2(100f, 50f)), Color32.white, TextOptions.Default);
                    sceneCanvas.DrawLine(screenStart, screenEnd, Color32.white, 2);

                    if (transform == inspectedGameObject.Transform)
                    {
                        sceneCanvas.DrawLine(screenEnd, rightEnd, Color32.red, 5);
                        sceneCanvas.DrawLine(screenEnd, upEnd, Color32.green, 5);
                        sceneCanvas.DrawLine(screenEnd, forwardEnd, Color32.blue, 5);
                    }

                    foreach (var child in transform.Children) DrawTransformLine(child);
                }

                void DrawTransform(Transform transform)
                {
                    if (positionY > 0)
                    {
                        string name = transform.Name;
                        if (transform.Parent != null) name = $"{transform.Parent.Children.IndexOf(transform) + 1}){name}";
                        if (canvas.DrawButton(transform.Name, new Rect(positionX * indent, positionY * elementHeight, elementWidth - positionX * indent - elementHeight, elementHeight), TextOptions.Default))
                        {
                            inspectorMenu.Inspect(inspectedGameObject = transform.GameObject);
                        }

                        if (canvas.DrawButton("-", new Rect(elementWidth - elementHeight, elementHeight * positionY, elementHeight, elementHeight), TextOptions.Default))
                        {
                            transform.GameObject.Destroy();
                        }
                        positionY++;
                    }
                }
                foreach (var obj in GameScene.ActiveScene.GameObjects.SelectMany(g => g.TraceElement(l => l.Transform.Children.Select(c => c.GameObject)))
                    .Where(g => g.GetComponents().Any(c => !(c is Transform))))
                {
                    DrawTransform(obj.Transform);
                }
            }
        }
    }
}