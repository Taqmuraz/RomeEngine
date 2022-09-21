using System;
using System.Collections.Generic;

namespace RomeEngine
{
	public static class Debug
	{
		struct Line
		{
			public Vector2 a;
			public Vector2 b;
			public Color32 color;

			public Line(Vector2 a, Vector2 b, Color32 color)
			{
				this.a = a;
				this.b = b;
				this.color = color;
			}
		}

		static List<Line> lines = new List<Line>();

		public static void Log(object message)
		{
			Engine.Instance.Runtime.Log(message.ToString());
		}

		public static void DrawDebug(IGraphics2D graphics)
        {
			for (int i = 0; i < lines.Count; i++)
			{
				var line = lines[i];
				graphics.Brush = new SingleColorBrush(line.color);
				graphics.DrawLine(line.a, line.b);
			}
			lines.Clear();
		}

		public static void DrawLine(Vector2 a, Vector2 b, Color32 color)
		{
			lines.Add(new Line(a, b, color));
		}

        public static void LogError(object obj)
        {
			Log(obj.ToString());
        }

        public static void DrawRay(Vector2 origin, Vector2 direction, Color32 color)
		{
			lines.Add(new Line(origin, origin + direction, color));
		}
	}
}
