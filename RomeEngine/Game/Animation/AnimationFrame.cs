using RomeEngine.IO;
using System.Linq;
using System.Collections.Generic;
using System;

namespace RomeEngine
{
    public sealed class AnimationFrame : ISerializable
    {
        public ReadOnlyArray<AnimationFrameElement> FrameElements { get; private set; }
        public float TimeCode { get; private set; }

        public AnimationFrame()
        {
        }

        public AnimationFrame(ReadOnlyArray<AnimationFrameElement> frameElements, float timeCode)
        {
            FrameElements = frameElements;
            TimeCode = timeCode;
        }

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(FrameElements), FrameElements, value => FrameElements = new ReadOnlyArray<AnimationFrameElement>((Array)value), typeof(ReadOnlyArray<AnimationFrameElement>));
            yield return new SerializableField(nameof(TimeCode), TimeCode, value => TimeCode = (float)value, typeof(float));
        }

        public static void ApplyBlended(AnimationFrame a, AnimationFrame b, SafeDictionary<string, Transform> bonesMap, float blend)
        {
            var elements = a.FrameElements.Join(b.FrameElements, k => k.BoneName, k => k.BoneName, (ka, kb) => (ka, kb));
            foreach (var element in elements) AnimationFrameElement.ApplyBlended(element.ka, element.kb, bonesMap, blend); 
        }
    }
}