using System;
using System.Collections.Generic;
namespace OneEngine
{
    public interface IEngineRuntine
    {
        void SetInputHandler(IInputHandler inputHandler);
        void Log(string message);
        ISystemInfo SystemInfo { get; }
    }
}
