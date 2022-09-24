namespace RomeEngine
{
    public sealed class SkinnedMeshAttribute : IMeshAttributeInfo
    {
        public SkinnedMeshAttribute(int size, MeshAttributeType type)
        {
            Size = size;
            Type = type;
        }
        public int Size { get; }
        public MeshAttributeType Type { get; }
    }
}
