namespace OneEngine
{
	public sealed class Transform : Component
	{
		public Vector2 position { get; set; }
		public float rotation { get; set; }

		public Vector2 right => new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
		public Vector2 up => new Vector2(Mathf.Cos(rotation + 90f), Mathf.Sin(rotation + 90f));

		public Matrix3x3 localToWorld => Matrix3x3.WorldTransform(right, up, position);

		public Vector2 TransformPoint(Vector2 point)
		{
			return position + right * point.x + up * point.y;
		}
		public Vector2 TransformVector(Vector2 point)
		{
			return right * point.x + up * point.y;
		}
	}
}
