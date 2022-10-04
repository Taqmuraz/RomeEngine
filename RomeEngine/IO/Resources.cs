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
            if (cache.TryGetValue(file, out ISerializable value))
            {
                return (T)value;
            }
            else
            {
                var result = LoadFromFile<T>(file);
                cache.Add(file, result);
                return result;
            }
        }
        static T LoadFromFile<T>(string file) where T : ISerializable
        {
            return (T)new Serializer().DeserializeFile(Engine.Instance.Runtime.FileSystem.CombinePath(ResourcesGlobalPath, file));
        }
        public static T LoadInstance<T>(string file) where T : ISerializable, IInstantiatable<T>
        {
            return Load<T>(file).CreateInstance();
        }
        public static (T result, string fileName)[] LoadAll<T>(string directory) where T : ISerializable
        {
            directory = Engine.Instance.Runtime.FileSystem.CombinePath(ResourcesGlobalPath, directory);

            var files = Engine.Instance.Runtime.FileSystem.GetFiles(directory);
            return files.Select(f => (new Serializer().DeserializeFile(f), Engine.Instance.Runtime.FileSystem.GetFileNameWithoutExtension(f))).Where(s => s.Item1 is T).Select(s => ((T)s.Item1, s.Item2)).ToArray();
        }
    }
}
