using OneEngine;
using System;
using System.Collections.Generic;

namespace OneEngineGame
{

    public sealed class HumanModel : Component
    {
        ReadOnlyArray<HumanAnimation> animations;
        Transform[] skeleton;
        Action<Transform>[] boneBuilders;
        Color32[] colors;
        int[] queues;
        event Action<int> QueueChanged;

        public int Queue
        {
            get => queue;
            set
            {
                queue = value;
                if (QueueChanged != null) QueueChanged(value);
            }
        }
        int queue;

        [BehaviourEvent]
        void Start()
        {
            skeleton = new Transform[HumanBone.Count];
            boneBuilders = new Action<Transform>[HumanBone.Count];

            Color32 skin = new Color32(0xf5cfb0, 255);
            Color32 shirt = new Color32(0xc22828, 255);
            Color32 pants = new Color32(0x423b7b, 255);
            colors = new Color32[HumanBone.Count];
            colors[HumanBone.Body] = shirt;
            colors[HumanBone.Head] = skin;
            colors[HumanBone.LeftLeg] = pants;
            colors[HumanBone.RightLeg] = pants;
            colors[HumanBone.LeftKnee] = pants;
            colors[HumanBone.RightKnee] = pants;
            colors[HumanBone.LeftArm] = shirt;
            colors[HumanBone.RightArm] = shirt;
            colors[HumanBone.LeftForearm] = skin;
            colors[HumanBone.RightForearm] = skin;

            queues = new int[HumanBone.Count];

            queues[HumanBone.LeftArm] = 1;
            queues[HumanBone.LeftForearm] = 2;
            queues[HumanBone.RightArm] = -1;
            queues[HumanBone.RightForearm] = -2;
            queues[HumanBone.LeftLeg] = 1;
            queues[HumanBone.RightLeg] = -1;
            queues[HumanBone.LeftKnee] = 2;
            queues[HumanBone.RightKnee] = -2;

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

            Queue = 0;
        }

        const float HEAD_WIDTH = 0.2f;
        const float HEAD_HEIGHT = 0.25f;
        const float NECK_LENGTH = 0.1f;
        const float BODY_LENGTH = 0.5f;
        const float BODY_WIDTH_TOP = 0.25f;
        const float BODY_WIDTH_MID = 0.15f;
        const float BODY_WIDTH_BOTTOM = 0.15f;
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

        void BuildBodyBone(Transform body)
        {
            body.Transform.Parent = Transform;
            body.Transform.LocalPosition = new Vector2(0f, 1f);

            var renderer = body.GameObject.AddComponent<LineRenderer>();
            renderer.SetLines
                (
                new Line(new Vector2(0f, BODY_WIDTH_BOTTOM * 0.5f), new Vector2(0f, BODY_LENGTH * 0.5f), colors[HumanBone.Body], BODY_WIDTH_BOTTOM, BODY_WIDTH_MID),
                new Line(new Vector2(0f, BODY_LENGTH * 0.5f), new Vector2(0f, BODY_LENGTH - BODY_WIDTH_BOTTOM * 0.5f), colors[HumanBone.Body], BODY_WIDTH_MID, BODY_WIDTH_TOP)
                );
            renderer.SmoothEnding = true;

            QueueChanged += q => renderer.Queue = q + queues[HumanBone.Body];
        }
        void BuildHeadBone(Transform head)
        {
            head.Transform.Parent = skeleton[HumanBone.Body];
            head.LocalPosition = new Vector2(0f, BODY_LENGTH);

            var ellipse = new GameObject("Head_Ellipse").AddComponent<EllipseRenderer>();
            ellipse.Transform.Parent = head;
            ellipse.Transform.LocalScale = new Vector2(HEAD_WIDTH, HEAD_HEIGHT);
            ellipse.Transform.LocalPosition = new Vector2(0f, NECK_LENGTH * 0.5f + HEAD_HEIGHT * 0.5f);
            ellipse.Color = colors[HumanBone.Head];

            var neck = new GameObject("Head_Neck").AddComponent<LineRenderer>();
            neck.Transform.Parent = head;
            neck.SetLines(new Line(Vector2.zero, new Vector2(0f, NECK_LENGTH), colors[HumanBone.Head]));
            neck.SmoothEnding = true;

            QueueChanged += q => ellipse.Queue = neck.Queue = q + queues[HumanBone.Head];
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

            armEllipse.Color = armShoulder.Color = colors[left ? HumanBone.LeftArm : HumanBone.RightArm];

            arm.Transform.LocalRotation = -100f + (left ? -10 : 10);
            arm.Transform.Parent = skeleton[HumanBone.Body];
            arm.Transform.LocalPosition = new Vector2(BODY_WIDTH_TOP * (left ? -0.5f : 0.5f), BODY_LENGTH);

            QueueChanged += q => armEllipse.Queue = armShoulder.Queue = q + queues[left ? HumanBone.LeftArm : HumanBone.RightArm];
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

            forearmEllipse.Color = forearmElbow.Color = colors[left ? HumanBone.LeftForearm : HumanBone.RightForearm];

            forearm.Transform.Parent = skeleton[left ? HumanBone.LeftArm : HumanBone.RightArm];
            forearm.LocalPosition = new Vector2(ARM_LENGTH, 0f);
            forearm.Transform.LocalRotation = 25f;

            QueueChanged += q => forearmEllipse.Queue = forearmElbow.Queue = q + queues[left ? HumanBone.LeftForearm : HumanBone.RightForearm];
        }
        void BuildLeg(Transform leg, bool left)
        {
            var legLine = new GameObject("Leg_Line").AddComponent<LineRenderer>();
            legLine.Transform.Parent = leg;
            legLine.SetLines(new Line(Vector2.zero, new Vector2(LEG_LENGTH, 0f), colors[left ? HumanBone.LeftLeg : HumanBone.RightLeg], LEG_WIDTH, KNEE_WIDTH));
            legLine.SmoothEnding = true;

            leg.Transform.Parent = skeleton[HumanBone.Body];
            leg.Transform.LocalPosition = new Vector2((BODY_WIDTH_BOTTOM * 0.5f) * (left ? -1f : 1f), 0f);
            leg.Transform.LocalRotation = -90f + (left ? -15f : 30f);

            QueueChanged += q => legLine.Queue = q + queues[left ? HumanBone.LeftLeg : HumanBone.RightLeg];
        }
        void BuildKnee(Transform knee, bool left)
        {
            var kneeLine = new GameObject("Knee_Line").AddComponent<LineRenderer>();
            kneeLine.Transform.Parent = knee;
            kneeLine.SetLines(new Line(Vector2.zero, new Vector2(KNEE_LENGTH, 0f), colors[left ? HumanBone.LeftKnee : HumanBone.RightKnee], KNEE_WIDTH, KNEE_WIDTH));
            kneeLine.SmoothEnding = true;

            knee.Transform.Parent = skeleton[left ? HumanBone.LeftLeg : HumanBone.RightLeg];
            knee.Transform.LocalPosition = new Vector2(LEG_LENGTH, 0f);
            knee.Transform.LocalRotation = left ? -30f : -15f;

            QueueChanged += q => kneeLine.Queue = q + queues[left ? HumanBone.LeftKnee : HumanBone.RightKnee];
        }

        Transform CreateBone(int humanBone)
        {
            var bone = new GameObject(HumanBone.Names[humanBone]).Transform;
            boneBuilders[humanBone](bone);
            return skeleton[humanBone] = bone;
        }
    }
}
