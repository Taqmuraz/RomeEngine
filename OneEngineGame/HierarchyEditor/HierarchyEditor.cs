using System.Collections;
using System.IO;
using System.Linq;
using OneEngine;
using OneEngine.IO;
using OneEngine.UI;

namespace OneEngineGame
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
                    int posX = 0;
                    int posY = 1;
                    if (Input.GetKeyDown(KeyCode.ShiftKey)) accurateMode = !accurateMode;
                    editorMode.IsAccurate = accurateMode;

                    IEnumerator DrawTransform(Transform transform)
                    {
                        editorMode.DrawHandles(transform, inspectedGameObject?.Transform, camera, sceneCanvas);

                        string name = transform.Name;
                        if (transform.Parent != null) name = $"{transform.Parent.Children.IndexOf(transform) + 1}){name}";
                        if (canvas.DrawButton(transform.Name, new Rect(posX * indent, posY * elementHeight, elementWidth - posX * indent - elementHeight * 2f, elementHeight), TextOptions.Default))
                        {
                            inspectorMenu.Inspect(inspectedGameObject = transform.GameObject);
                        }

                        if (canvas.DrawButton("-", new Rect(elementWidth - elementHeight * 2f, elementHeight * posY, elementHeight, elementHeight), TextOptions.Default))
                        {
                            transform.GameObject.Destroy();
                        }
                        if (canvas.DrawButton("+", new Rect(elementWidth - elementHeight, elementHeight * posY, elementHeight, elementHeight), TextOptions.Default))
                        {
                            EditorMenu.ShowMenu<StringInputMenu>(canvas, menu =>
                            {
                                var newChild = new GameObject(menu.InputString).Transform;
                                newChild.Parent = transform;
                                newChild.LocalPosition = Vector2.right;
                            }).WithHeader("New child name");
                        }
                        posX++;

                        foreach (var child in transform.Children.ToArray())
                        {
                            posY++;
                            var drawIterator = DrawTransform(child);
                            while (drawIterator.MoveNext()) { yield return null; }
                        }
                        posX--;
                    }
                    var iterator = DrawTransform(root);
                    while (iterator.MoveNext()) { yield return null; }
                }
            }
        }
    }
}