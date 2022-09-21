using RomeEngine.IO;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class Animation2DKey : ISerializable
    {
        public Animation2DKey()
        {
        }
        public Animation2DKey(Vector2 position, float rotation, Vector2 scale, float timeLength)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
            TimeLength = timeLength;
        }

        public Vector2 Position { get; private set; }
        public float Rotation { get; private set; }
        public Vector2 Scale { get; private set; }
        public float TimeLength { get; private set; }

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(Position), Position, value => Position = (Vector2)value, typeof(Vector2));
            yield return new SerializableField(nameof(Rotation), Rotation, value => Rotation = (float)value, typeof(float));
            yield return new SerializableField(nameof(Scale), Scale, value => Scale = (Vector2)value, typeof(Vector2));
            yield return new SerializableField(nameof(TimeLength), TimeLength, value => TimeLength = (float)value, typeof(float));
        }
    }
}