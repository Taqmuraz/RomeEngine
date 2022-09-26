using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaTransformInfo
    {
        public ColladaTransformInfo(string name, int depth)
        {
            Name = name;
            Depth = depth;
        }
        public int Depth { get; }
        public string Name { get; }
        public string Matrix { get; set; } =
                $"1 0 0 0 " +
                $"0 1 0 0 " +
                $"0 0 1 0 " +
                $"0 0 0 1";

        public Vector3 StartOffset { get; set; }

        public float TipX { get; set; }
        public float TipY { get; set; }
        public float TipZ { get; set; }

        public Matrix4x4 ReadMatrix()
        {
            float[] floats = Matrix.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries).Select(m => m.ToFloat()).ToArray();
            return Matrix4x4.FromFloatsArray(floats);
        }

        public void UpdateChildren()
        {
            foreach (var child in Children) child.StartOffset = new Vector3(TipX, TipY, TipZ);
        }

        public List<ColladaTransformInfo> Children { get; } = new List<ColladaTransformInfo>();

        public override string ToString() => $"{Name} depth = {Depth}";
    }
}
