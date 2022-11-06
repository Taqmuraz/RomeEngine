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

            public void Sleep(float milliseconds)
            {
                Thread.Sleep(TimeSpan.FromSeconds(milliseconds));
            }
        }

        volatile Func<TResult> process;
        volatile Action<TResult> callback;
        volatile Action<Exception> exceptionCallback;
        volatile AsyncProcessPriority priority = AsyncProcessPriority.Low;

        public AsyncProcess(Func<TResult> process)
        {
            this.process = process;
        }

        public AsyncProcess(Func<TResult> process, Action<TResult> callback)
        {
            this.process = process;
            this.callback = callback;
        }

        public AsyncProcess(Func<TResult> process, Action<TResult> callback, AsyncProcessPriority priority)
        {
            this.process = process;
            this.callback = callback;
            this.priority = priority;
        }

        public AsyncProcess(Func<TResult> process, AsyncProcessPriority priority)
        {
            this.process = process;
            this.priority = priority;
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
                thread.IsBackground = priority == AsyncProcessPriority.Low;
                switch (priority)
                {
                    case AsyncProcessPriority.Low:
                        thread.Priority = ThreadPriority.Lowest;
                        break;
                    case AsyncProcessPriority.Medium:
                        thread.Priority = ThreadPriority.Normal;
                        break;
                    case AsyncProcessPriority.High:
                        thread.Priority = ThreadPriority.Highest;
                        break;
                }
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
