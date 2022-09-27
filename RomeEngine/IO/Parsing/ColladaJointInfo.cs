namespace RomeEngine.IO
{
    public sealed class ColladaJointInfo
    {
        public ColladaJointInfo(string jointName, int jointIndex)
        {
            JointName = jointName;
            JointIndex = jointIndex;
        }

        public string JointName { get; }
        public int JointIndex { get; }
    }
}
