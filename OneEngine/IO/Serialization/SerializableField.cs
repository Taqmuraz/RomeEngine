using System;

namespace OneEngine.IO
{
    public sealed class SerializableField
    {
        public SerializableField(string name, object value, Action<object> setter, Type type)
        {
            Name = name;
            Value = value;
            Setter = setter;
            Type = type;
        }

        public string Name { get; }
        public object Value { get; }
        public Action<object> Setter { get; }
        public Type Type { get; }
    }
}
