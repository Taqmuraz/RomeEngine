using System;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class Parser
    {
        static Parser()
        {
            Parsers = new Func<IParser>[] { () => new ColladaParser(Engine.Instance.Runtime.FileSystem) };
        }

        static IEnumerable<Func<IParser>> Parsers { get; }

        public static bool TryParseFile(string fileName, out ISerializable result)
        {
            var parsers = Parsers.Select(p => p()).ToArray();
            var parser = parsers.FirstOrDefault(p => p.CanParse(fileName));
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
