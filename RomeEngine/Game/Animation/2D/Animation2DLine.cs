using RomeEngine.IO;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public sealed class Animation2DLine : ISerializable
    {
        public Animation2DLine()
        {
        }
        public Animation2DLine(string boneName, params Animation2DKey[] keys)
        {
            BoneName = boneName;
            Keys = keys;
            TimeLength = keys.Aggregate(0f, (length, key) => length + key.TimeLength);
        }

        public string BoneName { get; private set; }
        public ReadOnlyArray<Animation2DKey> Keys { get; private set; }
        public float TimeLength { get; private set; }

        public void Apply(SafeDictionary<string, Transform2D> bonesMap, float time)
        {
            Transform2D bone = bonesMap[BoneName];
            if (bone == null) return;

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

            bone.LocalPosition = Vector2.Lerp(currentFrame.Position, nextFrame.Position, normalizedTime);
            bone.LocalRotation = Mathf.Lerp(currentFrame.Rotation, nextFrame.Rotation, normalizedTime);
            bone.LocalScale = Vector2.Lerp(currentFrame.Scale, nextFrame.Scale, normalizedTime);
        }

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(Keys), Keys, value => Keys = (Animation2DKey[])value, typeof(Animation2DKey[]));
            yield return new SerializableField(nameof(BoneName), BoneName, value => BoneName = (string)value, typeof(string));
            yield return new SerializableField(nameof(TimeLength), TimeLength, value => TimeLength = (float)value, typeof(float));
        }
    }
}