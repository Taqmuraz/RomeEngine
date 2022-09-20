using System.Collections;
using System.IO;
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
            var camera = Camera.Cameras[0];
            Canvas sceneCanvas = GameObject.AddComponent<Canvas>();
            EditorCanvas canvas = GameObject.AddComponent<EditorCanvas>();
            var canvasRect = new Rect(Vector2.zero, Screen.Size);
            Transform root = null;
            InspectorMenu inspectorMenu = new InspectorMenu() { Rect = new Rect(canvasRect.Width * 0.75f, 0f, canvasRect.Width * 0.25f, canvasRect.Height) };
            GameObject inspectedGameObject = null;
            bool accurateMode = false;
            int scroll = 0;
            
            IHierarchyEditorMode[] editorModes = 
            {
                new FullModelEditorMode(),
                new RotationOnlyEditorMode(),
            };
            IHierarchyEditorMode editorMode = editorModes[0];

            while (true)
            {
                yield return null;

                camera.Transform.LocalPosition += Input.GetWASD() * Time.DeltaTime * 5f;

                if (Input.GetKeyDown(KeyCode.Q)) camera.OrthographicMultiplier *= 2f;
                if (Input.GetKeyDown(KeyCode.E)) camera.OrthographicMultiplier *= 0.5f;

                inspectorMenu.Draw(canvas);
                float elementWidth = canvasRect.Size.x * 0.25f;
                float elementHeight = 30f;
                float indent = 30f;
                canvas.DrawRect(new Rect(0f, 0f, elementWidth, Screen.Size.y));

                var worldToScreen = camera.WorldToScreenMatrix;

                sceneCanvas.DrawLine(worldToScreen.MultiplyPoint(new Vector2(-10f, 0f)), worldToScreen.MultiplyPoint(new Vector2(10f, 0f)), Color32.red, 1);
                sceneCanvas.DrawLine(worldToScreen.MultiplyPoint(new Vector2(0f, -10f)), worldToScreen.MultiplyPoint(new Vector2(0f, 10f)), Color32.green, 1);

                Color32 gridColor = (Color32.white * 0.8f).WithAlpha(128);

                for (int i = -10; i <= 10; i++)
                {
                    if (i == 0) continue;
                    sceneCanvas.DrawLine(worldToScreen.MultiplyPoint(new Vector2(i, -10f)), worldToScreen.MultiplyPoint(new Vector2(i, 10f)), gridColor, 1);
                    sceneCanvas.DrawLine(worldToScreen.MultiplyPoint(new Vector2(-10f, i)), worldToScreen.MultiplyPoint(new Vector2(10f, i)), gridColor, 1);
                }

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
                    for (int i = 0; i < editorModes.Length; i++)
                    {
                        if (canvas.DrawButton(editorModes[i].Name, new Rect(i * elementWidth * 0.5f + elementWidth, 0f, elementWidth * 0.5f, elementHeight), TextOptions.Default))
                        {
                            editorMode = editorModes[i];
                        }
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
                    editorMode.IsAccurate = accurateMode;

                    IEnumerator DrawTransform(Transform transform)
                    {
                        editorMode.DrawHandles(transform, inspectedGameObject?.Transform, camera, sceneCanvas);

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