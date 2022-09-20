using RomeEngine;

namespace RomeEngineGame
{
    public sealed class HumanAnimation : LineBasedAnimation
    {
        public HumanAnimation(string name, params AnimationLine[] lines) : base(lines)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
