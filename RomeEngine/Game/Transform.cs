using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class Transform : SerializableEventsHandler, ITransform
    {
        interface ITransformParentMode
        {
            void UnbindParent(ITransform oldParent);
            void BindParent(ITransform newParent);
            void UpdateParentMatrix();
        }
        sealed class TransformAnyParentMode : ITransformParentMode
        {
            Transform transform;

            public TransformAnyParentMode(Transform transform)
            {
                this.transform = transform;
            }

            public void BindParent(ITransform newParent)
            {
                transform.parent = newParent;
            }

            public void UpdateParentMatrix()
            {
                if (transform.parent != null)
                {
                    transform.ParentToWorld = transform.parent.LocalToWorld;
                }
                else transform.ParentToWorld = Matrix4x4.Identity;
            }

            public void UnbindParent(ITransform oldParent)
            {
                
            }
        }
        sealed class TransformDefaultParentMode : ITransformParentMode
        {
            Transform transform;

            public TransformDefaultParentMode(Transform transform)
            {
                this.transform = transform;
            }

            public void BindParent(ITransform newParent)
            {
                transform.parent = newParent;
                var newParentTransform = (Transform)newParent;
                newParentTransform.children.Add(transform);
                transform.UpdateParent(newParent.LocalToWorld);
            }

            public void UpdateParentMatrix()
            {
            }

            public void UnbindParent(ITransform oldParent)
            {
                ((Transform)oldParent).children.Remove(transform);
            }
        }

        ITransformParentMode parentMode;
        [SerializeField] ITransform parent;
        [SerializeField] List<Transform> children = new List<Transform>();
        [SerializeField] string name;

        public ReadOnlyArrayList<Transform> Children => children;

        Transform()
        {
        }

        public Transform(string name)
        {
            LocalScale = Vector3.one;
            parentMode = new TransformAnyParentMode(this);
            this.name = name;
        }

        public override string ToString()
        {
            return name;
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
        }

        bool hasToUpdate;

        void MarkToUpdate()
        {
            hasToUpdate = true;
        }

        void UpdateLocal()
        {
            hasToUpdate = false;
            parentMode.UpdateParentMatrix();
            Matrix4x4 rotationMatrix = Matrix4x4.CreateRotationMatrix(localRotation);
            LocalMatrix = Matrix4x4.CreateWorldMatrix((Vector3)rotationMatrix.column_0 * LocalScale.x, (Vector3)rotationMatrix.column_1 * LocalScale.y, (Vector3)rotationMatrix.column_2 * LocalScale.z, LocalPosition);
            LocalToWorld = (parent == null ? Matrix4x4.Identity : parent.LocalToWorld) * LocalMatrix;
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
            private set => localToWorld = value;
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
            private set => localMatrix = value;
        }

        public ITransform Parent
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
                    parentMode.UnbindParent(parent);
                }
                parent = value;
                parentMode = (parent is Transform) ? (ITransformParentMode)new TransformDefaultParentMode(this) : (ITransformParentMode)new TransformAnyParentMode(this);
                if (parent != null)
                {
                    parentMode.BindParent(parent);
                }
            }
        }
    }
}
