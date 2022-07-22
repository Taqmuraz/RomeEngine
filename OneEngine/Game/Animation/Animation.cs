using System;

namespace OneEngine
{
    public class Animation
    {
        public Animation(params AnimationLine[] lines)
        {
            this.lines = lines;
        }

        AnimationLine[] lines;

        internal void Apply(float time)
        {
            foreach (var line in lines) line.Apply(time);
        }
    }
}