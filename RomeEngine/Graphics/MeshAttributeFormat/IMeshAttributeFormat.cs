using System;

namespace RomeEngine
{
    public interface IMeshAttributeFormat
    {
        Array ConvertFromAttributeBuffer(Array array);
        Array ConvertToAttributeBuffer(Array array);
        MeshAttributeFormatType Type { get; }
    }
}