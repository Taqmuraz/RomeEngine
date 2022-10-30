namespace RomeEngine
{
    public sealed class Transform : SerializableEventsHandler, ITransform
    {
        Matrix4x4 localToWorld;
        public Matrix4x4 LocalToWorld
        {
            get
            {
                if (hasChanges)
                {
                    localToWorld = Matrix4x4.CreateWorldMatrix(Position, Rotation, Scale);
                    hasChanges = false;
                }
                return localToWorld;
            }
            set => localToWorld = value;
        }
        public Vector3 Right => (Vector3)LocalToWorld.column_0;
        public Vector3 Up => (Vector3)LocalToWorld.column_1;
        public Vector3 Forward => (Vector3)LocalToWorld.column_2;

        Vector3 position;
        Vector3 rotation;
        Vector3 scale = Vector3.one;
        bool hasChanges;

        [SerializeField] public Vector3 Position
        {
            get => position;
            set
            {
                hasChanges = true;
                position = value;
            }
        }
        [SerializeField] public Vector3 Rotation
        {
            get => rotation;
            set
            {
                hasChanges = true;
                rotation = value;
            }
        }
        [SerializeField] public Vector3 Scale
        {
            get => scale;
            set
            {
                hasChanges = true;
                scale = value;
            }
        }

        public Transform()
        {
            LocalToWorld = Matrix4x4.Identity;
        }

        public void ApplyMatrix(Matrix4x4 matrix)
        {
            Rotation = matrix.GetEulerRotation();
            Position = (Vector3)matrix.column_3;
            Scale = matrix.GetScale();
        }

        string IGameEntity.Name => ToString();

        void IGameEntity.Activate(IGameEntityActivityProvider activityProvider) => activityProvider.Activate(this);
        void IGameEntity.Deactivate(IGameEntityActivityProvider activityProvider) => activityProvider.Deactivate(this);
    }
}
