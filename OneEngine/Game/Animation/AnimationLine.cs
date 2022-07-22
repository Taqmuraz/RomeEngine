using System.Linq;

namespace OneEngine
{
    public sealed class AnimationLine
    {
        public AnimationLine(Transform transform, params AnimationKey[] keys)
        {
            Transform = transform;
            Keys = keys;
            TimeLength = keys.Aggregate(0f, (length, key) => length + key.TimeLength);
        }

        public Transform Transform { get; }
        public ReadOnlyArray<AnimationKey> Keys { get; }
        public float TimeLength { get; }

        public void Apply(float time)
        {
            int cycles = (int)(time / TimeLength);
            time -= TimeLength * cycles;

            float total = 0f;
            int keyIndex = 0;
            for (int i = 0; i < Keys.Length; i++)
            {
                float nextTotal = total + Keys[i].TimeLength;
                if (time < nextTotal)
                {
                    keyIndex = i;
                    break;
                }
                total = nextTotal;
            }
            var currentFrame = Keys[keyIndex];
            var nextFrame = Keys[(keyIndex + 1) % Keys.Length];
            float normalizedTime = (time - total) / currentFrame.TimeLength;

            Transform.LocalPosition = Vector2.Lerp(currentFrame.Position, nextFrame.Position, normalizedTime);
            Transform.LocalRotation = Mathf.Lerp(currentFrame.Rotation, nextFrame.Rotation, normalizedTime);
            Transform.LocalScale = Vector2.Lerp(currentFrame.Scale, nextFrame.Scale, normalizedTime);
        }
    }
}