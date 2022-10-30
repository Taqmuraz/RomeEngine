using System.Collections.Generic;

namespace RomeEngine
{
    public interface ISkeleton
    {
        IEnumerable<SkeletonBone> Bones { get; }
    }
}