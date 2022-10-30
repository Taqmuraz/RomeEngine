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

        public override void Apply(SafeDictionary<string, ITransform> bonesMap, float time)
        {
            if (Frames == null || Frames.Length == 0) return;

            float normalizedTime = time - (int)(time / Length) * Length;

            AnimationFrame frameCurrent = null;
            AnimationFrame frameNext = null;

            if (Frames.Length == 1)
            {
                frameCurrent = frameNext = Frames[0];
            }
            else
            {
                for (int i = 1; i < Frames.Length; i++)
                {
                    if (Frames[i].TimeCode >= normalizedTime)
                    {
                        frameCurrent = Frames[i - 1];
                        frameNext = Frames[i];
                        break;
                    }
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

        public override Animation CreateTransition(Animation nextAnimation, float time, float length)
        {
            if (nextAnimation == null) return null;

            if (nextAnimation is FrameBasedAnimation frameBasedNext)
            {
                float loopTime = Mathf.Loop(time, 0f, Frames[Frames.Length - 1].TimeCode);
                return new FramesTransitionAnimation(new AnimationFrame((Frames.FirstOrDefault(f => f.TimeCode >= loopTime) ?? Frames.Last()).FrameElements, 0f), new AnimationFrame(frameBasedNext.Frames[0].FrameElements, length));
            }
            else throw new NotImplementedException();
        }
    }
}