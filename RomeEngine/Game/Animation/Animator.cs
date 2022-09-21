using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public class Animator : Component
    {
        [SerializeField(HideInInspector = true)] Animation animation;
        public Animation Animation => animation;
        SafeDictionary<string, Transform> bonesMap;
        float timeStart;
        bool isStopped;

        public float LocalTime => (Time.CurrentTime - timeStart) * PlaybackSpeed;

        [SerializeField] public float PlaybackSpeed { get; set; } = 1f;

        public IEnumerable<Transform> Bones => bonesMap.Values;

        protected virtual Transform GetRoot()
        {
            return Transform;
        }

        [BehaviourEvent]
        void Start()
        {
            bonesMap = new SafeDictionary<string, Transform>();
            TraceBones(GetRoot(), bonesMap);
        }
        void TraceBones(Transform root, SafeDictionary<string, Transform> map)
        {
            map[root.Name] = root;
            foreach (var child in root.Children) TraceBones(child, map);
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