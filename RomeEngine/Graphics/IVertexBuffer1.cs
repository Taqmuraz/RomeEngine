using System;

namespace RomeEngine
{
    public interface IVertexBuffer
    {
        void Write(object value);
        Array ToArray();
    }
}
