using RomeEngine.IO;
using System.Collections.Generic;

namespace RomeEngine
{
    public abstract class Animation2D : ISerializable
    {
        public abstract void Apply(SafeDictionary<string, Transform2D> bonesMap, float time);
        public abstract IEnumerable<SerializableField> EnumerateFields();
        public abstract Animation2D CreateTransition(Animation2D nextAnimation, float time, float length);
    }
}