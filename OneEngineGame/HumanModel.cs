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
            boneBuilders[HumanBone.LeftArm] = t => BuildArm(t, true);
            boneBuilders[HumanBone.RightArm] = t => BuildArm(t, false);
            boneBuilders[HumanBone.LeftForearm] = t => BuildForearm(t, true);
            boneBuilders[HumanBone.RightForearm] = t => BuildForearm(t, false);
            boneBuilders[HumanBone.LeftLeg] = t => BuildLeg(t, true);
            boneBuilders[HumanBone.RightLeg] = t => BuildLeg(t, false);
            boneBuilders[HumanBone.LeftKnee] = t => BuildKnee(t, true);
            boneBuilders[HumanBone.RightKnee] = t => BuildKnee(t, false);

            CreateBone(HumanBone.Body);
            CreateBone(HumanBone.Head);
            CreateBone(HumanBone.LeftArm);
            CreateBone(HumanBone.RightArm);
            CreateBone(HumanBone.LeftForearm);
            CreateBone(HumanBone.RightForearm);
            CreateBone(HumanBone.LeftLeg);
            CreateBone(HumanBone.RightLeg);
            CreateBone(HumanBone.LeftKnee);
            CreateBone(HumanBone.RightKnee);
        }

        const float HEAD_WIDTH = 0.2f;
        const float HEAD_HEIGHT = 0.25f;
        const float NECK_LENGTH = 0.1f;
        const float BODY_LENGTH = 0.5f;
        const float BODY_WIDTH_TOP = 0.25f;
        const float BODY_WIDTH_BOTTOM = 0.2f;
        const float ARM_LENGTH = 0.3f;
        const float ARM_WIDTH = 0.075f;
        const float SHOULDER_WIDTH = 0.1f;
        const float SHOULDER_LENGTH = 0.15f;
        const float ELBOW_WIDTH = 0.075f;
        const float ELBOW_LENGTH = 0.1f;
        const float FOREARM_LENGTH = 0.3f;
        const float FOREARM_WIDTH = 0.075f;
        const float LEG_LENGTH = 0.35f;
        const float LEG_WIDTH = 0.1f;
        const float KNEE_LENGTH = 0.35f;
        const float KNEE_WIDTH = 0.075f;

        public Color32 Color { get; set; } = Color32.black;

        void BuildBodyBone(Transform body)
        {
            body.Transform.Parent = Transform;
            body.Transform.LocalPosition = new Vector2(0f, 1f);

            var renderer = body.GameObject.AddComponent<LineRenderer>();
            renderer.SetLines
                (
                new Line(Vector2.zero, new Vector2(0f, BODY_LENGTH * 0.5f), Color, BODY_WIDTH_BOTTOM, (BODY_WIDTH_TOP + BODY_WIDTH_BOTTOM) * 0.3f),
                new Line(new Vector2(0f, BODY_LENGTH * 0.5f), new Vector2(0f, BODY_LENGTH), Color, (BODY_WIDTH_TOP + BODY_WIDTH_BOTTOM) * 0.3f, BODY_WIDTH_TOP)
                );
        }
        void BuildHeadBone(Transform head)
        {
            head.Transform.Parent = skeleton[HumanBone.Body];
            head.LocalPosition = new Vector2(0f, BODY_LENGTH);

            var ellipse = new GameObject("Head_Ellipse").AddComponent<EllipseRenderer>();
            ellipse.Transform.Parent = head;
            ellipse.Transform.LocalScale = new Vector2(HEAD_WIDTH, HEAD_HEIGHT);
            ellipse.Transform.LocalPosition = new Vector2(0f, NECK_LENGTH * 0.5f + HEAD_HEIGHT * 0.5f);

            var neck = new GameObject("Head_Neck").AddComponent<LineRenderer>();
            neck.Transform.Parent = head;
            neck.SetLines(new Line(Vector2.zero, new Vector2(0f, NECK_LENGTH), Color));
        }
        void BuildArm(Transform arm, bool left)
        {
            var armEllipse = new GameObject("Arm_Ellipse").AddComponent<EllipseRenderer>();
            armEllipse.Transform.Parent = arm;
            armEllipse.Transform.LocalPosition = new Vector2(ARM_LENGTH * 0.5f, 0f);
            armEllipse.Transform.LocalScale = new Vector2(ARM_LENGTH, ARM_WIDTH);

            var armShoulder = new GameObject("Shoulder_Ellipse").AddComponent<EllipseRenderer>();
            armShoulder.Transform.Parent = arm;
            armShoulder.Transform.LocalScale = new Vector2(SHOULDER_LENGTH, SHOULDER_WIDTH);
            armShoulder.Transform.LocalPosition = new Vector2(SHOULDER_LENGTH * 0.5f, 0f);

            arm.Transform.LocalRotation = -100f + (left ? -10 : 10);
            arm.Transform.Parent = skeleton[HumanBone.Body];
            arm.Transform.LocalPosition = new Vector2(BODY_WIDTH_TOP * (left ? -0.5f : 0.5f), BODY_LENGTH);
        }
        void BuildForearm(Transform forearm, bool left)
        {
            var forearmEllipse = new GameObject("Forearm_Ellipse").AddComponent<EllipseRenderer>();
            forearmEllipse.Transform.Parent = forearm;
            forearmEllipse.Transform.LocalPosition = new Vector2(FOREARM_LENGTH * 0.5f, 0f);
            forearmEllipse.Transform.LocalScale = new Vector2(FOREARM_LENGTH, FOREARM_WIDTH);

            var forearmElbow = new GameObject("Elbow_Ellipse").AddComponent<EllipseRenderer>();
            forearmElbow.Transform.Parent = forearm;
            forearmElbow.Transform.LocalScale = new Vector2(ELBOW_LENGTH, ELBOW_WIDTH);
            forearmElbow.Transform.LocalPosition = new Vector2(0f, 0f);

            forearm.Transform.Parent = skeleton[left ? HumanBone.LeftArm : HumanBone.RightArm];
            forearm.LocalPosition = new Vector2(ARM_LENGTH, 0f);
            forearm.Transform.LocalRotation = 25f;
        }
        void BuildLeg(Transform leg, bool left)
        {
            var legLine = new GameObject("Leg_Line").AddComponent<LineRenderer>();
            legLine.Transform.Parent = leg;
            legLine.SetLines(new Line(Vector2.zero, new Vector2(LEG_LENGTH, 0f), Color, LEG_WIDTH, KNEE_WIDTH));

            leg.Transform.Parent = skeleton[HumanBone.Body];
            leg.Transform.LocalPosition = new Vector2((BODY_WIDTH_BOTTOM * 0.5f - LEG_WIDTH * 0.5f) * (left ? -1f : 1f), LEG_WIDTH * 0.5f);
            leg.Transform.LocalRotation = -90f + (left ? -15f : 30f);
        }
        void BuildKnee(Transform knee, bool left)
        {
            var legLine = new GameObject("Knee_Line").AddComponent<LineRenderer>();
            legLine.Transform.Parent = knee;
            legLine.SetLines(new Line(Vector2.zero, new Vector2(KNEE_LENGTH, 0f), Color, KNEE_WIDTH, KNEE_WIDTH));

            knee.Transform.Parent = skeleton[left ? HumanBone.LeftLeg : HumanBone.RightLeg];
            knee.Transform.LocalPosition = new Vector2(LEG_LENGTH, 0f);
            knee.Transform.LocalRotation = left ? -30f : -15f;
        }

        Transform CreateBone(int humanBone)
        {
            var bone = new GameObject(HumanBone.Names[humanBone]).Transform;
            boneBuilders[humanBone](bone);
            return skeleton[humanBone] = bone;
        }
    }
}
