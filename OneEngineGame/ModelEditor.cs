using System.Collections;
using System.Linq;
using OneEngine;
using OneEngine.UI;

namespace OneEngineGame
{
    public sealed class ModelEditor : Editor
    {
        [BehaviourEvent]
        void Update()
        {

        }

        protected override IEnumerator Routine()
        {
            var camera = Camera.Cameras[0];
            Canvas sceneCanvas = GameObject.AddComponent<Canvas>();
            EditorCanvas canvas = GameObject.AddComponent<EditorCanvas>();
            var canvasRect = new Rect(Vector2.zero, Screen.Size);
            Transform root = null;
            InspectorMenu inspectorMenu = new InspectorMenu() { Rect = new Rect(canvasRect.Width * 0.75f, 0f, canvasRect.Width * 0.25f, canvasRect.Height) };
            Transform inspectedTransform = null;
            bool accurateMode = false;
            ITransformHandle[] transformHandles =
            {
                new TransformRotationHandle(),
                new TransformPositionHandle(),
                new TransformScaleHandle() { Axis = 1 },
                new TransformScaleHandle() { Axis = 2 },
                new TransformScaleHandle() { Axis = 3 },
            };

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
                        var inputMenu = EditorMenu.ShowMenu<StringInputMenu>(canvas, menu => root = new GameObject(menu.InputString).Transform);
                        inputMenu.Header = "New transform name";
                    }
                }
                else
                {
                    int posX = 0;
                    int posY = 0;
                    if (Input.GetKeyDown(KeyCode.ShiftKey)) accurateMode = !accurateMode;
                    
                    IEnumerator DrawTransform(Transform transform)
                    {
                        var worldToScreen = camera.WorldToScreenMatrix;
                        var l2w = transform.LocalToWorld;
                        Vector2 textLocalPosition = new Vector2(0.5f, 0f);
                        Vector2 textScreenOffset = new Vector2(0f, 25f);

                        sceneCanvas.DrawLine(worldToScreen.MultiplyPoint((Vector2)l2w.Column_2), worldToScreen.MultiplyPoint(l2w.MultiplyPoint(Vector2.right)), transform == inspectedTransform ? Color32.white : Color32.blue, 1);
                        
                        if (transform == inspectedTransform) foreach (var handle in transformHandles) handle.Draw(transform, sceneCanvas, camera, accurateMode);

                        string name = transform.Name;
                        if (transform.Parent != null) name = $"{transform.Parent.Children.IndexOf(transform) + 1}){name}";
                        if (canvas.DrawButton(transform.Name, new Rect(posX * indent, posY * elementHeight, elementWidth - posX * indent - elementHeight * 2f, elementHeight), TextOptions.Default))
                        {
                            inspectorMenu.Inspect(inspectedTransform = transform);
                        }

                        if (canvas.DrawButton("-", new Rect(elementWidth - elementHeight * 2f, elementHeight * posY, elementHeight, elementHeight), TextOptions.Default))
                        {
                            transform.GameObject.Destroy();
                        }
                        if (canvas.DrawButton("+", new Rect(elementWidth - elementHeight, elementHeight * posY, elementHeight, elementHeight), TextOptions.Default))
                        {
                            var inputMenu = EditorMenu.ShowMenu<StringInputMenu>(canvas, menu =>
                            {
                                var newChild = new GameObject(menu.InputString).Transform;
                                newChild.Parent = transform;
                                newChild.LocalPosition = Vector2.right;
                            });
                            inputMenu.Header = "New child name";
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