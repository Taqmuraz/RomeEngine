using System;

namespace RomeEngine
{
    public sealed class MeshFloatAttributeFormat : IMeshAttributeFormat
    {
        Func<Array, float[]> readFloatBuffer;
        Func<float[], Array> writeFloatBuffer;

        public MeshFloatAttributeFormat(Func<Array, float[]> readFloatBuffer, Func<float[], Array> writeFloatBuffer)
        {
            this.readFloatBuffer = readFloatBuffer;
            this.writeFloatBuffer = writeFloatBuffer;
        }

        public Array ConvertFromAttributeBuffer(Array array)
        {
            return readFloatBuffer(array);
        }
        public Array ConvertToAttributeBuffer(Array array)
        {
            return writeFloatBuffer((float[])array);
        }
        public MeshAttributeFormatType Type => MeshAttributeFormatType.Float;
    }
}