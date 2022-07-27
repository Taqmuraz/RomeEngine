﻿using System.Collections.Generic;

namespace OneEngine.IO
{
    public interface ISerializable
    {
        IEnumerable<SerializableField> EnumerateFields();
    }
}
