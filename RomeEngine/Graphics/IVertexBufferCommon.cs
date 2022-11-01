using System;

namespace RomeEngine
{
    public interface IVertexBuffer
    {
        MeshAttributeType ElementType { get; }
        void Write(object value);
        Array ToArray();
    }
}
