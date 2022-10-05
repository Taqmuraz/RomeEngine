using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class Transform : Component
	{
		[SerializeField(HideInInspector = true)] Transform parent;
		[SerializeField(HideInInspector = true)] List<Transform> children = new List<Transform>();

		public ReadOnlyArrayList<Transform> Children => children;

		public Transform()
		{
			LocalScale = Vector3.one;
		}

		Vector3 localPosition;
		[SerializeField]
		public Vector3 LocalPosition
		{
			get => localPosition;
			set
			{
				localPosition = value;
				UpdateLocal();
			}
		}
		[SerializeField]
		public Vector3 LocalRotation
		{
			get
			{
				return localRotation;
			}
			set
			{
				localRotationMatrix = Matrix4x4.CreateRotationMatrix(localRotation = value);
				UpdateLocal();
			}
		}
		Matrix4x4 localRotationMatrix = Matrix4x4.Identity;
		Vector3 localRotation;

		Vector3 localScale;
		[SerializeField]
		public Vector3 LocalScale
		{
			get => localScale;
			set
			{
				localScale = value;
				UpdateLocal();
			}
		}

		public Vector3 Position
		{
			get => (Vector3)LocalToWorld.column_3;
			set => LocalPosition = ParentToWorld.GetInversed().MultiplyPoint(value);
		}

		public Vector3 Rotation
		{
			get => LocalToWorld.GetEulerRotation();
			set => LocalRotation = value - ParentToWorld.GetEulerRotation();
		}

		public void ApplyMatrix(Matrix4x4 matrix)
		{
			LocalRotation = matrix.GetEulerRotation();
			LocalPosition = (Vector3)matrix.column_3;
			LocalScale = matrix.GetScale();
		}

        public override bool IsUnary => true;

		public Vector3 LocalRight => (Vector3)localRotationMatrix.column_0;
		public Vector3 LocalUp => (Vector3)localRotationMatrix.column_1;
		public Vector3 LocalForward => (Vector3)localRotationMatrix.column_2;

		public Vector3 Right => ParentToWorld.MultiplyDirection(LocalRight);
		public Vector3 Up => ParentToWorld.MultiplyDirection(LocalUp);
		public Vector3 Forward => ParentToWorld.MultiplyDirection(LocalForward);

		[BehaviourEvent]
		void OnDestroy()
		{
			Parent = null;
			foreach (var child in children.ToArray()) child.GameObject.Destroy();
		}

        protected internal override Transform GetTransform()
        {
			return this;
        }

		void UpdateLocal()
		{
			LocalMatrix = Matrix4x4.CreateWorldMatrix(LocalRight * LocalScale.x, LocalUp * LocalScale.y, LocalForward * LocalScale.z, LocalPosition);
			LocalToWorld = ParentToWorld * LocalMatrix;
			foreach (var child in children) child.UpdateParent(LocalToWorld);
		}
		void UpdateParent(Matrix4x4 parentToWorld)
		{
			ParentToWorld = parentToWorld;
			UpdateLocal();
		}

		public Matrix4x4 LocalToWorld { get; private set; } = Matrix4x4.Identity;

		public Matrix4x4 ParentToWorld { get; private set; } = Matrix4x4.Identity;

		public Matrix4x4 LocalMatrix { get; private set; } = Matrix4x4.Identity;

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
					UpdateParent(parent.LocalToWorld);
				}
				else
				{
					UpdateParent(Matrix4x4.Identity);
				}
			}
		}

		public Transform Root => Parent == null ? this : Parent.Root;
	}
}
