using System.Threading;
using System.Collections.Generic;

namespace RomeEngine
{
	public static class Game
	{
		static List<SerializableEventsHandler> gameHandlers = new List<SerializableEventsHandler>();
		static List<SerializableEventsHandler> handlersToAdd = new List<SerializableEventsHandler>();
		static List<SerializableEventsHandler> handlersToRemove = new List<SerializableEventsHandler>();

		public abstract class GameThreadHandler : SerializableEventsHandler
		{
			bool m_active;
			public bool IsActive
			{
				get => m_active && !destroyed;
				set => m_active = value;
			}

			bool destroyed;

			public GameThreadHandler()
			{
				IsActive = true;
				handlersToAdd.Add(this);
			}

			public void Destroy()
			{
				if (destroyed) return;

				try
				{
					CallEvent("OnDestroy");
				}
				catch (System.Exception ex)
				{
					Debug.Log(ex.ToString());
				}
				handlersToRemove.Add(this);
				destroyed = true;
			}
		}
		static void SendHandlersMessage(string message)
		{
			if (handlersToAdd.Count != 0) gameHandlers.AddRange(handlersToAdd);
			for (int i = 0; i < handlersToRemove.Count; i++) gameHandlers.Remove(handlersToRemove[i]);
			handlersToAdd.Clear();
			handlersToRemove.Clear();

			lock (gameHandlers)
			{
				foreach (var handler in gameHandlers.ToArray())
				{
					try
					{
						handler.CallEvent(message);
					}
					catch (System.Exception ex)
					{
						Debug.Log(ex.ToString());
					}
				}
			}
		}

		public static void InitializeGame()
		{
			GameScenes.gameScenes[1].LoadScene();
		}

		public static void UpdateGameState()
		{
			Time.Update();

			SendHandlersMessage("EarlyUpdate");

			// update physics here

			SendHandlersMessage("Update");

			SendHandlersMessage("LateUpdate");

			Input.UpdateInput();
		}
		public static void UpdateGraphics(IGraphics2D graphics2D, IGraphics graphics, IGraphicsContext context)
		{
			SendHandlersMessage("OnPreRender");
			Renderer.UpdateGraphics(graphics, context);
			SendHandlersMessage("OnPostRender");
			SendHandlersMessage("OnPreRender2D");
			Renderer2D.Update2DGraphics(graphics2D);
			SendHandlersMessage("OnPostRender2D");
		}
	}
}
