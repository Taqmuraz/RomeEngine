using System;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaSkin : ColladaStackContainingObject<ColladaVertexBuffer>
    {
        public ColladaSkin(string sourceName)
        {
            SourceName = sourceName;
        }

        public string SourceName { get; }
        public string JointNames { get; set; }
        public string BindingPositions { get; set; }
        public string VerticeWeightNumbers { get; set; }
        public string JointWeightIndices { get; set; }
    }
}
