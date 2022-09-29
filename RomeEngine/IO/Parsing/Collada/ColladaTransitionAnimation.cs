using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaTransitionAnimation : Animation, IColladaAnimation
    {
        ColladaSingleBoneAnimation[] animations;
        float timeStart;
        float length;

        public ReadOnlyArray<ColladaSingleBoneAnimation> BoneAnimations => animations;

        public ColladaTransitionAnimation(IColladaAnimation last, IColladaAnimation next, float time, float length)
        {
            animations = last.BoneAnimations
                .Select(b => { b.GenerateFrame(time, out ColladaSingleBoneFrame frame, out _, ColladaAnimationBlendMode.Loop); return (frame, b.BoneName); })
                .Concat(next.BoneAnimations
                .Select(b => { b.GenerateFrame(time + length, out _, out ColladaSingleBoneFrame frame, ColladaAnimationBlendMode.Loop); return (frame, b.BoneName); }))
                .GroupBy(b => b.BoneName)
                .Select(g => new ColladaSingleBoneAnimation(g.Key, g.First().frame, g.Last().frame) { BlendMode = ColladaAnimationBlendMode.Clamp }).ToArray();
            timeStart = time;
        }

        public override void Apply(SafeDictionary<string, Transform> bonesMap, float time)
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
