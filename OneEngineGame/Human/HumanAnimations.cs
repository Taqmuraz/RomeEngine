using OneEngine;

namespace OneEngineGame
{
    public static class HumanAnimations
    {
        public static Animation CreateIdleAnimation(ReadOnlyArray<Transform> skeleton)
        {
            var rightForearm = skeleton[HumanBone.RightForearm];
            var rightArm = skeleton[HumanBone.RightArm];
            var rightForearmLine = new AnimationLine
                (rightForearm,
                new AnimationKey(rightForearm.LocalPosition, rightForearm.LocalRotation, rightForearm.LocalScale, 1f),
                new AnimationKey(rightForearm.LocalPosition, rightForearm.LocalRotation, rightForearm.LocalScale, 0.5f),
                new AnimationKey(rightForearm.LocalPosition, rightForearm.LocalRotation + 90f, rightForearm.LocalScale, 0.5f),
                new AnimationKey(rightForearm.LocalPosition, rightForearm.LocalRotation, rightForearm.LocalScale, 0.5f),
                new AnimationKey(rightForearm.LocalPosition, rightForearm.LocalRotation + 90f, rightForearm.LocalScale, 0.5f),
                new AnimationKey(rightForearm.LocalPosition, rightForearm.LocalRotation, rightForearm.LocalScale, 0.5f)
                );
            var rightArmLine = new AnimationLine
                (rightArm,
                new AnimationKey(rightArm.LocalPosition, rightArm.LocalRotation, rightArm.LocalScale, 1f),
                new AnimationKey(rightArm.LocalPosition, rightArm.LocalRotation + 45f, rightArm.LocalScale, 2f),
                new AnimationKey(rightArm.LocalPosition, rightArm.LocalRotation + 45f, rightArm.LocalScale, 0.5f)
                );
            return new Animation(rightForearmLine, rightArmLine);
        }
    }
}
