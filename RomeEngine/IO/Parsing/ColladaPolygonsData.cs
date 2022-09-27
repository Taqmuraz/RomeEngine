using System;
using System.Collections.Generic;

namespace RomeEngine.IO
{
    public sealed class ColladaPolygonsData
    {
        public ColladaPolygonsData(string materialName, PolygonFormat format)
        {
            MaterialName = materialName;
            PolygonFormat = format;
        }
        public PolygonFormat PolygonFormat { get; }
        public string Indices { get; set; }
        public string MaterialName { get; set; }
    }
}
