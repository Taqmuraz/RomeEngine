namespace RomeEngine
{
    public interface IMeshAttributeInfo
    {
        int ElementSize { get; }
        int Layout { get; }
        MeshAttributeFormat Type { get; }
    }
}