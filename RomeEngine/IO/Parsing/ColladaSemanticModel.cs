using System.Collections.Generic;

namespace RomeEngine.IO
{
    public sealed class ColladaSemanticModel
    {
        Dictionary<string, ColladaSemantic> semantics = new Dictionary<string, ColladaSemantic>();

        string IdToKey(string id)
        {
            return id.TrimStart('#');
        }

        public void AddSemantic(ColladaSemantic semantic)
        {
            if (semantic.Id == null || semantic.Value == null) return;

            string key = IdToKey(semantic.Id);
            if (key == IdToKey(semantic.Value)) return;

            if (semantics.TryGetValue(key, out ColladaSemantic exist))
            {
                semantics[key] = new ColladaSemantic(semantic.Value, exist.Id);
            }
            else
            {
                semantics.Add(key, semantic);
            }
        }
        public ColladaSemantic GetSemantic(string sourceId)
        {
            if (semantics.TryGetValue(IdToKey(sourceId), out var result)) return result;
            else throw new System.ArgumentException($"Semantic for \"{sourceId}\" is not exist");
        }
    }
}
