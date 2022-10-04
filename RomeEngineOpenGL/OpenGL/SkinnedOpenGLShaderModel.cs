using RomeEngine;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngineOpenGL
{
    class SkinnedOpenGLShaderModel : StaticOpenGLShaderModel
    {
        Dictionary<int, IJointInfo> jointsMap;
        Matrix4x4[] joints;

        public SkinnedOpenGLShaderModel(Matrix4x4 model, Matrix4x4 view, Matrix4x4 projection, ISkinnedMeshInfo skinnedMeshInfo) : base(model, view, projection)
        {
            jointsMap = skinnedMeshInfo.GetJointsMap();
            joints = new Matrix4x4[jointsMap.Count];
        }

        protected override IEnumerable<IOpenGLShaderParameter> CreateParameters()
        {
            return base.CreateParameters().Concat(new IOpenGLShaderParameter[] 
            {
                new CustomOpenGLShaderParameter(shader =>
                {
                    foreach (var pair in jointsMap)
                    {
                        joints[pair.Key] = pair.Value.Transform.LocalToWorld * pair.Value.InversedInitialState;
                    }
                    shader.SetMatrixArray("jointTransforms", joints);
                }),
            });
        }
    }
}
