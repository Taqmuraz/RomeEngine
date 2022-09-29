using RomeEngine.IO;
using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public static class SerializationExtensions
    {
        public static IEnumerable<SerializableField> EnumerateFieldsByReflection(this ISerializable serializable)
        {
            var type = serializable.GetType();
            var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public;

            var properties = type.GetProperties(flags);
            foreach (var property in properties)
            {
                var attribute = Attribute.GetCustomAttribute(property, typeof(SerializeFieldAttribute)) as SerializeFieldAttribute;

                if (property.GetMethod != null && property.SetMethod != null && attribute != null)
                {
                    yield return new SerializableField(property.Name, property.GetValue(serializable), value => property.SetValue(serializable, value), property.PropertyType, attribute.HideInInspector);
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
                        yield return new SerializableField(field.Name, field.GetValue(serializable), value => field.SetValue(serializable, value), field.FieldType, attribute.HideInInspector);
                    }
                }
                type = type.BaseType;
            } while (type != typeof(object));
        }
    }
}
