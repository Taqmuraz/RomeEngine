using System.Collections.Generic;

namespace RomeEngine.IO
{
    public sealed class ColladaSkeletonHierarchy : Component, ISkeleton
    {
        [SerializeField] List<SkeletonBone> bones = new List<SkeletonBone>();
        [SerializeField] public HierarchyTransform Root { get; set; }

        public void AddBone(SkeletonBone bone)
        {
            bones.Add(bone);
        }

        IEnumerable<SkeletonBone> ISkeleton.Bones => bones;

        [BehaviourEvent]
        void OnPreRender()
        {
            if (Root != null)
            {
                Root.UpdateHierarchy();
            }
        }
    }
}
