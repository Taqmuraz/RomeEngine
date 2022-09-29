using RomeEngine.IO;
using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class MeshAttribute : ISerializable, IMeshAttributeInfo
    {
        public MeshAttribute()
        {
        }
        public MeshAttribute(Array buffer, int elementSize, MeshAttributeFormat type, int layout)
        {
            Buffer = type.ConvertToAttributeBuffer(buffer);
            ElementSize = elementSize;
            Type = type;
            Layout = layout;
        }
        public Array Buffer { get; private set; }
        public int ElementSize { get; private set; }
        public int Layout { get; private set; }
        public MeshAttributeFormat Type { get; private set; }

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new GenericSerializableField<Array>(nameof(Buffer), Buffer, value => Buffer = value, true);
            yield return new GenericSerializableField<int>(nameof(ElementSize), ElementSize, value => ElementSize = value, true);
            yield return new GenericSerializableField<int>(nameof(Layout), Layout, value => Layout = value, true);
            yield return new GenericSerializableField<MeshAttributeFormat>(nameof(Type), Type, value => Type = value, true);
        }
    }
}
