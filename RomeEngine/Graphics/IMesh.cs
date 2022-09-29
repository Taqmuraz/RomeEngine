﻿using System.Collections.Generic;

namespace RomeEngine
{
    public interface IMesh
    {
        IEnumerable<int> EnumerateIndices();
        ReadOnlyArray<IMeshAttributeInfo> Attributes { get; }

        IVertexBuffer CreateVerticesAttributeBuffer(int attributeIndex);

        int PositionAttributeIndex { get; }
        int TexcoordAttributeIndex { get; }
        int NormalAttributeIndex { get; }

        PolygonFormat PolygonFormat { get; }
    }
}
