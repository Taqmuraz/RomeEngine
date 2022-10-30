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
            IGameObject inspectedGameObject = null;
            bool accurateMode = false;
            int scroll = 0;

            while (true)
            {
                yield return null;

                camera.Transform.Position += camera.Transform.LocalToWorld.MultiplyDirection(Input.GetWASDQE()) * Time.DeltaTime * 5f;
                if (Input.GetKey(KeyCode.MouseR))
                {
                    Vector2 mouseDelta = Input.MouseDelta;
                    camera.Transform.Rotation += new Vector3(mouseDelta.y, mouseDelta.x, 0f) * 15f * Time.DeltaTime;
                }
                if (Input.GetKeyDown(KeyCode.Space)) camera.Transform.Position = camera.Transform.Rotation = Vector3.zero;

                inspectorMenu.Draw(canvas);
                float elementWidth = canvasRect.Size.x * 0.25f;
                float elementHeight = 30f;
                float indent = 30f;
                canvas.DrawRect(new Rect(0f, 0f, elementWidth, Screen.Size.y));

                if (canvas.DrawButton("Create root", new Rect(0f, 0f, elementWidth, elementHeight), TextOptions.Default))
                {
                    EditorMenu.ShowMenu<StringInputMenu>(canvas, menu => new GameObject(menu.InputString).ActivateForActiveScene()).WithHeader("New transform name");
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

                void DrawTransformLine(ITransform transform)
                {
                    Vector3 pos = transform.Position;
                    Vector2 rightPos = (Vector2)worldToScreen.MultiplyPoint_With_WDivision(pos + transform.Right);
                    Vector2 upPos = (Vector2)worldToScreen.MultiplyPoint_With_WDivision(pos + transform.Up);
                    Vector2 forwardPos = (Vector2)worldToScreen.MultiplyPoint_With_WDivision(pos + transform.Forward);
                    Vector2 screenPos = (Vector2)worldToScreen.MultiplyPoint_With_WDivision(pos);
                    
                    if (transform == inspectedGameObject.Transform)
                    {
                        sceneCanvas.DrawLine(screenPos, rightPos, Color32.red, 5);
                        sceneCanvas.DrawLine(screenPos, upPos, Color32.green, 5);
                        sceneCanvas.DrawLine(screenPos, forwardPos, Color32.blue, 5);
                    }
                }

                void DrawGameObject(IGameObject gameObject)
                {
                    if (positionY > 0)
                    {
                        string name = gameObject.Name;

                        if (canvas.DrawButton(name, new Rect(positionX * indent, positionY * elementHeight, elementWidth - positionX * indent - elementHeight, elementHeight), TextOptions.Default))
                        {
                            inspectorMenu.Inspect(inspectedGameObject = gameObject);
                        }

                        if (canvas.DrawButton("-", new Rect(elementWidth - elementHeight, elementHeight * positionY, elementHeight, elementHeight), TextOptions.Default))
                        {
                            gameObject.Deactivate(GameScene.ActiveScene);
                        }
                        positionY++;
                    }
                }
                foreach (var obj in GameScene.ActiveScene.GameObjects)
                {
                    DrawGameObject(obj);
                }
            }
        }
    }
}