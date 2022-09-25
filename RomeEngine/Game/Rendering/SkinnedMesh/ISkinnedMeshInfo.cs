using System.Collections.Generic;

namespace RomeEngine
{
    public interface ISkinnedMeshInfo
    {
        Dictionary<int, Transform> GetJointsMap();
    }
}
