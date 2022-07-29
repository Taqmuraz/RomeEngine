namespace OneEngine
{
	public static class Time
	{
		public static float CurrentTime => (float)(now - timeStart).TotalSeconds;
		public static float DeltaTime => (float)(currentUpdate - lastUpdate).TotalSeconds;

		static System.DateTime timeStart;
		static System.DateTime now => System.DateTime.Now;

		static System.DateTime lastUpdate;
		static System.DateTime currentUpdate;

		public static void Update()
		{
			lastUpdate = currentUpdate;
			currentUpdate = now;
		}

		public static void StartTime()
		{
			timeStart = now;
			lastUpdate = timeStart;
		}
	}
}
