using RomeEngine.IO;
using System.Collections.Generic;

namespace RomeEngine
{
    public abstract class Material : ISerializable
    {
        public abstract IEnumerable<SerializableField> EnumerateFields();

        public abstract void PrepareDraw(IGraphics graphics);
        public abstract void VisitContext(IGraphicsContext context);
    }
}
