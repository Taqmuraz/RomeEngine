using OpenTK.Graphics.OpenGL;
using RomeEngine;

namespace RomeEngineOpenGL
{
    abstract class OpenGLCommonGraphics
    {
        protected abstract OpenGLShader ActiveShader { get; }
        protected abstract IOpenGLShaderModel StandardShaderModel { get; }

        class MeshDrawInfo
        {
            public IOpenGLShaderModel ShaderModel { get; }
            public OpenGLShader Shader { get; }
            public IMeshIdentifier Mesh { get; }
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
            if (texture != null && !(texture is OpenGLTexture)) throw new System.ArgumentException($"{nameof(texture)} must be {nameof(OpenGLTexture)} instance");
            GL.ActiveTexture(TypeToUnit(type));
            GL.BindTexture(TextureTarget.Texture2D, texture == null ? 0 : ((OpenGLTexture)texture).TextureIndex);
        }

        public void DrawMesh(IMeshIdentifier meshIdentifier)
        {
            DrawMesh(meshIdentifier, ActiveShader, StandardShaderModel);
        }
        public void DrawMesh(IMeshIdentifier meshIdentifier, OpenGLShader shader, IOpenGLShaderModel shaderModel)
        {
            if (meshIdentifier is OpenGLMeshIdentifier identifier)
            {
                shader.Start();
                shaderModel.SetupShader(shader);
                GL.BindVertexArray(identifier.VertexArrrayObjectIndex);
                var attributes = identifier.SourceMesh.Attributes;
                for (int i = 0; i < attributes.Length; i++)
                {
                    GL.EnableVertexAttribArray(i);
                }

                GL.DrawElements(PrimitiveType.Triangles, identifier.IndicesNumber, DrawElementsType.UnsignedInt, 0);

                ActiveShader.Stop();
            }
            else
            {
                throw new System.ArgumentException($"Mesh identifier must be instance of ${nameof(OpenGLMeshIdentifier)}");
            }
        }
    }
}
