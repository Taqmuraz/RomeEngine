namespace RomeEngine
{
    public interface IAsyncProcessHandle
    {
        bool IsRunning { get; }
        void Abort();
    }
}
