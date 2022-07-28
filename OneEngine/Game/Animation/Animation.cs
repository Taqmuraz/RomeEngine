using OneEngine.IO;
using System;
using System.Collections.Generic;

namespace OneEngine
{
    public class Animation : ISerializable
    {
        public Animation(params AnimationLine[] lines)
        {
            this.lines = lines;
        }

        public Animation()
        {
        }

        AnimationLine[] lines;

        internal void Apply(SafeDictionary<string, Transform> bonesMap, float time)
        {
            foreach (var line in lines) line.Apply(bonesMap, time);
        }

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(lines), lines, value => lines = (AnimationLine[])value, typeof(AnimationLine[]));
        }
    }
}