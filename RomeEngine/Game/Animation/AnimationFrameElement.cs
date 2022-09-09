using RomeEngine.IO;
using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class AnimationFrameElement : ISerializable
    {
        public string BoneName { get; private set; }
        float rotation;
        Vector2 position;
        bool flipX;
        bool flipY;

        public AnimationFrameElement()
        {
        }

        public AnimationFrameElement(string boneName, float rotation, Vector2 position)
        {
            BoneName = boneName;
            this.rotation = rotation;
            this.position = position;
        }

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(BoneName), BoneName, value => BoneName = (string)value, typeof(string));
            yield return new SerializableField(nameof(rotation), rotation, value => rotation = (float)value, typeof(float));
            yield return new SerializableField(nameof(position), position, value => position = (Vector2)value, typeof(Vector2));
        }

        public static void ApplyBlended(AnimationFrameElement a, AnimationFrameElement b, SafeDictionary<string, Transform> bonesMap, float blend)
        {
            var bone = bonesMap[a.BoneName];
            if (bone == null) return;
            bone.LocalPosition = Vector2.Lerp(a.position, b.position, blend);
            bone.LocalRotation = Mathf.LerpAngle(a.rotation, b.rotation, blend);
            bone.FlipX = blend < 0.5f ? a.flipX : b.flipX;
            bone.FlipY = blend < 0.5f ? a.flipY : b.flipY;
        }

        public static void Apply(AnimationFrameElement element, SafeDictionary<string, Transform> bonesMap)
        {
            var bone = bonesMap[element.BoneName];
            if (bone == null) return;
            bone.LocalPosition = element.position;
            bone.LocalRotation = element.rotation;
            bone.FlipX = element.flipX;
            bone.FlipY = element.flipY;
        }
    }
}