using RomeEngine;

namespace OneEngineGame
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
