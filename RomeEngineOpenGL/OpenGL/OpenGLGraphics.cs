using OpenTK.Graphics.OpenGL;
using RomeEngine;
using System.Linq;

namespace RomeEngineOpenGL
{
    abstract class OpenGLCommonGraphics
    {
        protected abstract OpenGLShader ActiveShader { get; }

        TextureUnit TypeToUnit(TextureType type)
        {
            switch (type)
            {
                default:
                case TextureType.Albedo: return TextureUnit.Texture0;
                case TextureType.Normalmap: return TextureUnit.Texture1;
            }
        }

        public void SetTexture(Texture texture, TextureType type)
        {
            if (!(texture is OpenGLTexture)) throw new System.ArgumentException($"{nameof(texture)} must be {nameof(OpenGLTexture)} instance");
            GL.ActiveTexture(TypeToUnit(type));
            GL.BindTexture(TextureTarget.Texture2D, ((OpenGLTexture)texture).TextureIndex);
        }

        protected abstract void SetupShader(OpenGLShader shader);

        public void DrawMesh(IMeshIdentifier meshIdentifier)
        {
            DrawMesh(meshIdentifier, ActiveShader);
        }
        public void DrawMesh(IMeshIdentifier meshIdentifier, OpenGLShader shader)
        {
            if (meshIdentifier is OpenGLMeshIdentifier identifier)
            {
                shader.Start();
                SetupShader(shader);
                GL.BindVertexArray(identifier.VertexArrrayObjectIndex);
                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
                GL.EnableVertexAttribArray(2);

                GL.DrawElements(PrimitiveType.Triangles, identifier.VerticesNumber * 3, DrawElementsType.UnsignedInt, 0);

                ActiveShader.Stop();
            }
            else
            {
                throw new System.ArgumentException($"Mesh identifier must be instance of ${nameof(OpenGLMeshIdentifier)}");
            }
        }
    }
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
                Vector3 pos = mvp.MultiplyPoint_With_WDivision(vertex.Position);

                GL.Color4(Color32.white.WithAlpha(64));
                GL.Vertex3(pos.x, pos.y, 0f);
                GL.TexCoord2(vertex.UV.x, vertex.UV.y);
                GL.Normal3(vertex.Normal.x, vertex.Normal.y, vertex.Normal.z);
            }
            GL.End();
        }
    }
}
