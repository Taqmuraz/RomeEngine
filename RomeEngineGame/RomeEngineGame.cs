﻿using RomeEngine;
using RomeEngine.IO;
using RomeEngine.UI;
using RomeEngineCubeWorld;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomeEngineEditor
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
            var camera = new GameEntityInstancer(() => new GameObject("Camera").AddComponent<Camera>().GameObject);
            var scene = new GameScene("Level select scene");
            scene.AddGameEntityInstancer(camera);
            scene.AddGameEntityInstancer(new GameEntityInstancer(() => new GameObject("Level select menu").AddComponent<LevelSelectMenu>().GameObject));
            return scene;
        }
        static GameScene AnimationEditorScene()
        {
            var camera = new GameEntityInstancer(() => new GameObject("Camera").AddComponent<Camera>().GameObject);
            var scene = new GameScene("Editor scene");
            scene.AddGameEntityInstancer(camera);
            scene.AddGameEntityInstancer(new GameEntityInstancer(() =>
            {
                var modelEditor = new GameObject("ModelEditor").AddComponent<HierarchyEditor>();
                return modelEditor.GameObject;
            }));
            return scene;
        }
        static GameScene GamePlayScene()
        {
            var camera = new GameEntityInstancer(() =>
            {
                var instance = new GameObject("Camera").AddComponent<Camera>();
                instance.ClearColor = new Color32(0.3f, 0.5f, 0.6f, 1f);
                return instance.GameObject;
            });
            var light = new GameEntityInstancer(() =>
            {
                var lightObject = new GameObject("Light").AddComponent<GlobalLight>();

                lightObject.Transform.Rotation = new Vector3(75f, 135f, 0f);
                lightObject.Setup(0.1f, 0.3f, Color32.white);

                return lightObject.GameObject;
            });

            var scene = new GameScene("Game scene");
            scene.AddGameEntityInstancer(camera);
            scene.AddGameEntityInstancer(light);

            scene.AddGameEntityInstancer(new GameEntityInstancer(() => new GameObject("Terrain").AddComponent<TerrainRenderer>().GameObject));

            scene.AddGameEntityInstancer(new GameEntityInstancer(() =>
            {
                var player = Resources.LoadInstance<GameObject>("Models/KnightFemale.bin");
                player.Name = "Player";
                player.AddComponent<PlayerController>();
                //var collider = player.AddComponent<SphereCollider>();
                //collider.PhysicalBody = new SimpleDynamicBody(player.Transform);
                player.Transform.Position = new Vector3(0f, 0f, 0f);
                return player;
            }));

            scene.AddGameEntityInstancer(() =>
            {
                var chunk = new CubeChunk();
                chunk.ModifyCube(new CubeCustomModifier(c => c.WithId(1)), new CubeCoords(3, 1, 3));
                for (int x = 0; x < chunk.Width; x++)
                {
                    for (int y = 0; y < chunk.Width; y++)
                    {
                        if (((x + y) & 1) == 0) continue;

                        chunk.ModifyCube(new CubeCustomModifier(c => c.WithId(y)), new CubeCoords(x, 0, y));
                    }
                }
                var mesh = chunk.BuildMesh();
                var rendererEntity = new GameObject("CubeRenderer");
                var renderer = rendererEntity.AddComponent<GenericMeshRenderer>();
                renderer.GenericMesh = mesh;
                renderer.Material = new SingleTextureMaterial("Grass") { TextureFileName = "./Resources/Textures/BlocksMap.jpg" };
                return rendererEntity;
            });

            for (int i = 0; i < 10; i++)
            {
                int index = i;
                scene.AddGameEntityInstancer(new GameEntityInstancer(() =>
                {
                    var sphere = Resources.LoadInstance<GameObject>("Models/KnightFemale.bin");
                    sphere.Name = "Sphere";
                    sphere.AddComponent<HumanAnimator>().PlayAnimation("Sword_Idle");
                    float angle = 36f * index;
                    sphere.Transform.Position = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * 3f;
                    sphere.Transform.Rotation = new Vector3(0f, -angle + 90f, 0f);

                    //sphere.AddComponent<SphereCollider>();
                    return sphere;
                }));
            }

            scene.AddGameEntityInstancer(() =>
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
