using RomeEngine.IO;
using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public static class SerializationExtensions
    {
        public static System.Reflection.BindingFlags AnyMemberFlags => System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public;

        public static IEnumerable<SerializableField> EnumerateFieldsByReflection(this ISerializable serializable)
        {
            var type = serializable.GetType();
            var flags = AnyMemberFlags;

            var properties = type.GetProperties(flags);
            foreach (var property in properties)
            {
                var attribute = Attribute.GetCustomAttribute(property, typeof(SerializeFieldAttribute));

                if (property.GetMethod != null && property.SetMethod != null && attribute != null)
                {
                    yield return new SerializableField(property.Name, property.GetValue(serializable), value => property.SetValue(serializable, value), property.PropertyType);
                }
            }

            do
            {
                var fields = type.GetFields(flags);
                foreach (var field in fields)
                {
                    var attribute = Attribute.GetCustomAttribute(field, typeof(SerializeFieldAttribute));

                    if (attribute != null)
                    {
                        yield return new SerializableField(field.Name, field.GetValue(serializable), value => field.SetValue(serializable, value), field.FieldType);
                    }
                }
                type = type.BaseType;
            } while (type != typeof(object));
        }
    }
}
