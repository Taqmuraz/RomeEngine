using OpenTK.Graphics.OpenGL;
using RomeEngine;
using System.Collections.Generic;

namespace RomeEngineOpenGL
{
    abstract class OpenGLCommonGraphics
    {
        class MeshDrawInfo
        {
            public MeshDrawInfo(IMeshIdentifier mesh, IOpenGLShaderModel shaderModel, OpenGLShader shader, (OpenGLTexture, TextureType)[] textures)
            {
                Mesh = mesh;
                ShaderModel = shaderModel;
                Shader = shader;
                Texture = textures;
            }

            public IOpenGLShaderModel ShaderModel { get; }
            public OpenGLShader Shader { get; }
            public IMeshIdentifier Mesh { get; }
            public IEnumerable<(OpenGLTexture texture, TextureType type)> Texture { get; }

            public void Deconstruct(out IMeshIdentifier mesh, out IOpenGLShaderModel shaderModel, out OpenGLShader shader, out IEnumerable<(OpenGLTexture texture, TextureType type)> textures)
            {
                mesh = Mesh;
                shaderModel = ShaderModel;
                shader = Shader;
                textures = Texture;
            }
        }

        List<MeshDrawInfo> MeshesToDraw { get; } = new List<MeshDrawInfo>();

        TextureUnit TypeToUnit(TextureType type)
        {
            switch (type)
            {
                default:
                case TextureType.Albedo: return TextureUnit.Texture0;
                case TextureType.Normalmap: return TextureUnit.Texture1;
            }
        }

        List<(OpenGLTexture, TextureType)> currentTextures = new List<(OpenGLTexture, TextureType)>();

        public void SetTexture(Texture texture, TextureType type)
        {
            if (texture != null && !(texture is OpenGLTexture)) throw new System.ArgumentException($"{nameof(texture)} must be {nameof(OpenGLTexture)} instance");
            currentTextures.Add((texture as OpenGLTexture, type));
        }

        public void DrawMesh(IMeshIdentifier meshIdentifier, OpenGLShader shader, IOpenGLShaderModel shaderModel)
        {
            MeshesToDraw.Add(new MeshDrawInfo(meshIdentifier, shaderModel, shader, currentTextures.ToArray()));
            currentTextures.Clear();
        }

        public void RenderScene()
        {
            foreach (var (mesh, shaderModel, shader, textures) in MeshesToDraw)
            {
                if (mesh is OpenGLMeshIdentifier identifier)
                {
                    foreach (var (texture, textureType) in textures)
                    {
                        GL.ActiveTexture(TypeToUnit(textureType));
                        GL.BindTexture(TextureTarget.Texture2D, texture == null ? 0 : ((OpenGLTexture)texture).TextureIndex);
                    }
                    shader.Start();
                    shaderModel.SetupShader(shader);
                    GL.BindVertexArray(identifier.VertexArrrayObjectIndex);
                    var attributes = identifier.SourceMesh.Attributes;
                    for (int i = 0; i < attributes.Length; i++)
                    {
                        GL.EnableVertexAttribArray(i);
                    }
                    GL.DrawElements(PrimitiveType.Triangles, identifier.IndicesNumber, DrawElementsType.UnsignedInt, 0);

                    shader.Stop();
                }
                else
                {
                    throw new System.ArgumentException($"Mesh identifier must be instance of ${nameof(OpenGLMeshIdentifier)}");
                }
            }
            MeshesToDraw.Clear();
        }
    }
}
