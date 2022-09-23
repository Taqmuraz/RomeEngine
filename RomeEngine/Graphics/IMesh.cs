using System.Collections.Generic;

namespace RomeEngine
{
    public interface IMesh
    {
        IEnumerable<int> EnumerateIndices();
        ReadOnlyArray<IMeshAttributeInfo> Attributes { get; }

        void WriteVerticesToAttributeBuffer(IVertexBuffer buffer, int attributeIndex);
        IVertexBuffer CreateVerticesAttributeBuffer(int attributeIndex);
    }
}
