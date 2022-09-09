using RomeEngine.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public abstract class SerializableEventsHandler : EventsHandler, ISerializable
    {
        public IEnumerable<SerializableField> EnumerateFields()
        {
            var type = GetType();
            var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public;

            var properties = type.GetProperties(flags);
            foreach (var property in properties)
            {
                var attribute = Attribute.GetCustomAttribute(property, typeof(SerializeFieldAttribute)) as SerializeFieldAttribute;

                if (property.GetMethod != null && property.SetMethod != null && attribute != null)
                {
                    yield return new SerializableField(property.Name, property.GetValue(this), value => property.SetValue(this, value), property.PropertyType, attribute.HideInInspector);
                }
            }

            do
            {
                var fields = type.GetFields(flags);
                foreach (var field in fields)
                {
                    var attribute = Attribute.GetCustomAttribute(field, typeof(SerializeFieldAttribute)) as SerializeFieldAttribute;

                    if (attribute != null)
                    {
                        yield return new SerializableField(field.Name, field.GetValue(this), value => field.SetValue(this, value), field.FieldType, attribute.HideInInspector);
                    }
                }
                type = type.BaseType;
            } while (type != typeof(object));
        }
    }
}
