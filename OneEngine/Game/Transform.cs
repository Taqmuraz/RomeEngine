namespace OneEngine
{
	public sealed class Transform : Component
	{
		public Vector2 LocalPosition { get; set; }
		public float LocalRotation { get; set; }
		public Vector2 LocalScale { get; set; } = Vector2.one;

        public override bool IsUnary => true;

        public Vector2 LocalRight => new Vector2(Mathf.Cos(LocalRotation), Mathf.Sin(LocalRotation));
		public Vector2 LocalUp => new Vector2(Mathf.Cos(LocalRotation + 90f), Mathf.Sin(LocalRotation + 90f));

        protected internal override Transform GetTransform()
        {
			return this;
        }

        public Matrix3x3 LocalToWorld
		{
			get
			{
				if (Parent != null)
				{
					return Parent.LocalToWorld * LocalMatrix;
				}
				return LocalMatrix;
			}
		}

		public Matrix3x3 LocalMatrix => Matrix3x3.WorldTransform(LocalRight * LocalScale.x, LocalUp * LocalScale.y, LocalPosition);

		public Transform Parent
		{
			get
			{
				return parent;
			}
			set
			{
				if (value == this) throw new System.ArgumentException("Parent can't be equal to current instance");
				parent = value;
			}
		}
		Transform parent;

		public Vector2 TransformPointLocal(Vector2 point)
		{
			return LocalPosition + LocalRight * LocalScale.x * point.x + LocalUp * LocalScale.y * point.y;
		}
		public Vector2 TransformVectorLocal(Vector2 point)
		{
			return LocalRight * LocalScale.x * point.x + LocalUp * LocalScale.y * point.y;
		}
	}
}
