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
            GameScenes.InitializeGameScenes(LoadScene(), new GameScene[] { GameScene() });
            engine.Initialize(runtime);
            return engine;
        }
        static GameScene LoadScene()
        {
            var camera = new GameObjectInstancer(() => new GameObject("Camera").AddComponent<Camera>().GameObject);
            var text = new GameObjectInstancer(() =>
            {
                var textComponent = new GameObject("LoadScreenText").AddComponent<TextRenderer>();
                textComponent.FontSize = 0.1f;
                return textComponent.GameObject;
            });
            var scene = new GameScene();
            scene.AddGameObjectInstancer(camera);
            scene.AddGameObjectInstancer(text);
            scene.AddGameObjectInstancer(() => new GameObject("FPS").AddComponent<FpsRenderer>().GameObject);

            for (int i = 0; i < 1; i++)
            {
                int index = i;

                scene.AddGameObjectInstancer(new GameObjectInstancer(() =>
                {
                    using (var humanWriter = new MemoryStream())
                    {
                        var human = new GameObject("Human").AddComponent<HumanModel>().GameObject;
                        human.RemoveComponent(human.GetComponent<HumanModel>());

                        using (var animationWriter = new MemoryStream())
                        {
                            new Serializer().Serialize(human.GetComponent<Animator>().Animation, new BinarySerializationStream(animationWriter));
                        }

                        human.GetComponent<Animator>().PlayAnimation(null);

                        new Serializer().Serialize(human, new BinarySerializationStream(humanWriter));
                    }

                    using (TextReader animationReader = new StreamReader("./AnimationTest.txt"))
                    {
                        using (TextReader humanReader = new StreamReader("./HumanTest.txt"))
                        {
                            var human = (GameObject)new Serializer().Deserialize(new TextSerializationStream(humanReader, null));

                            var animation = (Animation)new Serializer().Deserialize(new TextSerializationStream(animationReader, null));

                            human.GetComponent<Animator>().PlayAnimation(animation);

                            return human;
                        }
                    }
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
