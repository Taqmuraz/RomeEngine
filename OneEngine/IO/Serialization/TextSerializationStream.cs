using System;
using System.Globalization;
using System.IO;

namespace OneEngine.IO
{
    public sealed class TextSerializationStream : ISerializationStream
    {
        TextReader textReader;
        TextWriter textWriter;

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
            return float.Parse(ReadValue());
        }

        public Type ReadType()
        {
            return TypesMap.GetType(ReadValue());
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
