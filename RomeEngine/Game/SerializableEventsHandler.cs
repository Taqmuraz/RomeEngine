﻿using RomeEngine.IO;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public abstract class SerializableEventsHandler : EventsHandler, ISerializable
    {
        public IEnumerable<SerializableField> EnumerateFields()
        {
            return this.EnumerateFieldsByReflection();
        }
    }
}
