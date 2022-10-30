using System.Collections.Generic;

namespace RomeEngine
{
	public class GameScene : IEventsHandler, IGameEntityActivityProvider
	{
		List<GameEntityInstancer> instancers = new List<GameEntityInstancer>();
		DynamicLinkedList<IGameEntity> gameEntities = new DynamicLinkedList<IGameEntity>();

		public string Name { get; private set; }
		public ReadOnlyArrayList<IGameEntity> GameEntities => new ReadOnlyArrayList<IGameEntity>(gameEntities);

		static readonly GameScene emptyScene = new GameScene("Empty");

        public GameScene(string name)
        {
            Name = name;
        }

        public static GameScene ActiveScene { get; private set; } = emptyScene;

		public void AddGameEntityInstancer (GameEntityInstancer instancer)
		{
			instancers.Add(instancer);
		}
		public void AddGameEntityInstancer(GameEntityInstancer.InstantiateDelegate instantiateFunc)
		{
			instancers.Add(new GameEntityInstancer(instantiateFunc));
		}
		void IGameEntityActivityProvider.Activate(IGameEntity gameEntity)
		{
			gameEntities.Add(gameEntity);
			gameEntity.CallEvent("Start");
		}
		void IGameEntityActivityProvider.Deactivate(IGameEntity gameEntity)
		{
			gameEntities.Remove(gameEntity);
			gameEntity.CallEvent("OnDestroy");
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

			foreach (var gameEntity in gameEntities)
			{
				gameEntity.CallEvent("OnDestroy");
			}
			gameEntities.Clear();

			ActiveScene = emptyScene;
		}

        public void CallEvent(string name)
        {
			foreach (var gameEntity in gameEntities)
			{
				gameEntity.CallEvent(name);
			}
        }
    }
}
