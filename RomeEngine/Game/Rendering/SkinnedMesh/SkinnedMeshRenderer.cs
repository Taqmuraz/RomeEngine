using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public sealed class SkinnedMeshRenderer : MeshRenderer<SkinnedMesh>, ISkinnedMeshInfo
    {
        [SerializeField]
        public SkinnedMesh SkinnedMesh
        {
            get => skinnedMesh;
            set
            {
                skinnedMesh = value;
                if (value != null) SearchBones();
            }
        }
        SkinnedMesh skinnedMesh;
        protected override SkinnedMesh Mesh => SkinnedMesh;

        Dictionary<int, Transform> bonesMap = new Dictionary<int, Transform>();

        void SearchBones()
        {
            bonesMap = Transform.TraceElement(t => t.Children).ToDictionary(t => skinnedMesh.JointNames.IndexOf(t.Name));
        }

        protected override void DrawCall(IGraphics graphics, IMeshIdentifier meshIdentifier)
        {
            graphics.DrawSkinnedMesh(meshIdentifier, this);
        }

        public Dictionary<int, Transform> GetJointsMap() => bonesMap;
    }
}
