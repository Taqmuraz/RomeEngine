using System.Collections.Generic;

namespace RomeEngine
{
	public class GameScene : IEventsHandler, IGameObjectActivityProvider
	{
		List<GameObjectInstancer> instancers = new List<GameObjectInstancer>();
		DynamicLinkedList<IGameObject> gameObjects = new DynamicLinkedList<IGameObject>();

		public string Name { get; private set; }
		public ReadOnlyArrayList<IGameObject> GameObjects => new ReadOnlyArrayList<IGameObject>(gameObjects);

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
		void IGameObjectActivityProvider.Activate(IGameObject gameObject)
		{
			gameObjects.Add(gameObject);
			gameObject.CallEvent("Start");
		}
		void IGameObjectActivityProvider.Deactivate(IGameObject gameObject)
		{
			gameObjects.Remove(gameObject);
			gameObject.CallEvent("OnDestroy");
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
					instancer.Instantiate().Activate(this);
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
				gameObject.CallEvent("OnDestroy");
			}
			gameObjects.Clear();

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
