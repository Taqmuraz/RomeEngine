using RomeEngine;

namespace RomeEngineGame
{
    public static class HumanAnimations
    {
        public static LineBasedAnimation2D CreateIdleAnimation(ReadOnlyArray<Transform2D> skeleton)
        {
            var rightForearm = skeleton[HumanBone.RightForearm];
            var rightArm = skeleton[HumanBone.RightArm];
            var rightForearmLine = new Animation2DLine
                (HumanBone.Names[HumanBone.RightForearm],
                new Animation2DKey(rightForearm.LocalPosition, rightForearm.LocalRotation, rightForearm.LocalScale, 1f),
                new Animation2DKey(rightForearm.LocalPosition, rightForearm.LocalRotation, rightForearm.LocalScale, 0.5f),
                new Animation2DKey(rightForearm.LocalPosition, rightForearm.LocalRotation + 90f, rightForearm.LocalScale, 0.5f),
                new Animation2DKey(rightForearm.LocalPosition, rightForearm.LocalRotation, rightForearm.LocalScale, 0.5f),
                new Animation2DKey(rightForearm.LocalPosition, rightForearm.LocalRotation + 90f, rightForearm.LocalScale, 0.5f),
                new Animation2DKey(rightForearm.LocalPosition, rightForearm.LocalRotation, rightForearm.LocalScale, 0.5f)
                );
            var rightArmLine = new Animation2DLine
                (HumanBone.Names[HumanBone.RightArm],
                new Animation2DKey(rightArm.LocalPosition, rightArm.LocalRotation, rightArm.LocalScale, 1f),
                new Animation2DKey(rightArm.LocalPosition, rightArm.LocalRotation + 45f, rightArm.LocalScale, 2f),
                new Animation2DKey(rightArm.LocalPosition, rightArm.LocalRotation + 45f, rightArm.LocalScale, 0.5f)
                );
            return new LineBasedAnimation2D(rightForearmLine, rightArmLine);
        }
    }
}
