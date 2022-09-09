using RomeEngine.IO;
using System.Collections.Generic;

namespace RomeEngine
{
    public abstract class Animation : ISerializable
    {
        public abstract void Apply(SafeDictionary<string, Transform> bonesMap, float time);
        public abstract IEnumerable<SerializableField> EnumerateFields();
        public abstract Animation CreateTransition(Animation nextAnimation, float length);
    }
}