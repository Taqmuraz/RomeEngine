using OpenTK.Graphics.OpenGL;
using RomeEngine;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace RomeEngineOpenGL
{
    class OpenGLContext : IGraphicsContext
    {
        Dictionary<IMesh, IMeshIdentifier> meshIdentifiers = new Dictionary<IMesh, IMeshIdentifier>();
        Dictionary<string, OpenGLTexture> textures = new Dictionary<string, OpenGLTexture>();

        public Texture LoadTexture(string fileName)
        {
            if (textures.TryGetValue(fileName, out OpenGLTexture texture))
            {
                return texture;
            }
            else
            {
                Bitmap bmp = new Bitmap(1, 1);
                if (File.Exists(fileName))
                {
                    bmp = (Bitmap)Image.FromFile(fileName);
                }
                return LoadTexture(bmp, fileName);
            }
        }
        public Texture LoadTexture(Bitmap bitmap, string key)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            var texture = new OpenGLTexture(width, height, id);
            textures.Add(key, texture);

            int linear = (int)TextureMinFilter.Linear;
            int repeat = (int)TextureWrapMode.Repeat;
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, repeat);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            bitmap.UnlockBits(data);

            return texture;
        }

        public IMeshIdentifier LoadMesh(IMesh mesh)
        {
            if (meshIdentifiers.TryGetValue(mesh, out IMeshIdentifier meshIdentifier))
            {
                return meshIdentifier;
            }
            else
            {
                int vaoID = CreateVAO();
                var indices = mesh.EnumerateIndices().ToArray();
                BindIndicesBuffer(indices);
                var vertices = mesh.EnumerateVertices().ToArray();

                StoreDataInAttributeList(0, 3, vertices.Select(v => v.Position).SelectMany(v => new[] { v.x, v.y, v.z }).ToArray());
                StoreDataInAttributeList(1, 2, vertices.Select(v => v.UV).SelectMany(v => new[] { v.x, v.y }).ToArray());
                StoreDataInAttributeList(2, 3, vertices.Select(v => v.Normal).SelectMany(v => new[] { v.x, v.y, v.z }).ToArray());

                UnbindVAO();

                var identifier = new OpenGLMeshIdentifier(indices.Length, vaoID);
                meshIdentifiers.Add(mesh, identifier);
                return identifier;
            }
        }
        private static int CreateVAO()
        {
            int vaoID = GL.GenVertexArray();
            GL.BindVertexArray(vaoID);
            return vaoID;
        }
        private static int CreateVBO()
        {
            int vboID = GL.GenBuffer();
            return vboID;
        }
        private static void StoreDataInAttributeList(int attributeNumber, int coordinateSize, float[] data)
        {
            int vboID = CreateVBO();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeNumber, coordinateSize, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        private static void UnbindVAO()
        {
            GL.BindVertexArray(0);
        }
        private static void BindIndicesBuffer(int[] indices)
        {
            int vboID = CreateVBO();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vboID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);
        }
    }
}
