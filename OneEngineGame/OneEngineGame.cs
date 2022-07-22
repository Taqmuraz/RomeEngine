using OneEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEngineGame
{
    public static class OneEngineGame
    {
        public static IEngine StartGame(IEngineRuntine runtime)
        {
            var engine = new Engine();
            GameScenes.InitializeGameScenes(LoadScene(), new GameScene[] { GameScene() });
            engine.Initialize(runtime);
            return engine;
        }
        static GameScene LoadScene()
        {
            var camera = new GameObjectInstancer(() => new GameObject("Camera").AddComponent<Camera>().GameObject);
            var text = new GameObjectInstancer(() => new GameObject("LoadScreenText").AddComponent<TextRenderer>().GameObject);
            var scene = new GameScene();
            scene.AddGameObjectInstancer(camera);
            scene.AddGameObjectInstancer(text);
            scene.AddGameObjectInstancer(() => new GameObject("FPS").AddComponent<FpsRenderer>().GameObject);

            for (int i = 0; i < 100; i++)
            {
                int index = i;

                scene.AddGameObjectInstancer(new GameObjectInstancer(() =>
                {
                    var obj = new GameObject($"Human").AddComponent<HumanModel>().GameObject;
                    obj.Transform.LocalPosition = new Vector2(-index, 0f);
                    return obj;
                }));
            }
            return scene;
        }
        static GameScene GameScene()
        {
            var camera = new GameObjectInstancer(() => new GameObject("Camera").AddComponent<Camera>().GameObject);
            var text = new GameObjectInstancer(() => new GameObject("GameSceneText").AddComponent<TextRenderer>().GameObject);
            var scene = new GameScene();
            scene.AddGameObjectInstancer(camera);
            scene.AddGameObjectInstancer(text);
            return scene;
        }
    }
}
