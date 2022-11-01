using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public static class MeshAttributeTypeExtensions
    {
        static Dictionary<MeshAttributeType, Type> attributeTypesMap = new Dictionary<MeshAttributeType, Type>()
        {
            [MeshAttributeType.Float] = typeof(float),
            [MeshAttributeType.Int] = typeof(int),
        };

        public static Type GetElementType(this MeshAttributeType meshAttributeType)
        {
            return attributeTypesMap[meshAttributeType];
        }
    }
}
