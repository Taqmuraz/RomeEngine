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
            EditorMenu.ShowMenu<DropdownMenu>(canvas, menu => GameScenes.GameScenesList[menu.SelectedOption + 1].LoadScene(),
                new Rect(screenSize.x * 0.5f - menuSize.x * 0.5f, screenSize.y * 0.5f - menuSize.y * 0.5f, menuSize.x, menuSize.y))
                .DropdownOptions = GameScenes.GameScenesList.Skip(1).Select(s => s.Name).ToArray();
        }
    }
    public static class RomeEngineGame
    {
        public static IEngine StartGame(IEngineRuntine runtime)
        {
            var engine = new Engine();
            GameScenes.InitializeGameScenes(new GameScene[] { LevelSelectScene(), AnimationEditorScene(), GamePlayScene() });
            engine.Initialize(runtime);
            return engine;
        }
        static GameScene LevelSelectScene()
        {
            var camera = new GameObjectInstancer(() => new GameObject("Camera").AddComponent<Camera>().GameObject);
            var scene = new GameScene("Level select scene");
            scene.AddGameObjectInstancer(camera);
            scene.AddGameObjectInstancer(new GameObjectInstancer(() => new GameObject("Level select menu").AddComponent<LevelSelectMenu>().GameObject));
            return scene;
        }
        static GameScene AnimationEditorScene()
        {
            var camera = new GameObjectInstancer(() => new GameObject("Camera").AddComponent<Camera>().GameObject);
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
            var camera = new GameObjectInstancer(() =>
            {
                var instance = new GameObject("Camera").AddComponent<Camera>();
                instance.ClearColor = new Color32(0.3f, 0.5f, 0.6f, 1f);
                return instance.GameObject;
            });
            var light = new GameObjectInstancer(() =>
            {
                var lightObject = new GameObject("Light").AddComponent<GlobalLight>();

                lightObject.Transform.LocalRotation = new Vector3(75f, 135f, 0f);
                lightObject.Setup(0.1f, 0.3f, Color32.white);

                return lightObject.GameObject;
            });

            var scene = new GameScene("Game scene");
            scene.AddGameObjectInstancer(camera);
            scene.AddGameObjectInstancer(light);

            scene.AddGameObjectInstancer(new GameObjectInstancer(() => new GameObject("Terrain").AddComponent<TerrainRenderer>().GameObject));

            scene.AddGameObjectInstancer(new GameObjectInstancer(() =>
            {
                var player = Resources.LoadInstance<GameObject>("Models/KnightFemale.bin");
                player.Name = "Player";
                player.AddComponent<PlayerController>();
                //var collider = player.AddComponent<SphereCollider>();
                //collider.PhysicalBody = new SimpleDynamicBody(player.Transform);
                player.Transform.Position = new Vector3(0f, 0f, 0f);
                return player;
            }));

            for (int i = 0; i < 10; i++)
            {
                int index = i;
                scene.AddGameObjectInstancer(new GameObjectInstancer(() =>
                {
                    var sphere = Resources.LoadInstance<GameObject>("Models/Knight.bin");
                    sphere.Name = "Sphere";
                    sphere.AddComponent<HumanAnimator>().PlayAnimation("Sword_Idle");
                    float angle = 36f * index;
                    sphere.Transform.Position = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * 3f;
                    sphere.Transform.Rotation = new Vector3(0f, -angle + 90f, 0f);

                    //sphere.AddComponent<SphereCollider>();
                    return sphere;
                }));
            }

            scene.AddGameObjectInstancer(() =>
            {
                var canvas = new GameObject("Loop test").AddComponent<EditorCanvas>();
                canvas.TextColor = Color32.white;
                Routine.StartRoutine(new ActionRoutine(() => canvas.DrawText((1f / Time.DeltaTime).ToString("F1"), new Rect(25, 25, 200, 50), TextOptions.Default)));
                return canvas.GameObject;
            });
            return scene;
        }
    }
}
