using System.Collections.Generic;

namespace RomeEngine
{
	public static class GameScenes
	{
		public static ReadOnlyArrayList<GameScene> GameScenesList { get; private set; }

		public static void InitializeGameScenes(ReadOnlyArray<GameScene> gameScenes)
        {
            var scenesList = new List<GameScene>();
			scenesList.AddRange(gameScenes);
			GameScenesList = scenesList;
        }

        public static void LoadScene(int index)
		{
			GameScenesList[index].LoadScene();
		}
	}
}
