using System;
using System.IO;

namespace OneEngine.IO
{
    public sealed class BinarySerializationStream : ISerializationStream
    {
        Stream stream;
        byte[] fourBytesBuffer;
        static System.Text.Encoding Encoding => System.Text.Encoding.ASCII;

        public BinarySerializationStream(Stream stream)
        {
            fourBytesBuffer = new byte[4];
            this.stream = stream;
        }

        public string ReadString()
        {
            int length = ReadInt();
            byte[] bytes = new byte[length];
            stream.Read(bytes, 0, length);
            return Encoding.GetString(bytes);
        }

        public int ReadInt()
        {
            stream.Read(fourBytesBuffer, 0, 4);
            return BitConverter.ToInt32(fourBytesBuffer, 0);
        }

        public float ReadFloat()
        {
            stream.Read(fourBytesBuffer, 0, 4);
            return BitConverter.ToSingle(fourBytesBuffer, 0);
        }

        public Type ReadType()
        {
            return TypesMap.GetType(ReadString());
        }

        public void WriteString(string value)
        {
            WriteInt(value.Length);
            var bytes = Encoding.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        public void WriteInt(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, 4);
        }

        public void WriteFloat(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, 4);
        }

        public void WriteType(Type type)
        {
            WriteString(type.FullName);
        }
    }
}
