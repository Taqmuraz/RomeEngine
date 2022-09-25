using OpenTK.Graphics.OpenGL;
using RomeEngine;
using System;
using System.Linq;

namespace RomeEngineOpenGL
{
    class OpenGLGraphics : OpenGLCommonGraphics, IGraphics
    {
        class OpenGLDefaultShaderModel : IOpenGLShaderModel
        {
            OpenGLGraphics graphics;

            public OpenGLDefaultShaderModel(OpenGLGraphics graphics)
            {
                this.graphics = graphics;
            }

            public void SetupShader(OpenGLShader shader)
            {
                shader.SetFloat("ambienceIntencivity", GlobalLight.AmbienceIntencivity);
                shader.SetVector3("lightDirection", GlobalLight.LightDirection);
                shader.SetVector4("lightColor", GlobalLight.LightColor.ToVector4());
                shader.SetMatrix("viewMatrix", graphics.view.GetInversed());
                shader.SetMatrix("projectionMatrix", graphics.projection);
                shader.SetMatrix("transformationMatrix", graphics.model);
            }
        }
        class OpenGLSkinShaderModel : IOpenGLShaderModel
        {
            OpenGLGraphics graphics;

            public OpenGLSkinShaderModel(OpenGLGraphics graphics)
            {
                this.graphics = graphics;
            }

            public void SetupShader(OpenGLShader shader)
            {
                graphics.standardShaderModel.SetupShader(shader);

                var jointsMap = graphics.skinnedMeshInfo.GetJointsMap();
                Matrix4x4[] joints = new Matrix4x4[jointsMap.Count];
                foreach (var pair in jointsMap)
                {
                    joints[pair.Key] = pair.Value.LocalToWorld;
                }
                shader.SetMatrixArray("jointTransforms", joints);
            }
        }

        int width;
        int height;

        OpenGLShader standardShader = new OpenGLShader("Default");
        OpenGLShader skinShader = new OpenGLShader("Skin");
        IOpenGLShaderModel standardShaderModel;
        IOpenGLShaderModel skinShaderModel;
        ISkinnedMeshInfo skinnedMeshInfo;

        public OpenGLGraphics()
        {
            standardShaderModel = new OpenGLDefaultShaderModel(this);
            skinShaderModel = new OpenGLSkinShaderModel(this);
        }

        protected override OpenGLShader ActiveShader => standardShader;

        public void Clear(Color32 color)
        {
            GL.ClearColor(color);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.DepthTest);
        }

        public void SetCulling(CullingMode cullingMode)
        {
            switch (cullingMode)
            {
                case CullingMode.None:
                    GL.Disable(EnableCap.CullFace);
                    break;
                case CullingMode.Back:
                    GL.Enable(EnableCap.CullFace);
                    GL.CullFace(CullFaceMode.Back);
                    break;
                case CullingMode.Front:
                    GL.Enable(EnableCap.CullFace);
                    GL.CullFace(CullFaceMode.Front);
                    break;
            }
        }

        Matrix4x4 model, view, projection;

        public void SetProjectionMatrix(Matrix4x4 projection)
        {
            this.projection = projection;
        }

        public void SetViewMatrix(Matrix4x4 view)
        {
            this.view = view;
        }

        public void SetModelMatrix(Matrix4x4 model)
        {
            this.model = model;
        }

        public void Setup(int width, int height)
        {
            this.width = width;
            this.height = height;
            GL.LoadIdentity();
        }

        protected override void SetupShader(OpenGLShader shader)
        {
            
        }

        public void DrawDynamicMesh(IMesh mesh)
        {
            var mvp = projection * view.GetInversed() * model;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Viewport(0, 0, width, height);
            GL.Begin(PrimitiveType.Triangles);

            var attributes = mesh.Attributes;
            float[] positions = mesh.CreateVerticesFloatAttributeBuffer(mesh.PositionAttributeIndex).ToArray();
            float[] uvs = mesh.CreateVerticesFloatAttributeBuffer(mesh.TexcoordAttributeIndex).ToArray();
            float[] normals = mesh.CreateVerticesFloatAttributeBuffer(mesh.NormalAttributeIndex).ToArray();

            float[] argumentBuffer = new float[4];

            (Action glFunc, int attributeIndex, float[] buffer, Action process)[] actions =
            {
                (() => GL.TexCoord2(argumentBuffer[0], argumentBuffer[1]), mesh.TexcoordAttributeIndex, uvs, null),
                (() => GL.Normal3(argumentBuffer[0], argumentBuffer[1], argumentBuffer[2]), mesh.NormalAttributeIndex, normals, null),
                (() => GL.Vertex3(argumentBuffer[0], argumentBuffer[1], argumentBuffer[2]), mesh.PositionAttributeIndex, positions, () =>
                {
                    Vector3 pos = new Vector3(argumentBuffer[0], argumentBuffer[1], argumentBuffer[2]);
                    pos = mvp.MultiplyPoint_With_WDivision(pos);
                    argumentBuffer[0] = pos.x;
                    argumentBuffer[1] = pos.y;
                    argumentBuffer[2] = pos.z;
                }),
            };

            foreach (var index in mesh.EnumerateIndices())
            {
                foreach (var action in actions)
                {
                    int size = attributes[action.attributeIndex].Size;
                    int bufferIndex = index * size;
                    for (int i = 0; i < size; i++) argumentBuffer[i] = action.buffer[bufferIndex + i];
                    if (action.process != null) action.process();
                    action.glFunc();
                }
            }
            GL.End();
        }

        public void DrawSkinnedMesh(IMeshIdentifier meshIdentifier, ISkinnedMeshInfo skinnedMeshInfo)
        {
            this.skinnedMeshInfo = skinnedMeshInfo;
            DrawMesh(meshIdentifier, skinShader, standardShaderModel);
        }
    }
}
