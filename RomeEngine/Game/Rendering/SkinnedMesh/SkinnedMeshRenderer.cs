﻿using RomeEngine.IO;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public sealed class SkinnedMeshRenderer : MeshRenderer<SkinnedMesh>, ISkinnedMeshInfo, ISerializationHandler
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
            else graphics.DrawSkinnedMesh(meshIdentifier, this);
        }

        public void InitializeBindings()
        {
            bindingsMap = Transform.TraceElement(t => t.Children).Select(t => (index: SkinnedMesh.JointNames.IndexOf(t.Name), joint: t)).Where(t => t.index != -1).ToDictionary(t => t.index, t => (IJointInfo)new SkinnedMeshJointInfo(t.joint, SkinnedMesh.BindMatrices[t.index]));
        }

        public Dictionary<int, IJointInfo> GetJointsMap() => bindingsMap;

        void ISerializationHandler.OnSerialize()
        {

        }

        void ISerializationHandler.OnDeserialize()
        {
            InitializeBindings();
        }
    }
}
