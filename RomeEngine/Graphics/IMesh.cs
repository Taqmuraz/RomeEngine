using System.Collections.Generic;

namespace RomeEngine
{
    public interface IMesh
    {
        IEnumerable<int> EnumerateIndices();
        ReadOnlyArray<IMeshAttributeInfo> Attributes { get; }

        void WriteVerticesToAttributeBuffer(IVertexBuffer<float> buffer, int attributeIndex);
        void WriteVerticesToAttributeBuffer(IVertexBuffer<int> buffer, int attributeIndex);

        IVertexBuffer<float> CreateVerticesFloatAttributeBuffer(int attributeIndex);
        IVertexBuffer<int> CreateVerticesIntAttributeBuffer(int attributeIndex);

        int PositionAttributeIndex { get; }
        int TexcoordAttributeIndex { get; }
        int NormalAttributeIndex { get; }
    }
}
