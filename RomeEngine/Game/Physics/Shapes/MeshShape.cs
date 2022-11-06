using System;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public sealed class MeshShape : IPhysicalShape
    {
        Bounds bounds;
        Octotree<MeshShapeTriangle> trianglesTree;

        public MeshShape(Vector3[] vertices, int[] triangles, Matrix4x4 model)
        {
            Vector3[] world = vertices.Select(v => model.MultiplyPoint3x4(v)).ToArray();
            bounds = Bounds.FromPoints(world);
            trianglesTree = new Octotree<MeshShapeTriangle>(bounds, 5, 5);
            for (int i = 2; i < triangles.Length; i+=3)
            {
                Vector3 vertex0 = world[triangles[i - 2]];
                Vector3 vertex1 = world[triangles[i - 1]];
                Vector3 vertex2 = world[triangles[i]];
                var triangle = new MeshShapeTriangle(vertex0, vertex1, vertex2);
                trianglesTree.AddLocatable(triangle);
            }
        }

        public void CheckTriangles(Bounds area, Action<IEnumerable<MeshShapeTriangle>> checkAction)
        {
            trianglesTree.VisitTree(new CustomTreeAcceptor<MeshShapeTriangle>(checkAction, box => box.IntersectsWith(area)));
        }

        public Bounds Bounds => bounds;

        public PhysicalShapeType ShapeType => PhysicalShapeType.Mesh;
    }
}