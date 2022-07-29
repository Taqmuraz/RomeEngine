using System.Collections.Generic;

namespace OneEngine
{
	public sealed class Transform : Component
	{
		[SerializeField] Transform parent;
		[SerializeField] List<Transform> children = new List<Transform>();

		public ReadOnlyArrayList<Transform> Children => children;

		[SerializeField] public Vector2 LocalPosition { get; set; }
		[SerializeField] public float LocalRotation { get; set; }
		[SerializeField] public Vector2 LocalScale { get; set; } = Vector2.one;

		public Vector2 Position => (Vector2)LocalToWorld.Column_2;
		public Vector2 Scale => LocalToWorld.MultiplyScale(Vector2.one);

        public override bool IsUnary => true;

        public Vector2 LocalRight => new Vector2(Mathf.Cos(LocalRotation), Mathf.Sin(LocalRotation));
		public Vector2 LocalUp => new Vector2(Mathf.Cos(LocalRotation + 90f), Mathf.Sin(LocalRotation + 90f));

		[BehaviourEvent]
		void OnDestroy()
		{
			parent = null;
		}

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
		public Matrix3x3 ParentToWorld => Parent == null ? Matrix3x3.identity : Parent.LocalToWorld;

		public Matrix3x3 LocalMatrix => Matrix3x3.WorldTransform(LocalRight * LocalScale.x, LocalUp * LocalScale.y, LocalPosition);

		public Transform Parent
		{
			get
			{
				return parent;
			}
			set
			{
				if (value == parent) return;
				if (value == this) throw new System.ArgumentException("Parent can't be equal to current instance");
				if (parent != null)
				{
					parent.children.Remove(this);
				}
				parent = value;
				if (parent != null)
				{
					parent.children.Add(this);
				}
			}
		}

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
