using System.Collections.Generic;

namespace OneEngine
{
    public sealed class Animator : Component
    {
        [SerializeField(HideInInspector = true)] Animation animation;
        public Animation Animation => animation;
        SafeDictionary<string, Transform> bonesMap;
        float timeStart;
        bool isStopped;

        public IEnumerable<Transform> Bones => bonesMap.Values;

        [BehaviourEvent]
        void Start()
        {
            bonesMap = new SafeDictionary<string, Transform>();
            TraceBones(Transform, bonesMap);
        }
        void TraceBones(Transform root, SafeDictionary<string, Transform> map)
        {
            map[root.Name] = root;
            foreach (var child in root.Children) TraceBones(child, map);
        }

        [BehaviourEvent]
        void Update()
        {
            if (animation != null && !isStopped) animation.Apply(bonesMap, Time.CurrentTime - timeStart);
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