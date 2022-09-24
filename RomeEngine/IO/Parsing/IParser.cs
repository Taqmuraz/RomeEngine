namespace RomeEngine.IO
{
    public interface IParser
    {
        bool CanParse(string fileName);
        ISerializable ParseObject(string fileName);
    }
}
