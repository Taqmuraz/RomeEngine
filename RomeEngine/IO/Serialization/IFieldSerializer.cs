﻿using System;

namespace RomeEngine.IO
{
    public interface IFieldSerializer
    {
        bool CanSerializeType(Type type);
        void SerializeField(object value, ISerializationContext context);
        object DeserializeField(Type type, ISerializationContext context);
    }
}
