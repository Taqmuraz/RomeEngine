namespace RomeEngine
{
    public sealed class CustomMeshAttribute : IMeshAttributeInfo
    {
        public CustomMeshAttribute(int size, MeshAttributeType type)
        {
            Size = size;
            Type = type;
        }
        public int Size { get; }
        public MeshAttributeType Type { get; }
    }
}
