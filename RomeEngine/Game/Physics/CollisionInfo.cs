using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class CollisionInfo
    {
        public IEnumerable<ContactInfo> Contacts { get; }
    }
}