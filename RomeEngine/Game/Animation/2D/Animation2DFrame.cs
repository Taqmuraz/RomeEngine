using RomeEngine.IO;
using System.Linq;
using System.Collections.Generic;
using System;

namespace RomeEngine
{
    public sealed class Animation2DFrame : ISerializable
    {
        public ReadOnlyArray<Animation2DFrameElement> FrameElements { get; private set; }
        public float TimeCode { get; private set; }

        public Animation2DFrame()
        {
        }

        public Animation2DFrame(ReadOnlyArray<Animation2DFrameElement> frameElements, float timeCode)
        {
            FrameElements = frameElements;
            TimeCode = timeCode;
        }

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(FrameElements), FrameElements, value => FrameElements = new ReadOnlyArray<Animation2DFrameElement>((Array)value), typeof(ReadOnlyArray<Animation2DFrameElement>));
            yield return new SerializableField(nameof(TimeCode), TimeCode, value => TimeCode = (float)value, typeof(float));
        }

        public static void Apply(Animation2DFrame animationFrame, SafeDictionary<string, Transform2D> bonesMap)
        {
            foreach (var element in animationFrame.FrameElements)
            {
                Animation2DFrameElement.Apply(element, bonesMap);
            }
        }

        public static void ApplyBlended(Animation2DFrame a, Animation2DFrame b, SafeDictionary<string, Transform2D> bonesMap, float blend)
        {
            var elements = a.FrameElements.Join(b.FrameElements, k => k.BoneName, k => k.BoneName, (ka, kb) => (ka, kb));
            foreach (var element in elements) Animation2DFrameElement.ApplyBlended(element.ka, element.kb, bonesMap, blend); 
        }
    }
}