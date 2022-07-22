using System.Collections.Generic;

namespace OneEngine
{
	public static class GameScenes
	{
		public static ReadOnlyArrayList<GameScene> gameScenes { get; private set; }
		public static int SceneToLoad { get; private set; }

		public static void InitializeGameScenes(GameScene loadingScene, ReadOnlyArray<GameScene> gameScenes)
        {
            var scenesList = new List<GameScene>();
			scenesList.Add(loadingScene);
			scenesList.AddRange(gameScenes);
			GameScenes.gameScenes = scenesList;
        }

        public static void LoadScene(int index)
		{
			SceneToLoad = index;
			gameScenes[0].LoadScene();
		}
	}
}
