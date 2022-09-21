using System;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class Serializer
    {
        public static Dictionary<string, IFieldSerializer> FieldSerializers { get; }
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

        public static string EmptySerializerKey { get; } = "Empty";

        public static string GetSerializerKey(IFieldSerializer serializer)
        {
            return serializer.GetType().Name;
        }

        static Serializer()
        {
            FieldSerializers = GetFieldSerializers().ToDictionary(GetSerializerKey);
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
                    var serializer = FieldSerializers.Values.FirstOrDefault(f => f.CanSerializeType(field.Type));
                    if (serializer == null)
                    {
                        throw new InvalidOperationException($"Can't serialize field with type {field.Type.FullName}");
                    }
                    stream.WriteString(field.Name);
                    stream.WriteString(GetSerializerKey(serializer));
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
                        string serializerIndex = stream.ReadString();
                        if (serializerIndex == Serializer.EmptySerializerKey)
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
            string extension = Engine.Instance.Runtime.FileSystem.GetFileExtension(file).ToLower();
            switch (extension)
            {
                case BIN:
                    using (System.IO.Stream fs = Engine.Instance.Runtime.FileSystem.OpenFileRead(file))
                    {
                        return Deserialize(new BinarySerializationStream(fs));
                    }
                case TXT:
                    using (System.IO.TextReader tr = Engine.Instance.Runtime.FileSystem.ReadText(file))
                    {
                        return Deserialize(new TextSerializationStream(tr, null));
                    }
                default:
                    throw new InvalidOperationException($"Extension {extension} is not supported to deserialize");
            }
        }
        public void SerializeFile(ISerializable serializable, string path)
        {
            string extension = Engine.Instance.Runtime.FileSystem.GetFileExtension(path).ToLower();
            switch (extension)
            {
                case BIN:
                    using (System.IO.Stream fs = Engine.Instance.Runtime.FileSystem.OpenFileWrite(path))
                    {
                        Serialize(serializable, new BinarySerializationStream(fs));
                    }
                    break;
                case TXT:
                    using (System.IO.TextWriter sw = Engine.Instance.Runtime.FileSystem.WriteText(path))
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
