using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public class Animator2D : Component
    {
        [SerializeField(HideInInspector = true)] Animation2D animation;
        public Animation2D Animation => animation;
        SafeDictionary<string, Transform2D> bonesMap;
        float timeStart;
        bool isStopped;

        public float LocalTime => (Time.CurrentTime - timeStart) * PlaybackSpeed;

        [SerializeField] public float PlaybackSpeed { get; set; } = 1f;

        public IEnumerable<Transform2D> Bones => bonesMap.Values;

        protected virtual Transform2D GetRoot()
        {
            return Transform;
        }

        [BehaviourEvent]
        void Start()
        {
            bonesMap = new SafeDictionary<string, Transform2D>();
            TraceBones(GetRoot(), bonesMap);
        }
        void TraceBones(Transform2D root, SafeDictionary<string, Transform2D> map)
        {
            map[root.Name] = root;
            foreach (var child in root.Children) TraceBones(child, map);
        }

        [BehaviourEvent]
        void Update()
        {
            if (animation != null && !isStopped) animation.Apply(bonesMap, LocalTime);
        }

        public void PlayAnimation(Animation2D animation)
        {
            this.animation = animation;
            isStopped = animation == null;
            timeStart = Time.CurrentTime;
        }
        public void Stop()
        {
            isStopped = true;
        }

        public void PlayAnimationFrame(Animation2D animation, float time)
        {
            if (animation != null)
            {
                isStopped = true;
                animation.Apply(bonesMap, time);
            }
        }
    }
}