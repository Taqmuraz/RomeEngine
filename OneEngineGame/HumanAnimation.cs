using OneEngine;

namespace OneEngineGame
{
    public sealed class HumanAnimation : Animation
    {
        public HumanAnimation(string name, params AnimationLine[] lines) : base(lines)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
