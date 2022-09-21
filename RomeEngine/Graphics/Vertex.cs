using RomeEngine.IO;
using System.Collections.Generic;

namespace RomeEngine
{
    public class Vertex : ISerializable
    {
        public Vertex()
        {
        }
        public Vertex(Vector3 position, Vector3 normal, Vector2 uV)
        {
            Position = position;
            Normal = normal;
            UV = uV;
        }

        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 UV { get; set; }

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(Position), Position, v => Position = (Vector3)v, typeof(Vector3), false);
            yield return new SerializableField(nameof(Normal), Normal, v => Normal = (Vector3)v, typeof(Vector3), false);
            yield return new SerializableField(nameof(UV), UV, v => UV = (Vector2)v, typeof(Vector2), false);
        }
    }
}
