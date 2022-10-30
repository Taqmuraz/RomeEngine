using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class HierarchyTransform : SerializableEventsHandler, ITransform
    {
        [SerializeField] ITransform parent;
        [SerializeField] List<HierarchyTransform> children = new List<HierarchyTransform>();
        [SerializeField] string name;
        string IGameEntity.Name => name;

        public ReadOnlyArrayList<HierarchyTransform> Children => children;

        HierarchyTransform()
        {
        }

        public HierarchyTransform(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }

        [SerializeField] public Vector3 LocalPosition { get; set; }
        [SerializeField] public Vector3 LocalRotation { get; set; }
        [SerializeField] public Vector3 LocalScale { get; set; } = Vector3.one; 

        public void ApplyMatrix(Matrix4x4 matrix)
        {
            LocalRotation = matrix.GetEulerRotation();
            LocalPosition = (Vector3)matrix.column_3;
            LocalScale = matrix.GetScale();
        }

        public Vector3 LocalRight => (Vector3)localMatrix.column_0;
        public Vector3 LocalUp => (Vector3)localMatrix.column_1;
        public Vector3 LocalForward => (Vector3)localMatrix.column_2;

        public Vector3 Right => parentToWorld.MultiplyDirection(LocalRight);
        public Vector3 Up => parentToWorld.MultiplyDirection(LocalUp);
        public Vector3 Forward => parentToWorld.MultiplyDirection(LocalForward);

        [BehaviourEvent]
        void OnDestroy()
        {
            Parent = null;
        }

        void UpdateLocal()
        {
            localMatrix = Matrix4x4.CreateWorldMatrix(LocalPosition, LocalRotation, LocalScale);
            localToWorld = parentToWorld * localMatrix;
            foreach (var child in children) child.UpdateParent(localToWorld);
        }
        void UpdateParent(Matrix4x4 parentToWorld)
        {
            this.parentToWorld = parentToWorld;
            UpdateLocal();
        }
        public void UpdateHierarchy()
        {
            UpdateParent(parent == null ? Matrix4x4.Identity : parent.LocalToWorld);
        }

        Matrix4x4 localToWorld = Matrix4x4.Identity;
        Matrix4x4 localMatrix = Matrix4x4.Identity;
        Matrix4x4 parentToWorld = Matrix4x4.Identity;

        public Matrix4x4 LocalToWorld => localToWorld;

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
                if (parent is HierarchyTransform transformParent)
                {
                    transformParent.children.Remove(this);
                }
                parent = value;
                if (parent is HierarchyTransform transformParentNew)
                {
                    transformParentNew.children.Add(this);
                }
            }
        }

        Vector3 ITransform.Position
        {
            get => LocalPosition;
            set => LocalPosition = value;
        }
        Vector3 ITransform.Rotation
        {
            get => LocalRotation;
            set => LocalRotation = value;
        }
        Vector3 ITransform.Scale
        {
            get => LocalScale;
            set => LocalScale = value;
        }

        void IGameEntity.Activate(IGameEntityActivityProvider activityProvider)
        {
            UpdateHierarchy();
            activityProvider.Activate(this);
        }

        void IGameEntity.Deactivate(IGameEntityActivityProvider activityProvider)
        {
            activityProvider.Deactivate(this);
        }
    }
}
