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
				MarkToUpdate();
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
				localRotation = value;
				MarkToUpdate();
			}
		}
		Vector3 localRotation;

		Vector3 localScale;
		[SerializeField]
		public Vector3 LocalScale
		{
			get => localScale;
			set
			{
				localScale = value;
				MarkToUpdate();
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

		public Vector3 LocalRight => (Vector3)LocalMatrix.column_0;
		public Vector3 LocalUp => (Vector3)LocalMatrix.column_1;
		public Vector3 LocalForward => (Vector3)LocalMatrix.column_2;

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

		bool hasToUpdate;

		void MarkToUpdate()
		{
			hasToUpdate = true;
		}

		void UpdateLocal()
		{
			hasToUpdate = false;
			Matrix4x4 rotationMatrix = Matrix4x4.CreateRotationMatrix(localRotation);
			LocalMatrix = Matrix4x4.CreateWorldMatrix((Vector3)rotationMatrix.column_0 * LocalScale.x, (Vector3)rotationMatrix.column_1 * LocalScale.y, (Vector3)rotationMatrix.column_2 * LocalScale.z, LocalPosition);
			LocalToWorld = ParentToWorld * LocalMatrix;
			foreach (var child in children) child.UpdateParent(LocalToWorld);
		}
		void UpdateParent(Matrix4x4 parentToWorld)
		{
			ParentToWorld = parentToWorld;
			MarkToUpdate();
		}

		Matrix4x4 localToWorld = Matrix4x4.Identity;
		Matrix4x4 localMatrix = Matrix4x4.Identity;

		public Matrix4x4 LocalToWorld
		{
			get
			{
				if (hasToUpdate)
				{
					UpdateLocal();
				}
				return localToWorld;
			}
			set => localToWorld = value;
		}

		public Matrix4x4 ParentToWorld { get; private set; } = Matrix4x4.Identity;

		public Matrix4x4 LocalMatrix
		{
			get
			{
				if (hasToUpdate)
				{
					UpdateLocal();
				}
				return localMatrix;
			}
			set => localMatrix = value;
		}

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
