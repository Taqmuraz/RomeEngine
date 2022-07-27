using System;

namespace OneEngine.IO
{
    public interface ISerializationStream
    {
        string ReadString();
        int ReadInt();
        float ReadFloat();
        Type ReadType();

        void WriteString(string value);
        void WriteInt(int value);
        void WriteFloat(float value);
        void WriteType(Type type);

        int Position { get; set; }
        int Length { get; }
    }
}
