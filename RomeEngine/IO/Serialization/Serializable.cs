using RomeEngine.IO;
using System.Collections.Generic;

namespace RomeEngine
{
    public abstract class Serializable<TSerializable> : ISerializable where TSerializable : new()
    {
        public IEnumerable<SerializableField> EnumerateFields()
        {
            return this.EnumerateFieldsByReflection();
        }
    }
}
