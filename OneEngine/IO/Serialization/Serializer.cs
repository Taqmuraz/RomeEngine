using System;
using System.Collections.Generic;
using System.Linq;

namespace OneEngine.IO
{
    public sealed class Serializer
    {
        public static ReadOnlyArrayList<IFieldSerializer> FieldSerializers { get; }

        class SerializationContext : ISerializationContext
        {
            public SerializationContext(ReadOnlyArrayList<ISerializable> objects, ISerializationStream stream)
            {
                Objects = objects;
                Stream = stream;
            }

            public ReadOnlyArrayList<ISerializable> Objects { get; }
            public ISerializationStream Stream { get; }
        }

        static Serializer()
        {
            FieldSerializers = GetFieldSerializers().ToList();
        }

        static IEnumerable<IFieldSerializer> GetFieldSerializers()
        {
            yield return new ObjectReferenceFieldSerializer();
            yield return new PrimitiveTypeFieldSerializer();
            yield return new StringFieldSerializer();
            yield return new ArrayFieldSerializer();
            yield return new ListFieldSerializer();
            yield return new DictionaryFieldSerializer();
            yield return new Vector2Serializer();
            yield return new Color32Serializer();
        }

        public void Serialize(ISerializable serializable, ISerializationStream stream)
        {
            var objects = new List<ISerializable>();
            TraceObjects(serializable, objects);

            var context = new SerializationContext(objects, stream);

            stream.WriteInt(objects.Count);
            foreach (var obj in objects)
            {
                stream.WriteType(obj.GetType());
            }
            foreach (var obj in objects)
            {
                foreach (var field in obj.EnumerateFields())
                {
                    var serializer = FieldSerializers.FirstOrDefault(f => f.CanSerializeType(field.Type));
                    if (serializer == null)
                    {
                        throw new InvalidOperationException($"Can't serialize field with type {field.Type.FullName}");
                    }
                    serializer.SerializeField(field.Value, context);
                }
            }
        }

        void TraceObjects(ISerializable root, List<ISerializable> objects)
        {
            if (objects.Contains(root))
            {
                return;
            }
            objects.Add(root);
            var fields = root.EnumerateFields();
            foreach (var field in fields.Where(field => field.Value is ISerializable)) TraceObjects((ISerializable)field.Value, objects);
            foreach (var element in fields.Where(c => c.Value is IEnumerable<ISerializable>).SelectMany(c => (IEnumerable<ISerializable>)c.Value)) TraceObjects(element, objects);
        }

        public ISerializable Deserialize(ISerializationStream stream)
        {
            var objects = new List<ISerializable>();
            int length = stream.ReadInt();
            for (int i = 0; i < length; i++)
            {
                var type = stream.ReadType();
                var constructor = type.GetConstructors().FirstOrDefault(c => c.GetParameters().Length == 0);
                if (constructor == null) throw new InvalidOperationException($"Type {type.FullName} doesn't contain zero-argument constructor");
                objects.Add((ISerializable)constructor.Invoke(new object[0]));
            }
            var context = new SerializationContext(objects, stream);
            for (int i = 0; i < length; i++)
            {
                foreach (var field in objects[i].EnumerateFields())
                {
                    var serializer = FieldSerializers.FirstOrDefault(f => f.CanSerializeType(field.Type));
                    if (serializer == null)
                    {
                        throw new InvalidOperationException($"Can't deserialize field with type {field.Type.FullName}");
                    }
                    field.Setter(serializer.DeserializeField(field.Type, context));
                }
            }
            return objects[0];
        }
    }
}
