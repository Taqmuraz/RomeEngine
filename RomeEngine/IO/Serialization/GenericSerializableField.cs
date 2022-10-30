using System;

namespace RomeEngine.IO
{
    public class GenericSerializableField<TField> : SerializableField
    {
        public GenericSerializableField(string name, TField value, Action<TField> setter, bool hideInInspector = false)
            : base(name, value, v => setter((TField)v), typeof(TField))
        {
        }
    }
}
