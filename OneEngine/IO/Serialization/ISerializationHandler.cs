namespace OneEngine.IO
{
    public interface ISerializationHandler
    {
        void OnSerialize();
        void OnDeserialize();
    }
}
