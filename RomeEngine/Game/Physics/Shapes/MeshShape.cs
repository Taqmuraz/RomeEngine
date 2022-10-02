using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class MeshShape : IColliderShape
    {
        Bounds bounds;
        Octotree<MeshShapeTriangle> trianglesTree;

        public MeshShape(Vector3[] vertices, int[] triangles, Matrix4x4 model)
        {
            Bounds bounds = Bounds.FromPoints(vertices);
            trianglesTree = new Octotree<MeshShapeTriangle>(bounds, 5, 5);
            for (int i = 2; i < triangles.Length; i+=3)
            {
                Vector3 vertex0 = model.MultiplyPoint3x4(vertices[triangles[i - 2]]);
                Vector3 vertex1 = model.MultiplyPoint3x4(vertices[triangles[i - 1]]);
                Vector3 vertex2 = model.MultiplyPoint3x4(vertices[triangles[i]]);
                var triangle = new MeshShapeTriangle(vertex0, vertex1, vertex2);
                trianglesTree.AddLocatable(triangle);
            }
        }

        public Bounds Bounds => bounds;

        public ColliderShapeType ShapeType => ColliderShapeType.Mesh;
    }
}