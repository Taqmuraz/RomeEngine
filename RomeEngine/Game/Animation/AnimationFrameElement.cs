using RomeEngine.IO;
using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class AnimationFrameElement : ISerializable
    {
        public string BoneName { get; private set; }

        Vector3 position;
        Vector3 rotation;
        Vector3 scale;

        public AnimationFrameElement()
        {
        }

        public AnimationFrameElement(string boneName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            BoneName = boneName;
            this.rotation = rotation;
            this.position = position;
            this.scale = scale;
        }

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(BoneName), BoneName, value => BoneName = (string)value, typeof(string));
            yield return new SerializableField(nameof(rotation), rotation, value => rotation = (Vector3)value, typeof(Vector3));
            yield return new SerializableField(nameof(position), position, value => position = (Vector3)value, typeof(Vector3));
            yield return new SerializableField(nameof(position), scale, value => scale = (Vector3)value, typeof(Vector3));
        }

        public static void ApplyBlended(AnimationFrameElement a, AnimationFrameElement b, SafeDictionary<string, ITransform> bonesMap, float blend)
        {
            var bone = bonesMap[a.BoneName];
            if (bone == null) return;
            bone.LocalPosition = Vector3.Lerp(a.position, b.position, blend);
            bone.LocalRotation = Vector3.LerpRotation(a.rotation, b.rotation, blend);
        }

        public static void Apply(AnimationFrameElement element, SafeDictionary<string, ITransform> bonesMap)
        {
            var bone = bonesMap[element.BoneName];
            if (bone == null) return;
            bone.LocalPosition = element.position;
            bone.LocalRotation = element.rotation;
        }
    }
}