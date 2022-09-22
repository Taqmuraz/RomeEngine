using OpenTK.Graphics.OpenGL;
using RomeEngine;
using System.Linq;

namespace RomeEngineOpenGL
{
    class OpenGLGraphics : IGraphics
    {
        int width;
        int height;

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

        public void Setup(int width, int height)
        {
            this.width = width;
            this.height = height;
            GL.LoadIdentity();
        }

        Matrix4x4 model, view, projection;
        double[] matrix = new double[16];
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

        void SetupMatrix()
        {
            Matrix4x4 mvp = Matrix4x4.CreateViewport(width, height) * projection * view.GetInversed() * model;

            for (int i = 0; i < matrix.Length; i++) matrix[i] = mvp[i % 4, i / 4];
            GL.LoadIdentity();
            GL.LoadMatrix(matrix);
            GL.PushMatrix();
        }

        public void DrawMesh(IMeshIdentifier meshIdentifier)
        {
            if (meshIdentifier is OpenGLMeshIdentifier identifier)
            {
                SetupMatrix();
                GL.BindVertexArray(identifier.VertexArrrayObjectIndex);
                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
                GL.EnableVertexAttribArray(2);

                GL.DrawElements(BeginMode.Triangles, identifier.VerticesNumber, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                throw new System.ArgumentException($"Mesh identifier must be instance of ${nameof(OpenGLMeshIdentifier)}");
            }
        }

        public void DrawDynamicMesh(IMesh mesh)
        {
            SetupMatrix();
            GL.Begin(PrimitiveType.Triangles);
            var vertices = mesh.EnumerateVertices().ToArray();
            foreach (var index in mesh.EnumerateIndices())
            {
                var vertex = vertices[index];
                GL.Vertex3(vertex.Position.x, vertex.Position.y, vertex.Position.z);
                GL.TexCoord2(vertex.UV.x, vertex.UV.y);
                GL.Normal3(vertex.Normal.x, vertex.Normal.y, vertex.Normal.z);
            }
            GL.End();
        }
    }
}
