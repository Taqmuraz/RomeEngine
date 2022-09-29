using System;
using System.Linq;
using System.Collections.Generic;
using RomeEngine.IO;

namespace RomeEngine
{
    public sealed class MeshAttributeFormat : IMeshAttributeFormat, ISerializable
    {
        int index;
        string name;

        public MeshAttributeFormat()
        {
        }
        public MeshAttributeFormat(int index, string name)
        {
            this.index = index;
            this.name = name;
        }

        static IMeshAttributeFormat[] formats;

        static MeshAttributeFormat()
        {
            formats = new IMeshAttributeFormat[]
            {
                new MeshFloatAttributeFormat(array => (float[])array, array => array), // float
                new MeshIntAttributeFormat(array => (int[])array, array => array), // int
                new MeshFloatAttributeFormat(array => array.Select<byte, float>(a => a / 255f).ToArray(), array => array.Select<float, byte>(a =>(byte)(a * 255f)).ToArray()), // ufloat8bit
                new MeshIntAttributeFormat(array => array.Select<byte, int>(a => a).ToArray(), array => array.Select<int, byte>(a => (byte)a).ToArray()), // uint8bit
                new MeshFloatAttributeFormat(array => array.Select<byte, float>(a => ((sbyte)a) / 127f).ToArray(), array => array.Select<float, byte>(a => (byte)(sbyte)(a * 127f)).ToArray()), // float8bit
                new MeshIntAttributeFormat(array => array.Select<byte, int>(a => (sbyte)a).ToArray(), array => array.Select<int, byte>(a => (byte)a).ToArray()), // int8bit
                new MeshFloatAttributeFormat(array => array.Select<short, float>(a => Math.Sign(a) * ((a & 255) + ((a >> 8) & 127) / 127f)).ToArray(), array => array.Select<float, short>(a => (short)(Math.Sign(a) * (short)((((short)a) & 255) | (short)((Math.Abs(a - (int)a)) * 127f) << 8))).ToArray()), // float16bit
                new MeshIntAttributeFormat(array => array.Select<short, int>(a => a).ToArray(), array => array.Select<int, short>(a => (short)a).ToArray()) // int16bit
            };
        }

        public static MeshAttributeFormat Float { get; } = new MeshAttributeFormat(0, "float");
        public static MeshAttributeFormat Int { get; } = new MeshAttributeFormat(1, "int");
        public static MeshAttributeFormat UFloat8Bit { get; } = new MeshAttributeFormat(2, "ufloat_8");
        public static MeshAttributeFormat UInt8Bit { get; } = new MeshAttributeFormat(3, "uint_8");
        public static MeshAttributeFormat Float8Bit { get; } = new MeshAttributeFormat(4, "float_8");
        public static MeshAttributeFormat Int8Bit { get; } = new MeshAttributeFormat(5, "int_8");
        public static MeshAttributeFormat Float16Bit { get; } = new MeshAttributeFormat(6, "float_16");
        public static MeshAttributeFormat Int16Bit { get; } = new MeshAttributeFormat(7, "int_16");

        public override string ToString() => name;

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new GenericSerializableField<int>(nameof(index), index, value => index = value, true);
            yield return new GenericSerializableField<string>(nameof(name), name, value => name = value, true);
        }

        public Array ConvertToAttributeBuffer(Array array) => formats[index].ConvertToAttributeBuffer(array);
        public Array ConvertFromAttributeBuffer(Array array) => formats[index].ConvertFromAttributeBuffer(array);
        public MeshAttributeFormatType Type => formats[index].Type;
    }
}