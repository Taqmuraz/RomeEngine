﻿using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public sealed class SkinnedMrshJointInfo : IJointInfo
    {
        public SkinnedMrshJointInfo(Transform transform, Matrix4x4 initialState)
        {
            Transform = transform;
            InitialState = initialState;
        }

        public Transform Transform { get; }
        public Matrix4x4 InitialState { get; }
    }
    public sealed class SkinnedMeshRenderer : MeshRenderer<SkinnedMesh>, ISkinnedMeshInfo
    {
        [SerializeField] bool dynamicDraw = true;
        [SerializeField] public SkinnedMesh SkinnedMesh { get; set; }
        protected override SkinnedMesh Mesh => SkinnedMesh;

        protected override void DrawCall(IGraphics graphics, IMeshIdentifier meshIdentifier)
        {
            if (dynamicDraw)
            {
                if (Mesh != null) graphics.DrawDynamicMesh(Mesh, this);
            }
            else graphics.DrawSkinnedMesh(meshIdentifier, this);
        }

        public Dictionary<int, IJointInfo> GetJointsMap() => Transform.TraceElement(t => t.Children).Select(t => (index: SkinnedMesh.JointNames.IndexOf(t.Name), joint: t)).Where(t => t.index != -1).ToDictionary(t => t.index, t => (IJointInfo)new SkinnedMrshJointInfo(t.joint, SkinnedMesh.JointBindings[t.index]));
    }
}
