using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public class Animator : Component
    {
        [SerializeField] Animation animation;
        public Animation Animation => animation;
        SafeDictionary<string, ITransform> bonesMap;
        float timeStart;
        bool isStopped;
        ISkeleton skeleton;

        public float LocalTime => (Time.CurrentTime - timeStart) * PlaybackSpeed;

        [SerializeField] public float PlaybackSpeed { get; set; } = 1f;

        public IEnumerable<SkeletonBone> Bones => skeleton.Bones;

        protected virtual ISkeleton CreateSkeleton()
        {
            return GameObject.GetComponent<ISkeleton>();
        }

        [BehaviourEvent]
        void Start()
        {
            skeleton = CreateSkeleton();
            bonesMap = skeleton.Bones.ToDictionary(b => b.Name, b => b.Transform);
        }
        [BehaviourEvent]
        void Update()
        {
            if (animation != null && !isStopped) animation.Apply(bonesMap, LocalTime);
        }

        public void PlayAnimation(Animation animation)
        {
            this.animation = animation;
            isStopped = animation == null;
            timeStart = Time.CurrentTime;
        }
        public void Stop()
        {
            isStopped = true;
        }

        public void PlayAnimationFrame(Animation animation, float time)
        {
            if (animation != null)
            {
                isStopped = true;
                animation.Apply(bonesMap, time);
            }
        }
    }
}