using RomeEngine.IO;
using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class FramesTransitionAnimation : Animation
    {
        public FramesTransitionAnimation(AnimationFrame firstFrame, AnimationFrame lastFrame)
        {
            FirstFrame = firstFrame;
            LastFrame = lastFrame;
        }

        public AnimationFrame FirstFrame { get; private set; }
        public AnimationFrame LastFrame { get; private set; }

        public override void Apply(SafeDictionary<string, ITransform> bonesMap, float time)
        {
            if (time < LastFrame.TimeCode)
            {
                AnimationFrame.ApplyBlended(FirstFrame, LastFrame, bonesMap, time / LastFrame.TimeCode);
            }
            else AnimationFrame.Apply(LastFrame, bonesMap);
        }

        public override IEnumerable<SerializableField> EnumerateFields()
        {
            throw new NotImplementedException();
        }

        public override Animation CreateTransition(Animation nextAnimation, float time, float length)
        {
            throw new NotImplementedException();
        }
    }
}