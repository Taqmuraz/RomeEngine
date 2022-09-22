using OpenTK.Graphics.OpenGL;
using RomeEngine;

namespace RomeEngineOpenGL
{
    abstract class Style2D
    {
        int depth;

        public void Setup()
        {
            depth = InitialDepth;
        }
        protected abstract int InitialDepth { get; }
        protected float Depth => -depth * 0.0001f;
        protected void NextDepth() => depth++;

        protected void MatrixVertex(Vector2 vertex, ref Matrix3x3 matrix)
        {
            vertex = matrix.MultiplyPoint(vertex);
            GL.Vertex3(vertex.x, vertex.y, Depth);
        }
    }
}
