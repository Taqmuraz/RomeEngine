using System.Linq;

namespace RomeEngine
{
    public sealed class MeshCollider : Collider
    {
        IColliderShape shape;
        protected override IColliderShape Shape => shape;

        protected override void UpdateShape()
        {
        }

        public void AssignMesh(IMesh mesh)
        {
            var positions = mesh.CreateVerticesFloatAttributeBuffer(mesh.PositionAttributeIndex).ToArray();
            Vector3[] vertices = new Vector3[positions.Length / 3];
            for (int i = 0; i < vertices.Length; i++)
            {
                int floatIndex = i * 3;
                vertices[i] = new Vector3(positions[floatIndex], positions[floatIndex + 1], positions[floatIndex + 2]);
            }
            int[] indices = mesh.EnumerateIndices().ToArray();
            shape = new MeshShape(vertices, indices, Transform.LocalToWorld);
        }
    }
}