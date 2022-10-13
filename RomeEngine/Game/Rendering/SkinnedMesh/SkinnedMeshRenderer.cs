﻿using RomeEngine.IO;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public sealed class SkinnedMeshRenderer : MeshRenderer<SkinnedMesh>, ISkinnedMeshInfo
    {
        [SerializeField] bool dynamicDraw = false;
        Dictionary<int, IJointInfo> bindingsMap;

        [SerializeField] public SkinnedMesh SkinnedMesh { get; set; }
        protected override SkinnedMesh Mesh => SkinnedMesh;

        protected override void DrawCall(IGraphics graphics, IMeshIdentifier meshIdentifier)
        {
            if (dynamicDraw)
            {
                if (Mesh != null) graphics.DrawDynamicMesh(Mesh, this);
            }
            else if (bindingsMap != null)
            {
                graphics.DrawSkinnedMesh(meshIdentifier, this);
            }
        }

        public Dictionary<int, IJointInfo> GetJointsMap() => bindingsMap;

        [BehaviourEvent]
        void LateUpdate()
        {
            if (bindingsMap == null)
            {
                bindingsMap = Transform.TraceElement(t => t.Children)
                    .Select(t => (index: SkinnedMesh.JointNames.IndexOf(t.Name), joint: t))
                    .Where(t => t.index != -1)
                    .ToDictionary<(int index, Transform joint), int, IJointInfo>
                    (t => t.index, t => new SkinnedMeshJointInfo(t.joint, SkinnedMesh.BindMatrices[t.index].GetInversed()));
            }
        }
    }
}
