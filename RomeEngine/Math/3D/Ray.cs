namespace RomeEngine
{
    public struct Ray
	{
		public readonly Vector3 origin;
		public readonly Vector3 direction;

		public Ray(Vector3 origin, Vector3 direction)
		{
			this.origin = origin;
			this.direction = direction.Normalized;
		}
	}
}

