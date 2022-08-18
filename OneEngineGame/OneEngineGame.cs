using OneEngine;
using OneEngine.IO;
using System.Collections.Generic;
using System.IO;
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
            GameScenes.InitializeGameScenes(AnimationEditorScene(), new GameScene[] { AnimationEditorScene() });
            engine.Initialize(runtime);
            return engine;
        }
        static GameScene AnimationEditorScene()
        {
            var camera = new GameObjectInstancer(() => new GameObject("Camera").AddComponent<Camera>().GameObject);
            var scene = new GameScene();
            scene.AddGameObjectInstancer(camera);
            scene.AddGameObjectInstancer(new GameObjectInstancer(() =>
            {
                var modelEditor = new GameObject("ModelEditor").AddComponent<HierarchyEditor>();
                return modelEditor.GameObject;
            }));
            return scene;
        }
    }
}
