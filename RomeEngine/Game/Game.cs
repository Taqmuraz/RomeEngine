using System.Threading;
using System.Collections.Generic;

namespace RomeEngine
{
	public static class Game
	{
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
