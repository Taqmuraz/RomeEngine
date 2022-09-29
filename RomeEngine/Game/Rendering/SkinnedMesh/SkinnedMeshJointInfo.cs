namespace RomeEngine
{
    public sealed class SkinnedMeshJointInfo : IJointInfo
    {
        public SkinnedMeshJointInfo(Transform transform, Matrix4x4 initialState)
        {
            Transform = transform;
            InitialState = initialState;
        }

        public Transform Transform { get; }
        public Matrix4x4 InitialState { get; }
    }
}
