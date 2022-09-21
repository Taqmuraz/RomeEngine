using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class Transform2D : Transform
	{
		[SerializeField(HideInInspector = true)] Transform2D parent;
		[SerializeField(HideInInspector = true)] List<Transform2D> children = new List<Transform2D>();

		public ReadOnlyArrayList<Transform2D> Children => children;

		[SerializeField] public Vector2 LocalPosition { get; set; }
		[SerializeField] public float LocalRotation { get; set; }
		[SerializeField] public Vector2 LocalScale { get; set; } = Vector2.one;
		[SerializeField] public bool FlipX { get; set; }
		[SerializeField] public bool FlipY { get; set; }

		public Vector2 Position
		{
			get => (Vector2)LocalToWorld.Column_2;
			set => LocalPosition = ParentToWorld.GetInversed().MultiplyPoint(value);
		}
		public Vector2 Scale => LocalToWorld.MultiplyScale(Vector2.one);

        public override bool IsUnary => true;

        public Vector2 LocalRight => new Vector2(Mathf.Cos(LocalRotation), Mathf.Sin(LocalRotation)) * (FlipY ? -1f : 1f);
		public Vector2 LocalUp => new Vector2(Mathf.Cos(LocalRotation + 90f), Mathf.Sin(LocalRotation + 90f)) * (FlipX ? -1f : 1f);

		[BehaviourEvent]
		void OnDestroy()
		{
			Parent = null;
			foreach (var child in children.ToArray()) child.GameObject.Destroy();
		}

        protected internal override Transform2D GetTransform()
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

		public Transform2D Parent
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

		public Transform2D Root => Parent == null ? this : Parent.Root;

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
