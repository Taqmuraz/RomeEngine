namespace RomeEngine
{
    public struct Ray2D
	{
		public readonly Vector2 origin;
		public readonly Vector2 direction;

		public Ray2D(Vector2 origin, Vector2 direction)
		{
			this.origin = origin;
			this.direction = direction.normalized;
		}
	}
}

