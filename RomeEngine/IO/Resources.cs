using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public static class Resources
    {
        public static string ResourcesGlobalPath => Engine.Instance.Runtime.FileSystem.GetFullPath("./Resources/");

        static Dictionary<string, ISerializable> cache = new Dictionary<string, ISerializable>();

        public static T Load<T>(string file) where T : ISerializable
        {
            file = Engine.Instance.Runtime.FileSystem.CombinePath(ResourcesGlobalPath, file);

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
            directory = Engine.Instance.Runtime.FileSystem.CombinePath(ResourcesGlobalPath, directory);

            var files = Engine.Instance.Runtime.FileSystem.GetFiles(directory);
            return files.Select(f => (new Serializer().DeserializeFile(f), Engine.Instance.Runtime.FileSystem.GetFileNameWithoutExtension(f))).Where(s => s.Item1 is T).Select(s => ((T)s.Item1, s.Item2)).ToArray();
        }
    }
}
