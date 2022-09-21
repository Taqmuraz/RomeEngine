using RomeEngine.IO;
using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public class LineBasedAnimation2D : Animation2D
    {
        public LineBasedAnimation2D(params Animation2DLine[] lines)
        {
            this.lines = lines;
        }

        public LineBasedAnimation2D()
        {
        }

        Animation2DLine[] lines;

        public ReadOnlyArray<Animation2DLine> Lines => lines;

        public override void Apply(SafeDictionary<string, Transform2D> bonesMap, float time)
        {
            foreach (var line in lines) line.Apply(bonesMap, time);
        }

        public override IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(lines), lines, value => lines = (Animation2DLine[])value, typeof(Animation2DLine[]));
        }

        public override Animation2D CreateTransition(Animation2D nextAnimation, float time, float length)
        {
            throw new NotImplementedException();
        }
    }
}