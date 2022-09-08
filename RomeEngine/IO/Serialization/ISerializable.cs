using System.Collections.Generic;

namespace RomeEngine.IO
{
    public interface ISerializable
    {
        IEnumerable<SerializableField> EnumerateFields();
    }
}
