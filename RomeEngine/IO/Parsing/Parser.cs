using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class Parser
    {
        static Parser()
        {
            Parsers = new[] { new ColladaParser() };
        }

        static IEnumerable<IParser> Parsers { get; }

        public static bool TryParseFile(string fileName, out ISerializable result)
        {
            var parser = Parsers.FirstOrDefault(p => p.CanParse(fileName));
            if (parser != null)
            {
                result = parser.ParseObject(fileName);
                return true;
            }
            result = null;
            return false;
        }
    }
}
