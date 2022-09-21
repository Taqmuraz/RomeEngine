using RomeEngine;
using RomeEngine.IO;
using RomeEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomeEngineGame
{
    public sealed class LevelSelectMenu : Component
    {
        EditorCanvas canvas;

        [BehaviourEvent]
        void Start()
        {
            Vector2 menuSize = new Vector2(300f, 25f);
            var screenSize = Screen.Size;
            canvas = GameObject.AddComponent<EditorCanvas>();
            EditorMenu.ShowMenu<DropdownMenu>(canvas, menu => GameScenes.gameScenes[menu.SelectedOption].LoadScene(),
                new Rect(screenSize.x * 0.5f - menuSize.x * 0.5f, screenSize.y * 0.5f - menuSize.y * 0.5f, menuSize.x, menuSize.y))
                .DropdownOptions = GameScenes.gameScenes.Select(s => s.Name).ToArray();
        }
    }
    public static class RomeEngineGame
    {
        public static IEngine StartGame(IEngineRuntine runtime)
        {
            var engine = new Engine();
            GameScenes.InitializeGameScenes(LevelSelectScene(), new GameScene[] { AnimationEditorScene(), GamePlayScene() });
            engine.Initialize(runtime);
            return engine;
        }
        static GameScene LevelSelectScene()
        {
            var camera = new GameObjectInstancer(() => new GameObject("Camera").AddComponent<Camera2D>().GameObject);
            var scene = new GameScene("Level select scene");
            scene.AddGameObjectInstancer(camera);
            scene.AddGameObjectInstancer(new GameObjectInstancer(() => new GameObject("Level select menu").AddComponent<LevelSelectMenu>().GameObject));
            return scene;
        }
        static GameScene AnimationEditorScene()
        {
            var camera = new GameObjectInstancer(() => new GameObject("Camera").AddComponent<Camera2D>().GameObject);
            var scene = new GameScene("Editor scene");
            scene.AddGameObjectInstancer(camera);
            scene.AddGameObjectInstancer(new GameObjectInstancer(() =>
            {
                var modelEditor = new GameObject("ModelEditor").AddComponent<HierarchyEditor>();
                return modelEditor.GameObject;
            }));
            return scene;
        }
        static GameScene GamePlayScene()
        {
            var camera = new GameObjectInstancer(() => new GameObject("Camera").AddComponent<Camera2D>().GameObject);
            var scene = new GameScene("Game scene");
            scene.AddGameObjectInstancer(camera);
            scene.AddGameObjectInstancer(new GameObjectInstancer(() =>
            {
                var player = Resources.Load<GameObject>("Models/HumanWithSwordIdle.bin");
                player.AddComponent<PlayerController>();
                return player;
            }));
            scene.AddGameObjectInstancer(() =>
            {
                var canvas = new GameObject("Loop test").AddComponent<EditorCanvas>();
                canvas.TextColor = Color32.white;
                Routine.StartRoutine(new ActionRoutine(() => canvas.DrawText(((int)Mathf.Loop(Time.CurrentTime, 10, 20)).ToString(), new Rect(25, 25, 200, 50), TextOptions.Default)));
                return canvas.GameObject;
            });
            scene.AddGameObjectInstancer(() =>
            {
                var box = new GameObject("Box").AddComponent<BoxRenderer>();
                box.Scale = new Vector2(10f, 1f);
                box.Transform.Position = new Vector2(0f, -0.5f);
                box.Color = Color32.gray;
                return box.GameObject;
            });
            return scene;
        }
    }
}
