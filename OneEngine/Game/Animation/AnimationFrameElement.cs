using OneEngine.IO;
using System.Collections.Generic;

namespace OneEngine
{
    public sealed class AnimationFrameElement : ISerializable
    {
        public string BoneName { get; private set; }
        float rotation;

        public AnimationFrameElement()
        {
        }

        public AnimationFrameElement(string boneName, float rotation)
        {
            BoneName = boneName;
            this.rotation = rotation;
        }

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(BoneName), BoneName, value => BoneName = (string)value, typeof(string));
            yield return new SerializableField(nameof(rotation), rotation, value => rotation = (float)value, typeof(float));
        }

        public static void ApplyBlended(AnimationFrameElement a, AnimationFrameElement b, SafeDictionary<string, Transform> bonesMap, float blend)
        {
            bonesMap[a.BoneName].LocalRotation = Mathf.Lerp(a.rotation, b.rotation, blend);
        }
    }
}