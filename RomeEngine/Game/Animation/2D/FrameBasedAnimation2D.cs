using RomeEngine.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public sealed class FrameBasedAnimation2D : Animation2D
    {
        public FrameBasedAnimation2D()
        {
        }

        public FrameBasedAnimation2D(ReadOnlyArray<Animation2DFrame> frames)
        {
            Frames = frames;
        }

        public ReadOnlyArray<Animation2DFrame> Frames { get; private set; }

        public float Length => Frames[Frames.Length - 1].TimeCode;

        public override void Apply(SafeDictionary<string, Transform2D> bonesMap, float time)
        {
            if (Frames == null || Frames.Length == 0) return;

            float normalizedTime = time - (int)(time / Length) * Length;

            Animation2DFrame frameCurrent = null;
            Animation2DFrame frameNext = null;

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

            if (frameCurrent != null && frameNext != null) Animation2DFrame.ApplyBlended(frameCurrent, frameNext, bonesMap, blend);
        }

        public override IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(Frames), Frames, value => Frames = new ReadOnlyArray<Animation2DFrame>((Array)value), typeof(ReadOnlyArray<Animation2DFrame>));
        }

        public override Animation2D CreateTransition(Animation2D nextAnimation, float time, float length)
        {
            if (nextAnimation == null) return null;

            if (nextAnimation is FrameBasedAnimation2D frameBasedNext)
            {
                float loopTime = Mathf.Loop(time, 0f, Frames[Frames.Length - 1].TimeCode);
                return new FramesTransitionAnimation2D(new Animation2DFrame((Frames.FirstOrDefault(f => f.TimeCode >= loopTime) ?? Frames.Last()).FrameElements, 0f), new Animation2DFrame(frameBasedNext.Frames[0].FrameElements, length));
            }
            else throw new NotImplementedException();
        }
    }
}