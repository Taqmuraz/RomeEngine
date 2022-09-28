namespace RomeEngine.IO
{
    public interface IColladaNode
    {
        ReadOnlyArray<ColladaNodeAttribute> Attributes { get; }
        ColladaNodeAttribute GetAttribute(string name);
        bool HasAttribute(string name);
        string NodeType { get; }
        string GetValue();
    }
}
