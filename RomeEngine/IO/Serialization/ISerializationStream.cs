using System;

namespace RomeEngine.IO
{
    public interface ISerializationStream
    {
        string ReadString();
        int ReadInt();
        short ReadShort();
        byte ReadByte();
        float ReadFloat();
        Type ReadType();

        void WriteString(string value);
        void WriteInt(int value);
        void WriteShort(short value);
        void WriteByte(byte value);
        void WriteFloat(float value);
        void WriteType(Type type);
    }
    public interface ISerializationContext
    {
        ReadOnlyArrayList<ISerializable> Objects { get; }
        ISerializationStream Stream { get; }
    }
}
