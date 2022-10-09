using System.Threading;
using System.Collections.Generic;

namespace RomeEngine
{
	public static class Game
	{
		public abstract class GameThreadHandler : SerializableEventsHandler
		{
			public bool IsActive { get; private set; }
			public bool IsDestroyed { get; private set; }

			public GameThreadHandler()
			{
				
			}

			[BehaviourEvent]
			void OnActivate()
			{
				IsActive = true;
			}
			[BehaviourEvent]
			void OnDeactivate()
			{
				IsActive = false;
			}

			public void Destroy()
			{
				if (IsDestroyed) return;

				try
				{
					CallEvent("OnDestroy");
				}
				catch (System.Exception ex)
				{
					Debug.Log(ex.ToString());
				}
				IsDestroyed = true;
			}
		}

		public static void InitializeGame()
		{
			GameScenes.gameScenes[1].LoadScene();
		}

		static void SendMessageToActiveScene(string message)
		{
			GameScene.ActiveScene?.CallEvent(message);
		}

		public static void UpdateGameState()
		{
			Time.Update();

			SendMessageToActiveScene("EarlyUpdate");

			Collider.UpdatePhysics();
			Routine.UpdateDelayed();

			SendMessageToActiveScene("Update");

			SendMessageToActiveScene("LateUpdate");

			Input.UpdateInput();
		}
		public static void UpdateGraphics2D(IGraphics2D graphics2D)
		{
			SendMessageToActiveScene("OnPreRender2D");
			Renderer2D.Update2DGraphics(graphics2D);
			SendMessageToActiveScene("OnPostRender2D");
		}
		public static void UpdateGraphics3D(IGraphics graphics, IGraphicsContext context)
		{
			SendMessageToActiveScene("OnPreRender");
			Renderer.UpdateGraphics(graphics, context);
			SendMessageToActiveScene("OnPostRender");
		}
	}
}
