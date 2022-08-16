using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OneEngine.IO
{
    public sealed class Serializer
    {
        public static ReadOnlyArrayList<IFieldSerializer> FieldSerializers { get; }
        public static string BinaryFormatExtension => BIN;
        public static string TextFormatExtension => TXT;

        const string BIN = ".bin";
        const string TXT = ".txt";

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
            yield return new ArithmeticTypeFieldSerializer();
            yield return new StringFieldSerializer();
            yield return new ArrayFieldSerializer();
            yield return new ListFieldSerializer();
            yield return new DictionaryFieldSerializer();
            yield return new Vector2Serializer();
            yield return new Color32Serializer();
            yield return new BoolTypeFieldSerializer();
            yield return new ReadOnlyArrayFieldSerializer();
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
                if (obj is ISerializationHandler handler) handler.OnSerialize();
                var fieldsArray = obj.EnumerateFields().ToArray();
                stream.WriteInt(fieldsArray.Length);
                foreach (var field in fieldsArray)
                {
                    var serializer = FieldSerializers.FirstOrDefault(f => f.CanSerializeType(field.Type));
                    if (serializer == null)
                    {
                        throw new InvalidOperationException($"Can't serialize field with type {field.Type.FullName}");
                    }
                    stream.WriteString(field.Name);
                    stream.WriteInt(FieldSerializers.IndexOf(serializer));
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
                var fieldsMap = objects[i].EnumerateFields().ToDictionary(f => f.Name);
                int fieldsCount = stream.ReadInt();
                for (int f = 0; f < fieldsCount; f++)
                {
                    string fieldName = stream.ReadString();
                    if (fieldsMap.TryGetValue(fieldName, out SerializableField field))
                    {
                        int serializerIndex = stream.ReadInt();
                        if (serializerIndex == -1)
                        {
                            throw new InvalidOperationException($"Can't deserialize field with type {field.Type.FullName}");
                        }
                        var serializer = FieldSerializers[serializerIndex];
                        field.Setter(serializer.DeserializeField(field.Type, context));
                    }
                }
            }
            foreach (var obj in objects) if (obj is ISerializationHandler handler) handler.OnDeserialize();
            return objects[0];
        }

        public ISerializable DeserializeFile(string file)
        {
            string extension = Path.GetExtension(file).ToLower();
            switch (extension)
            {
                case BIN:
                    using (FileStream fs = File.OpenRead(file))
                    {
                        return Deserialize(new BinarySerializationStream(fs));
                    }
                case TXT:
                    using (StreamReader sr = new StreamReader(file))
                    {
                        return Deserialize(new TextSerializationStream(sr, null));
                    }
                default:
                    throw new InvalidOperationException($"Extension {extension} is not supported to deserialize");
            }
        }
        public void SerializeFile(ISerializable serializable, string path)
        {
            string extension = Path.GetExtension(path).ToLower();
            switch (extension)
            {
                case BIN:
                    using (FileStream fs = File.OpenWrite(path))
                    {
                        Serialize(serializable, new BinarySerializationStream(fs));
                    }
                    break;
                case TXT:
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        Serialize(serializable, new TextSerializationStream(null, sw));
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Extension {extension} is not supported to serialize");
            }
        }
    }
}
