namespace RomeEngine.IO
{
    public sealed class ColladaParsingInfo
    {
        public ColladaParsingInfo(string sourceFilePath)
        {
            SourceFilePath = sourceFilePath;
        }

        public string SourceFilePath { get; }
    }
}
