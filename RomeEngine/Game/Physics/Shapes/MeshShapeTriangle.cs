using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class MeshShapeTriangle : ILocatable
    {
        public Vector3 VertexA { get; }
        public Vector3 VertexB { get; }
        public Vector3 VertexC { get; }
        Bounds bounds;

        public MeshShapeTriangle(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC)
        {
            VertexA = vertexA;
            VertexB = vertexB;
            VertexC = vertexC;
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