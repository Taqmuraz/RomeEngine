namespace OneEngine
{
    public sealed class AnimationKey
    {
        public AnimationKey(Vector2 position, float rotation, Vector2 scale, float timeLength)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
            TimeLength = timeLength;
        }

        public Vector2 Position { get; }
        public float Rotation { get; }
        public Vector2 Scale { get; }
        public float TimeLength { get; }
    }
}