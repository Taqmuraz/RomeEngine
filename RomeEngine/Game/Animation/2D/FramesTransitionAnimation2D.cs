using RomeEngine.IO;
using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class FramesTransitionAnimation2D : Animation2D
    {
        public FramesTransitionAnimation2D(Animation2DFrame firstFrame, Animation2DFrame lastFrame)
        {
            FirstFrame = firstFrame;
            LastFrame = lastFrame;
        }

        public Animation2DFrame FirstFrame { get; private set; }
        public Animation2DFrame LastFrame { get; private set; }

        public override void Apply(SafeDictionary<string, Transform2D> bonesMap, float time)
        {
            if (time < LastFrame.TimeCode)
            {
                Animation2DFrame.ApplyBlended(FirstFrame, LastFrame, bonesMap, time / LastFrame.TimeCode);
            }
            else Animation2DFrame.Apply(LastFrame, bonesMap);
        }

        public override IEnumerable<SerializableField> EnumerateFields()
        {
            throw new NotImplementedException();
        }

        public override Animation2D CreateTransition(Animation2D nextAnimation, float time, float length)
        {
            throw new NotImplementedException();
        }
    }
}