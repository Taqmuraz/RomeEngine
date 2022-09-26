using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaSkin
    {
        public ColladaSkin(string sourceName)
        {
            SourceName = sourceName;
        }

        public string SourceName { get; }
        public string JointNames { get; set; }
        public string Weights { get; set; }
        public string BindingPositions { get; set; }
        public string VerticeWeightNumbers { get; set; }
        public string JointWeightIndices { get; set; }

        public Matrix4x4[] ReadJoints(out string[] names)
        {
            var separators = new char[] { ' ' };
            names = JointNames.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            Matrix4x4[] matrices = new Matrix4x4[names.Length];
            float[] rawMatrices = BindingPositions.Split(separators, System.StringSplitOptions.RemoveEmptyEntries).Select(f => f.ToFloat()).ToArray();
            for (int i = 0; i < matrices.Length; i++) matrices[i] = Matrix4x4.FromFloatsArray(rawMatrices, i * 16);
            return matrices;
        }
    }
}
