using RomeEngine;
using RomeEngine.IO;

namespace RomeEngineGame
{
    public sealed class HumanAnimator : Animator2D
    {
        static SafeDictionary<string, Animation2D> animationsMap;
        string lastAnimation;

        protected override Transform2D GetRoot()
        {
            return Transform.Children[0];
        }

        static HumanAnimator()
        {
            var animations = Resources.LoadAll<Animation2D>("Animations");
            animationsMap = new SafeDictionary<string, Animation2D>();
            foreach (var animation in animations) animationsMap.Add(animation.fileName, animation.result);
        }

        public void PlayAnimation(string name)
        {
            if (name != lastAnimation) PlayAnimation(animationsMap[lastAnimation = name]);
        }
        public void PlayAnimationWithTransition(string name, float transitionLength = 0.15f)
        {
            if (name != lastAnimation)
            {
                if (lastAnimation == null) PlayAnimation(animationsMap[lastAnimation = name]);
                else
                {
                    var transition = animationsMap[lastAnimation].CreateTransition(animationsMap[name], LocalTime, transitionLength);
                    lastAnimation = name;
                    PlayAnimation(transition);
                    Routine.StartRoutine(new DelayedActionRoutine(() =>
                    {
                        if (Animation == transition) PlayAnimation(animationsMap[name]);
                    }, transitionLength));
                }
            }
        }
    }
}
