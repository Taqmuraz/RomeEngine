using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RomeEngine.IO
{
    public static class Resources
    {
        public static string ResourcesGlobalPath => Path.GetFullPath("./Resources/");

        static Dictionary<string, ISerializable> cache = new Dictionary<string, ISerializable>();

        public static T Load<T>(string file) where T : ISerializable
        {
            file = Path.Combine(ResourcesGlobalPath, file);

            if (cache.TryGetValue(file, out ISerializable value))
            {
                return (T)value;
            }
            else
            {
                var result = (T)new Serializer().DeserializeFile(file);
                cache.Add(file, result);
                return result;
            }
        }
        public static (T result, string fileName)[] LoadAll<T>(string directory) where T : ISerializable
        {
            directory = Path.Combine(ResourcesGlobalPath, directory);

            var files = Directory.GetFiles(directory);
            return files.Select(f => (new Serializer().DeserializeFile(f), Path.GetFileNameWithoutExtension(f))).Where(s => s.Item1 is T).Select(s => ((T)s.Item1, s.Item2)).ToArray();
        }
    }
}
