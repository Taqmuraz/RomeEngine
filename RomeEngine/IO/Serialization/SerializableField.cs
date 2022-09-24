using System;

namespace RomeEngine.IO
{
    public class SerializableField
    {
        public SerializableField(string name, object value, Action<object> setter, Type type, bool hideInInspector = false)
        {
            Name = name;
            Value = value;
            Setter = setter;
            Type = type;
            HideInInspector = hideInInspector;
        }

        public string Name { get; }
        public object Value { get; }
        public Action<object> Setter { get; }
        public Type Type { get; }
        public bool HideInInspector { get; }
    }
}
