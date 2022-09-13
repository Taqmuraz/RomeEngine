using RomeEngine.IO;
using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public class LineBasedAnimation : Animation
    {
        public LineBasedAnimation(params AnimationLine[] lines)
        {
            this.lines = lines;
        }

        public LineBasedAnimation()
        {
        }

        AnimationLine[] lines;

        public ReadOnlyArray<AnimationLine> Lines => lines;

        public override void Apply(SafeDictionary<string, Transform> bonesMap, float time)
        {
            foreach (var line in lines) line.Apply(bonesMap, time);
        }

        public override IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(lines), lines, value => lines = (AnimationLine[])value, typeof(AnimationLine[]));
        }

        public override Animation CreateTransition(Animation nextAnimation, float time, float length)
        {
            throw new NotImplementedException();
        }
    }
}