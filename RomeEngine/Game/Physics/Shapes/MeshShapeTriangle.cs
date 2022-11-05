using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class MeshShapeTriangle : ILocatable
    {
        public Vector3 VertexA { get; }
        public Vector3 VertexB { get; }
        public Vector3 VertexC { get; }
        public Vector3 Normal { get; }
        public Vector3 Center { get; }
        Bounds bounds;

        public MeshShapeTriangle(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC)
        {
            VertexA = vertexA;
            VertexB = vertexB;
            VertexC = vertexC;
            Center = (vertexA + vertexB + vertexC) * (1f / 3f);
            Normal = Vector3.Cross(vertexB - vertexA, vertexC - vertexA).Normalized;
            bounds = Bounds.FromPoints(EnumeratePoints());
        }
        IEnumerable<Vector3> EnumeratePoints()
        {
            yield return VertexA;
            yield return VertexB;
            yield return VertexC;
        }

        public bool IsInsideBox(Bounds box)
        {
            return bounds.IntersectsWith(box);
        }
    }
}