using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OneEngine.IO
{
    public static class TypesMap
    {
        static SafeDictionary<string, Type> typesMap = new SafeDictionary<string, Type>();

        static TypesMap()
        {
            var assemblies = new List<Assembly>();
            TraceAssembly(Assembly.GetEntryAssembly(), assemblies);
            var types = assemblies.SelectMany(a => a.GetTypes());
            foreach (var type in types)
            {
                typesMap[type.FullName] = type;
                typesMap[type.Name] = type;
            }
        }

        static void TraceAssembly(Assembly root, List<Assembly> list)
        {
            if (list.Contains(root)) return;
            list.Add(root);
            foreach (var reference in root.GetReferencedAssemblies()) TraceAssembly(Assembly.Load(reference), list);
        }

        public static Type GetType(string name)
        {
            return typesMap[name];
        }
    }
}
