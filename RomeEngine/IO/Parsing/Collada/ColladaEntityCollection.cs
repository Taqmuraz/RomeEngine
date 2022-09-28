using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaEntityCollection : IEnumerable<ColladaEntity>
    {
        IEnumerable<ColladaEntity> source;

        public ColladaEntityCollection(IEnumerable<ColladaEntity> source)
        {
            this.source = source;
        }

        public IEnumerator<ColladaEntity> GetEnumerator() => source.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => source.GetEnumerator();

        public ColladaEntityCollection this[string type] => new ColladaEntityCollection(source.SelectMany(s => s[type]));
    }
}
