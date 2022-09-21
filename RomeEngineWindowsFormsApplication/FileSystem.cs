﻿using System.IO;

using RomeEngine;

namespace OneEngineWindowsFormsApplication
{
    class FileSystem : IFileSystem
    {
        public TextReader ReadText(string file)
        {
            return new StreamReader(file);
        }

        public TextWriter WriteText(string file)
        {
            return new StreamWriter(file);
        }

        public Stream OpenFileRead(string file)
        {
            return File.OpenRead(file);
        }

        public string[] GetFiles(string directory)
        {
            return Directory.GetFiles(directory);
        }

        public string[] GetDirectories(string directory)
        {
            return Directory.GetDirectories(directory);
        }

        public string GetFileName(string file)
        {
            return Path.GetFileName(file);
        }

        public string GetFileNameWithoutExtension(string file)
        {
            return Path.GetFileNameWithoutExtension(file);
        }

        public string GetParentDirectory(string directory)
        {
            return Directory.GetParent(directory).FullName;
        }

        public string GetFullPath(string relativePath)
        {
            return Path.GetFullPath(relativePath);
        }

        public string CombinePath(string a, string b)
        {
            return Path.Combine(a, b);
        }

        public void WriteAllText(string text, string file)
        {
            File.WriteAllText(file, text);
        }

        public void WriteAllBytes(byte[] bytes, string file)
        {
            File.WriteAllBytes(file, bytes);
        }

        public Stream OpenFileWrite(string file)
        {
            return File.OpenWrite(file);
        }

        public string GetFileExtension(string file)
        {
            return Path.GetExtension(file);
        }
    }
}
