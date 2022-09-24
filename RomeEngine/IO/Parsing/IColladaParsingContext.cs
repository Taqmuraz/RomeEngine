namespace RomeEngine.IO
{
    public interface IColladaParsingContext
    {
        string RootNodeName { get; }
        void ParseStart(IColladaNode node);
        void ParseEnd(IColladaNode node);
    }
}
