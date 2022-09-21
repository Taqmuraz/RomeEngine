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
            Transform root = null;
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

                if (root == null || !root.GameObject.IsActive)
                {
                    if (canvas.DrawButton("Create root", new Rect(0f, 0f, elementWidth, elementHeight), TextOptions.Default))
                    {
                        EditorMenu.ShowMenu<StringInputMenu>(canvas, menu => root = new GameObject(menu.InputString).Transform).WithHeader("New transform name");
                    }
                    if (canvas.DrawButton("Import hierarchy", new Rect(0f, elementHeight, elementWidth, elementHeight), TextOptions.Default))
                    {
                        Engine.Instance.Runtime.ShowFileOpenDialog("./", "Select GameObject file", file => root = ((GameObject)new Serializer().DeserializeFile(file)).Transform);
                    }
                }
                else
                {
                    if (canvas.DrawButton("Export hierarchy", new Rect(0f, 0f, elementWidth, elementHeight), TextOptions.Default))
                    {
                        Engine.Instance.Runtime.ShowFileWriteDialog("./", root.Name + Serializer.BinaryFormatExtension, "Select export path", file => new Serializer().SerializeFile(root.GameObject, file));
                    }

                    int transformsCount = root.TraceElement(r => r.Children).Count();
                    int scrollMax = transformsCount - (int)((canvasRect.Height - elementHeight) / elementHeight);

                    if (scrollMax > 0)
                    {
                        var scrollRect = new Rect(elementWidth, elementHeight, 30f, canvasRect.Height - elementHeight);
                        scroll = (int)canvas.DrawScrollbar(GetHashCode(), 0, scrollMax, scroll, scrollRect, 1, Color32.gray);
                    }

                    int positionX = 0;
                    int positionY = 1 - scroll;

                    if (Input.GetKeyDown(KeyCode.ShiftKey)) accurateMode = !accurateMode;

                    IEnumerator DrawTransform(Transform transform)
                    {
                        if (positionY > 0)
                        {
                            string name = transform.Name;
                            if (transform.Parent != null) name = $"{transform.Parent.Children.IndexOf(transform) + 1}){name}";
                            if (canvas.DrawButton(transform.Name, new Rect(positionX * indent, positionY * elementHeight, elementWidth - positionX * indent - elementHeight * 2f, elementHeight), TextOptions.Default))
                            {
                                inspectorMenu.Inspect(inspectedGameObject = transform.GameObject);
                            }

                            if (canvas.DrawButton("-", new Rect(elementWidth - elementHeight * 2f, elementHeight * positionY, elementHeight, elementHeight), TextOptions.Default))
                            {
                                transform.GameObject.Destroy();
                            }
                            if (canvas.DrawButton("+", new Rect(elementWidth - elementHeight, elementHeight * positionY, elementHeight, elementHeight), TextOptions.Default))
                            {
                                EditorMenu.ShowMenu<StringInputMenu>(canvas, menu =>
                                {
                                    var newChild = new GameObject(menu.InputString).Transform;
                                    newChild.Parent = transform;
                                    newChild.LocalPosition = Vector2.right;
                                }).WithHeader("New child name");
                            }
                        }
                        positionX++;

                        foreach (var child in transform.Children.ToArray())
                        {
                            positionY++;
                            var drawIterator = DrawTransform(child);
                            while (drawIterator.MoveNext()) { yield return null; }
                        }
                        positionX--;
                    }
                    var iterator = DrawTransform(root);
                    while (iterator.MoveNext()) { yield return null; }
                }
            }
        }
    }
}