using OpenTK.Graphics.OpenGL;
using RomeEngine;
using System;
using System.Linq;

namespace RomeEngineOpenGL
{
    class OpenGLGraphics : OpenGLCommonGraphics, IGraphics
    {
        int width;
        int height;

        OpenGLShader standardShader = new OpenGLShader("Default");

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
            shader.SetFloat("ambienceIntencivity", GlobalLight.AmbienceIntencivity);
            shader.SetVector3("lightDirection", GlobalLight.LightDirection);
            shader.SetVector4("lightColor", GlobalLight.LightColor.ToVector4());
            shader.SetMatrix("viewMatrix", view.GetInversed());
            shader.SetMatrix("projectionMatrix", projection);
            shader.SetMatrix("transformationMatrix", model);
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
    }
}
