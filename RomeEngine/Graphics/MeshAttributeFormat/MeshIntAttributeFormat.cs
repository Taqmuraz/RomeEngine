using System;

namespace RomeEngine
{
    public sealed class MeshIntAttributeFormat : IMeshAttributeFormat
    {
        Func<Array, int[]> readIntBuffer;
        Func<int[], Array> writeIntBuffer;

        public MeshIntAttributeFormat(Func<Array, int[]> readIntBuffer, Func<int[], Array> writeIntBuffer)
        {
            this.readIntBuffer = readIntBuffer;
            this.writeIntBuffer = writeIntBuffer;
        }
        public Array ConvertFromAttributeBuffer(Array array)
        {
            return readIntBuffer(array);
        }
        public Array ConvertToAttributeBuffer(Array array)
        {
            return writeIntBuffer((int[])array);
        }

        public MeshAttributeFormatType Type => MeshAttributeFormatType.Int;
    }
}