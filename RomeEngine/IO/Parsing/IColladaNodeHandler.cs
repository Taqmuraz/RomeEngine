namespace RomeEngine.IO
{
    public interface IColladaNodeHandler<TContext> where TContext : IColladaParsingContext
    {
        string Name { get; }
        void ParseStart(TContext context, IColladaNode node);
        void ParseEnd(TContext context, IColladaNode node);
    }
}
