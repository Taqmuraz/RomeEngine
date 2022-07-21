namespace OneEngine
{
    public struct Ray
	{
		public readonly Vector2 origin;
		public readonly Vector2 direction;

		public Ray(Vector2 origin, Vector2 direction)
		{
			this.origin = origin;
			this.direction = direction.normalized;
		}
	}
}

