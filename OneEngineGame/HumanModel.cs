using OneEngine;
using System;

namespace OneEngineGame
{

    public sealed class HumanModel : Component
    {
        ReadOnlyArray<HumanAnimation> animations;
        Transform[] skeleton;
        Action<Transform>[] boneBuilders;

        [BehaviourEvent]
        void Start()
        {
            skeleton = new Transform[HumanBone.Count];
            boneBuilders = new Action<Transform>[HumanBone.Count];

            boneBuilders[HumanBone.Body] = BuildBodyBone;
            boneBuilders[HumanBone.Head] = BuildHeadBone;

            CreateBone(HumanBone.Body);
            CreateBone(HumanBone.Head);
        }

        const float HEAD_RADIUS = 0.15f;
        const float NECK_LENGTH = 0.1f;
        const float BODY_LENGTH = 0.5f;
        const float BODY_WIDTH_TOP = 0.3f;
        const float BODY_WIDTH_BOTTOM = 0.2f;
        const float ARM_LENGTH = 0.3f;
        const float FOREARM_LENGTH = 0.3f;
        const float LEG_LENGTH = 0.3f;
        const float KNEE_LENGTH = 0.3f;

        public Color32 Color { get; set; } = Color32.black;

        void BuildBodyBone(Transform body)
        {
            body.Transform.Parent = Transform;
            body.Transform.LocalPosition = new Vector2(0f, 1f);

            var renderer = body.GameObject.AddComponent<LineRenderer>();
            renderer.SetLines(new Line(Vector2.zero, new Vector2(0f, BODY_LENGTH), Color, BODY_WIDTH_BOTTOM, BODY_WIDTH_TOP));
        }
        void BuildHeadBone(Transform head)
        {
            head.Transform.Parent = skeleton[HumanBone.Body];
            head.LocalPosition = new Vector2(0f, BODY_LENGTH);

            var ellipse = new GameObject("Head_Ellipse").AddComponent<EllipseRenderer>();
            ellipse.Transform.Parent = head;
            ellipse.Transform.LocalScale = new Vector2(HEAD_RADIUS * 2f, HEAD_RADIUS * 2f);
            ellipse.Transform.LocalPosition = new Vector2(0f, NECK_LENGTH + HEAD_RADIUS);

            var neck = new GameObject("Head_Neck").AddComponent<LineRenderer>();
            neck.Transform.Parent = head;
            neck.SetLines(new Line(Vector2.zero, new Vector2(0f, NECK_LENGTH), Color));
        }

        Transform CreateBone(int humanBone)
        {
            var bone = new GameObject(HumanBone.Names[humanBone]).Transform;
            boneBuilders[humanBone](bone);
            return skeleton[humanBone] = bone;
        }
    }
}
