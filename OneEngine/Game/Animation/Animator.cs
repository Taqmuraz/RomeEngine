namespace OneEngine
{
    public sealed class Animator : Component
    {
        [SerializeField] Animation animation;
        public Animation Animation => animation;
        SafeDictionary<string, Transform> bonesMap;
        float timeStart;

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
            if (animation != null) animation.Apply(bonesMap, Time.time - timeStart);
        }

        public void PlayAnimation(Animation animation)
        {
            this.animation = animation;
            timeStart = Time.time;
        }
    }
}