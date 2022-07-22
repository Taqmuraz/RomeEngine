using OneEngine;
using System;
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
            var camera = new GameObjectInstancer(() => new GameObject("Camera").AddComponent<Camera>().gameObject);
            var text = new GameObjectInstancer(() => new GameObject("LoadScreenText").AddComponent<TextRenderer>().gameObject);
            var scene = new GameScene();
            scene.AddGameObjectInstancer(camera);
            scene.AddGameObjectInstancer(text);
            return scene;
        }
        static GameScene GameScene()
        {
            var camera = new GameObjectInstancer(() => new GameObject("Camera").AddComponent<Camera>().gameObject);
            var text = new GameObjectInstancer(() => new GameObject("GameSceneText").AddComponent<TextRenderer>().gameObject);
            var scene = new GameScene();
            scene.AddGameObjectInstancer(camera);
            scene.AddGameObjectInstancer(text);
            return scene;
        }
    }
}
