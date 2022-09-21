using RomeEngine;

namespace RomeEngineGame
{
    public sealed class HumanAnimation : LineBasedAnimation2D
    {
        public HumanAnimation(string name, params Animation2DLine[] lines) : base(lines)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
