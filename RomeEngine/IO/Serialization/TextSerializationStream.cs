using System;
using System.Globalization;
using System.IO;

namespace RomeEngine.IO
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
            string line = textReader.ReadLine();
            if (string.IsNullOrEmpty(line)) return null;
            try
            {
                var row = line.SeparateString();
                return row.Length < 2 ? string.Empty : row[1];
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error while reading line : {line}", ex);
            }
        }

        public string ReadString()
        {
            return ReadValue();
        }

        public int ReadInt()
        {
            return int.TryParse(ReadValue(), out int result) ? result : 0;
        }

        public float ReadFloat()
        {
            return float.TryParse(ReadValue(), out float result) ? result : 0;
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

        public short ReadShort()
        {
            throw new NotImplementedException();
        }

        public byte ReadByte()
        {
            throw new NotImplementedException();
        }

        public void WriteShort(short value)
        {
            throw new NotImplementedException();
        }

        public void WriteByte(byte value)
        {
            throw new NotImplementedException();
        }
    }
}
