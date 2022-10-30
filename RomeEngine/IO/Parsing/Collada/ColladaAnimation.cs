using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaAnimation : Animation, IColladaAnimation, ISourceObject
    {
        ISerializable ISourceObject.CloneSourceReference() => this;

        [SerializeField] ColladaSingleBoneAnimation[] boneAnimations;

        public ReadOnlyArray<ColladaSingleBoneAnimation> BoneAnimations => boneAnimations;

        public ColladaAnimation()
        {
        }

        public ColladaAnimation(ColladaEntityCollection animationEntities)
        {
            boneAnimations = animationEntities.Select(a => new ColladaSingleBoneAnimation(a)).ToArray();
        }

        public override void Apply(SafeDictionary<string, ITransform> bonesMap, float time)
        {
            foreach (var boneAnimation in boneAnimations) boneAnimation.Apply(bonesMap, time);
        }

        public override IEnumerable<SerializableField> EnumerateFields()
        {
            return this.EnumerateFieldsByReflection();
        }

        public override Animation CreateTransition(Animation nextAnimation, float time, float length)
        {
            if (nextAnimation is IColladaAnimation colladaNext)
            {
                return new ColladaTransitionAnimation(this, colladaNext, time, length);
            }
            else
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
