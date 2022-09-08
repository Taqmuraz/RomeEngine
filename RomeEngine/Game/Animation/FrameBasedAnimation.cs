using RomeEngine.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
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

        public override void Apply(SafeDictionary<string, Transform> bonesMap, float time)
        {
            float normalizedTime = time - (int)(time / Length) * Length;

            AnimationFrame frameCurrent = null;
            AnimationFrame frameNext = null;

            for (int i = 1; i < Frames.Length; i++)
            {
                if (Frames[i].TimeCode >= normalizedTime)
                {
                    frameCurrent = Frames[i - 1];
                    frameNext = Frames[i];
                    break;
                }
            }

            float blend = (normalizedTime - frameCurrent.TimeCode) / (frameNext.TimeCode - frameCurrent.TimeCode);

            if (blend < 0) throw new Exception();

            if (frameCurrent != null && frameNext != null) AnimationFrame.ApplyBlended(frameCurrent, frameNext, bonesMap, blend);
        }

        public override IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(Frames), Frames, value => Frames = new ReadOnlyArray<AnimationFrame>((Array)value), typeof(ReadOnlyArray<AnimationFrame>));
        }
    }
}