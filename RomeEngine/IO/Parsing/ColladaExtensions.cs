namespace RomeEngine.IO
{
    public static class ColladaExtensions
    {
        public static char[] StringSeparators { get; } = new char[] { ' ', '\n', '\t', '\r' };

        public static string[] SeparateString(this string source)
        {
            return source.Split(StringSeparators, System.StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
