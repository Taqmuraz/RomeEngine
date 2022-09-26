namespace RomeEngine.IO
{
    public sealed class ColladaJointInfo
    {
        public ColladaJointInfo(string jointName, int jointIndex, Matrix4x4 matrix)
        {
            JointName = jointName;
            JointIndex = jointIndex;
            Matrix = matrix;
        }

        public string JointName { get; }
        public int JointIndex { get; }
        public Matrix4x4 Matrix { get; }
    }
}
