using OneEngine.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneEngine
{
    public sealed class FrameBasedAnimation : Animation
    {
        public FrameBasedAnimation()
        {
        }

        public FrameBasedAnimation(ReadOnlyArray<AnimationFrame> frames)
        {
            Frames = frames;
        }

        public ReadOnlyArray<AnimationFrame> Frames { get; private set; }

        public float Length => Frames[Frames.Length - 1].TimeCode;

        IEnumerable<(AnimationFrame frameFirst, AnimationFrame frameSecond, float timeCode)> EnumerateLoop()
        {
            float totalTimeCode = 0f;

            while (true)
            {
                for (int i = 0; i < Frames.Length; i++)
                {
                    var nextFrame = Frames[(i + 1) % Frames.Length];
                    yield return (Frames[i], nextFrame, totalTimeCode + nextFrame.TimeCode);
                }
                totalTimeCode += Length;
            }
        }

        public override void Apply(SafeDictionary<string, Transform> bonesMap, float time)
        {
            float normalizedTime = time - (int)(time / Length) * Length;
            var framesToBlend = EnumerateLoop().First(f => f.timeCode >= normalizedTime);
            AnimationFrame.ApplyBlended(framesToBlend.frameFirst, framesToBlend.frameSecond, bonesMap, (normalizedTime - framesToBlend.frameFirst.TimeCode) / (framesToBlend.frameSecond.TimeCode - framesToBlend.frameFirst.TimeCode));
        }

        public override IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(Frames), Frames, value => Frames = new ReadOnlyArray<AnimationFrame>((Array)value), typeof(ReadOnlyArray<AnimationFrame>));
        }
    }
}