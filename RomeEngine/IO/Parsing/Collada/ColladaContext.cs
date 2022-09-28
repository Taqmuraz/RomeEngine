using System.Collections.Generic;

namespace RomeEngine.IO
{
    public sealed class ColladaContext
    {
        Dictionary<string, ColladaNode> sources = new Dictionary<string, ColladaNode>();
        string IdToKey(string id) => id.TrimStart('#');
        public ColladaNode GetSource(string id) => sources[IdToKey(id)];
        public void AddSource(string id, ColladaNode stackObject) => sources[IdToKey(id)] = stackObject;
    }
}
