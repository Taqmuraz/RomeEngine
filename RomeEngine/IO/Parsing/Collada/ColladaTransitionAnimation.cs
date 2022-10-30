using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaTransitionAnimation : Animation, IColladaAnimation
    {
        ColladaSingleBoneAnimation[] animations;

        public ReadOnlyArray<ColladaSingleBoneAnimation> BoneAnimations => animations;

        public ColladaTransitionAnimation(IColladaAnimation last, IColladaAnimation next, float time, float length)
        {
            animations = last.BoneAnimations.Join(next.BoneAnimations, b => b.BoneName, b => b.BoneName, (a, b) => new ColladaSingleBoneAnimation(a.BoneName, a.GenerateFrame(time).last.CopyWithTimeCode(0f), b.First.CopyWithTimeCode(length)) { BlendMode = ColladaAnimationBlendMode.Clamp }).ToArray();
        }

        public override void Apply(SafeDictionary<string, ITransform> bonesMap, float time)
        {
            for (int i = 0; i < animations.Length; i++)
            {
                animations[i].Apply(bonesMap, time);
            }
        }

        public override IEnumerable<SerializableField> EnumerateFields()
        {
            throw new System.NotImplementedException();
        }

        public override Animation CreateTransition(Animation nextAnimation, float time, float length)
        {
            if (nextAnimation is IColladaAnimation)
            {
                return new ColladaTransitionAnimation(this, (IColladaAnimation)nextAnimation, time, length);
            }
            else
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
