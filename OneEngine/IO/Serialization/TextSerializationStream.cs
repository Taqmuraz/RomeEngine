using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OneEngine.IO
{
    public sealed class TextSerializationStream : ISerializationStream
    {
        TextReader textReader;
        TextWriter textWriter;

        static SafeDictionary<string, Type> typesMap = new SafeDictionary<string, Type>();

        static TextSerializationStream()
        {
            var assemblies = new List<Assembly>();
            TraceAssembly(Assembly.GetEntryAssembly(), assemblies);
            var types = assemblies.SelectMany(a => a.GetTypes());
            foreach (var type in types)
            {
                typesMap[type.FullName] = type;
            }
        }
        static void TraceAssembly(Assembly root, List<Assembly> list)
        {
            if (list.Contains(root)) return;
            list.Add(root);
            foreach (var reference in root.GetReferencedAssemblies()) TraceAssembly(Assembly.Load(reference), list);
        }

        public TextSerializationStream(TextReader textReader, TextWriter textWriter)
        {
            this.textReader = textReader;
            this.textWriter = textWriter;
        }

        private string ReadValue()
        {
            return textReader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
        }

        public string ReadString()
        {
            return ReadValue();
        }

        public int ReadInt()
        {
            return int.Parse(ReadValue());
        }

        public float ReadFloat()
        {
            return float.Parse(ReadValue(), CultureInfo.InvariantCulture);
        }

        public Type ReadType()
        {
            return typesMap[ReadValue()];
        }

        public void WriteString(string value)
        {
            textWriter.WriteLine($"string {value}");
        }

        public void WriteInt(int value)
        {
            textWriter.WriteLine($"int {value}");
        }

        public void WriteFloat(float value)
        {
            textWriter.WriteLine($"float {value}");
        }

        public void WriteType(Type type)
        {
            textWriter.WriteLine($"type {type.FullName}");
        }
    }
}
