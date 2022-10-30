namespace RomeEngine
{
    public sealed class SkeletonBone
    {
        public SkeletonBone(string name, ITransform transform)
        {
            Name = name;
            Transform = transform;
        }

        public string Name { get; }
        public ITransform Transform { get; }
    }
}