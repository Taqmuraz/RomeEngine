using System.Collections.Generic;

namespace RomeEngine.IO
{
    public sealed class ColladaSkeletonHierarchy : Component, ISkeleton
    {
        [SerializeField] List<SkeletonBone> bones = new List<SkeletonBone>();

        public void AddBone(SkeletonBone bone)
        {
            bones.Add(bone);
        }

        IEnumerable<SkeletonBone> ISkeleton.Bones => bones;
    }
}
