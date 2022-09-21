using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public interface IEngineRuntine
    {
        void SetInputHandler(IInputHandler inputHandler);
        void Log(string message);
        void ShowFileOpenDialog(string root, string title, Action<string> callback);
        void ShowFileWriteDialog(string root, string fileName, string title, Action<string> callback);
        ISystemInfo SystemInfo { get; }
        IFileSystem FileSystem { get; }
    }
}
