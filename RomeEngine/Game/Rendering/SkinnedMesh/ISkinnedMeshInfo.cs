using System.Collections.Generic;

namespace RomeEngine
{
    public interface ISkinnedMeshInfo
    {
        Dictionary<int, IJointInfo> GetJointsMap();
    }
}
