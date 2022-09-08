namespace RomeEngine.IO
{
    public interface ISerializationHandler
    {
        void OnSerialize();
        void OnDeserialize();
    }
}
