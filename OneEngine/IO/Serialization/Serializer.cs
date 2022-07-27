using System;
using System.Collections.Generic;
using System.Linq;

namespace OneEngine.IO
{
    public abstract class Serializer
    {
        public static ReadOnlyArrayList<IFieldSerializer> FieldSerializers { get; }

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
        }

        protected void Serialize(ISerializable serializable, ISerializationStream stream)
        {
            var objects = new Dictionary<ISerializable, int>();
            TraceObjects(serializable, objects);

            foreach (var obj in objects)
            {

            }
        }

        void TraceObjects(ISerializable root, Dictionary<ISerializable, int> objects)
        {
            if (objects.ContainsKey(root))
            {
                return;
            }
            objects.Add(root, objects.Count);
            var fields = root.EnumerateFields().Where(field => field.Value is ISerializable);
            foreach (var field in fields) TraceObjects((ISerializable)field.Value, objects);
        }

        protected void Deserialize(ISerializable serializable, ISerializationStream stream, Func<Type, ISerializable> constructor)
        {

        }
    }
}
