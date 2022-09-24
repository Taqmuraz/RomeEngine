namespace RomeEngine.IO
{
    public interface IColladaNode
    {
        string GetName();
        string GetAttribute(string name);
        string GetValue();
    }
}
