namespace RomeEngine.IO
{
    public sealed class ColladaParsingInfo
    {
        public ColladaParsingInfo(string sourceFilePath, IFileSystem fileSystem)
        {
            SourceFilePath = sourceFilePath;
            FileSystem = fileSystem;
        }

        public string SourceFilePath { get; }
        public IFileSystem FileSystem { get; }
    }
}
