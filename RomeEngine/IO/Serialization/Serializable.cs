using RomeEngine.IO;
using System.Collections.Generic;

namespace RomeEngine
{
    public abstract class Serializable : ISerializable
    {
        public IEnumerable<SerializableField> EnumerateFields()
        {
            return this.EnumerateFieldsByReflection();
        }
    }
}
