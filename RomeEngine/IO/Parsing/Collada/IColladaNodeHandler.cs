namespace RomeEngine.IO
{
    public interface IColladaNodeHandler
    {
        bool CanParse(IColladaNode node);
        void ParseStart(IColladaNode node);
        void ParseEnd(IColladaNode node);
    }
}
