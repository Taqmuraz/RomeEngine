namespace RomeEngine
{
    public sealed class SkeletonBone : Serializable
    {
        SkeletonBone()
        {
        }
        public SkeletonBone(string name, ITransform transform)
        {
            Name = name;
            Transform = transform;
        }

        [SerializeField] public string Name { get; private set; }
        [SerializeField] public ITransform Transform { get; private set; }
    }
}