using OpenTK.Graphics.OpenGL;
using RomeEngine;
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
            GL.Begin(PrimitiveType.LineLoop);
            var vertices = mesh.EnumerateVertices().ToArray();
            foreach (var index in mesh.EnumerateIndices())
            {
                var vertex = vertices[index];
                var attributes = vertex.Attributes.ToArray();
                Vector3 pos = mvp.MultiplyPoint_With_WDivision(attributes[0].ReadVector3());

                GL.Color4(Color32.white.WithAlpha(64));
                GL.Vertex3(pos.x, pos.y, 0f);
                Vector2 uv = attributes[1].ReadVector2();
                GL.TexCoord2(uv.x, uv.y);
                var normal = attributes[2].ReadVector3();
                GL.Normal3(normal.x, normal.y, normal.z);
            }
            GL.End();
        }
    }
}
