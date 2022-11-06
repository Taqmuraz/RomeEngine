namespace RomeEngine
{
    public interface IAsyncProcessHandle
    {
        bool IsRunning { get; }
        void Abort();
        void Sleep(float milliseconds);
    }
}
