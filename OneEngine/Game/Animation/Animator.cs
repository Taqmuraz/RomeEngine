namespace OneEngine
{
    public sealed class Animator : Component
    {
        Animation animation;
        float timeStart;

        [BehaviourEvent]
        void Update()
        {
            if (animation != null) animation.Apply(Time.time - timeStart);
        }

        public void PlayAnimation(Animation animation)
        {
            this.animation = animation;
            timeStart = Time.time;
        }
    }
}