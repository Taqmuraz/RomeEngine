using System.IO;

namespace RomeEngine
{
    public interface IFileSystem
    {
        TextReader ReadText(string file);
        TextWriter WriteText(string file);
        Stream OpenFile(string file);
        string[] GetFiles(string directory);
        string[] GetDirectories(string directory);
        string GetFileName(string file);
        string GetFileNameWithoutExtension(string file);
        string GetParentDirectory(string directory);
        string GetFullPath(string relativePath);
        string CombinePath(string a, string b);
        void WriteAllText(string text, string file);
        void WriteAllBytes(byte[] bytes, string file);
    }
}
