using System.IO;

namespace RomeEngine
{
    public interface IFileSystem
    {
        TextReader ReadText(string file);
        TextWriter WriteText(string file);
        Stream OpenFileRead(string file);
        Stream OpenFileWrite(string file);
        string[] GetFiles(string directory);
        string[] GetDirectories(string directory);
        string GetFileName(string file);
        string GetFileExtension(string file);
        string GetFileNameWithoutExtension(string file);
        string GetParentDirectory(string directory);
        string GetFullPath(string relativePath);
        string CombinePath(string a, string b);
        string RelativePath(string path);
        void WriteAllText(string text, string file);
        void WriteAllBytes(byte[] bytes, string file);
        string FileWithExtension(string file, string extension);
    }
}
