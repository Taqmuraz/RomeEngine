using System.Collections.Generic;

namespace RomeEngine
{
	public class GameScene : IEventsHandler
	{
		List<GameObjectInstancer> instancers = new List<GameObjectInstancer>();
		DynamicLinkedList<GameObject> gameObjects = new DynamicLinkedList<GameObject>();

		public string Name { get; private set; }
		public ReadOnlyArrayList<GameObject> GameObjects => new ReadOnlyArrayList<GameObject>(gameObjects);

		static readonly GameScene emptyScene = new GameScene("Empty");

        public GameScene(string name)
        {
            Name = name;
        }

        public static GameScene ActiveScene { get; private set; } = emptyScene;

		public void AddGameObjectInstancer (GameObjectInstancer instancer)
		{
			instancers.Add(instancer);
		}
		public void AddGameObjectInstancer(GameObjectInstancer.InstantiateDelegate instantiateFunc)
		{
			instancers.Add(new GameObjectInstancer(instantiateFunc));
		}
		public void AddGameObject(GameObject gameObject)
		{
			gameObjects.Add(gameObject);
			gameObject.CallEvent("OnActivate");
		}
		public void RemoveGameObject(GameObject gameObject)
		{
			gameObjects.Remove(gameObject);
			gameObject.CallEvent("OnDeactivate");
		}

		public void LoadScene()
		{
			if (ActiveScene == this)
			{
				Debug.LogError("Scene already loaded");
				return;
			}

			ActiveScene.UnloadScene();
			ActiveScene = this;

			foreach (var instancer in instancers)
			{
				try
				{
					instancer.Instantiate();
				}
				catch (System.Exception ex)
				{
					Debug.LogError(ex);
				}
			}
		}
		public void UnloadScene()
		{
			if (ActiveScene != this)
			{
				Debug.LogError("Scene not loaded yet, nothing to unload");
				return;
			}

			foreach (var gameObject in gameObjects)
			{
				gameObject.Destroy();
			}

			ActiveScene = emptyScene;
		}

        public void CallEvent(string name)
        {
			foreach (var gameObject in gameObjects)
			{
				gameObject.CallEvent(name);
			}
        }
    }
}
