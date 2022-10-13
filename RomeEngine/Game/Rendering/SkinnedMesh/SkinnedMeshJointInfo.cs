namespace RomeEngine
{
    public sealed class SkinnedMeshJointInfo : IJointInfo
    {
        public SkinnedMeshJointInfo(Transform transform, Matrix4x4 inversedInitialState)
        {
            this.transform = transform;
            this.inversedInitialState = inversedInitialState;
        }

        Transform transform;
        Matrix4x4 inversedInitialState;
        public Matrix4x4 JointMatrix => transform.LocalToWorld * inversedInitialState;
    }
}
