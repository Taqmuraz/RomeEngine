using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaPropertiesCollection : IEnumerable<ColladaEntityProperty>
    {
        Dictionary<string, ColladaEntityProperty> properties;

        public ColladaPropertiesCollection(ColladaEntityProperty[] properties)
        {
            this.properties = properties.ToDictionary(p => p.Name);
        }

        public int Count => properties.Count;

        public ColladaEntityProperty this[string name] => properties[name];

        public IEnumerator<ColladaEntityProperty> GetEnumerator()
        {
            return properties.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
