using System;
using System.Threading;

namespace RomeEngine
{
    public sealed class AsyncProcess<TResult>
    {
        sealed class ProcessHandle : IAsyncProcessHandle
        {
            Thread thread;

            public ProcessHandle(Thread thread)
            {
                this.thread = thread;
            }

            public void Abort()
            {
                if (thread.IsAlive) thread.Abort();
            }

            public bool IsRunning => thread.IsAlive;
        }

        volatile Func<TResult> process;
        volatile Action<TResult> callback;
        volatile Action<Exception> exceptionCallback;

        public AsyncProcess(Func<TResult> process, Action<TResult> callback)
        {
            this.process = process;
            this.callback = callback;
        }

        public AsyncProcess(Func<TResult> process, Action<TResult> callback, Action<Exception> exceptionCallback)
        {
            this.process = process;
            this.callback = callback;
            this.exceptionCallback = exceptionCallback;
        }

        public IAsyncProcessHandle Start()
        {
            Thread thread = new Thread(ThreadProcess);

            try
            {
                thread.IsBackground = true;
                thread.Priority = ThreadPriority.Lowest;
                thread.Start();
            }
            catch (Exception ex)
            {
                exceptionCallback?.Invoke(ex);
            }

            return new ProcessHandle(thread);
        }
        void ThreadProcess()
        {
            TResult result = default;

            float time = Time.CurrentTime;

            try
            {
                result = process();
            }
            catch(Exception ex)
            {
                exceptionCallback?.Invoke(ex);
            }

            Routine.StartRoutineDelayed(new SingleCallRoutine(() => callback(result)));
        }
    }
}
