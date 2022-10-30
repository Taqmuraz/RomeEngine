using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaSingleBoneAnimation : Serializable, ISourceObject
    {
        ISerializable ISourceObject.CloneSourceReference() => this;

        [SerializeField] ColladaSingleBoneFrame[] frames;
        [SerializeField] string boneName;
        [SerializeField] public ColladaAnimationBlendMode BlendMode { get; set; } = ColladaAnimationBlendMode.Loop;

        ColladaSingleBoneAnimation()
        {
        }

        public ColladaSingleBoneAnimation(string boneName, ColladaSingleBoneFrame first, ColladaSingleBoneFrame last)
        {
            this.boneName = boneName;
            frames = new[] { first, last };
        }
        public ColladaSingleBoneAnimation(ColladaEntity animationEntity)
        {
            boneName = animationEntity.Properties["name"].Value;
            float[] timecodes = animationEntity["input"]["float_array"].Single().Value.SeparateString().Select(f => f.ToFloat()).ToArray();
            float[] matrices = animationEntity["output"]["float_array"].Single().Value.SeparateString().Select(f => f.ToFloat()).ToArray();

            frames = new ColladaSingleBoneFrame[timecodes.Length];

            for (int i = 0; i < timecodes.Length; i++)
            {
                frames[i] = new ColladaSingleBoneFrame(timecodes[i], Matrix4x4.FromFloatsArray(matrices, i * 16).GetTransponed());
            }
        }

        public ColladaSingleBoneFrame First => frames[0];
        public ColladaSingleBoneFrame Last => frames[frames.Length - 1];

        public void GenerateFrame(float time, out ColladaSingleBoneFrame first, out ColladaSingleBoneFrame last, ColladaAnimationBlendMode blendMode)
        {
            first = First;
            last = Last;
            time = first.TimeCode + (time - first.TimeCode);
            switch (blendMode)
            {
                case ColladaAnimationBlendMode.Loop:
                    time = Mathf.Loop(time, first.TimeCode, last.TimeCode);
                    break;
                case ColladaAnimationBlendMode.Clamp:
                    time = Mathf.Clamp(time, First.TimeCode, Last.TimeCode);
                    break;
            }
            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i].TimeCode <= time) first = frames[i];
                if (frames[i].TimeCode > time)
                {
                    last = frames[i];
                    break;
                }
            }
        }
        public (ColladaSingleBoneFrame first, ColladaSingleBoneFrame last) GenerateFrame(float time)
        {
            GenerateFrame(time, out var first, out var last, BlendMode);
            return (first, last);
        }

        public string BoneName => boneName;

        public void Apply(SafeDictionary<string, ITransform> bonesMap, float time)
        {
            GenerateFrame(time, out var firstFrame, out var lastFrame, BlendMode);
            float blend = (Mathf.Loop(time, First.TimeCode, Last.TimeCode) - firstFrame.TimeCode) / (lastFrame.TimeCode - firstFrame.TimeCode);
            ColladaSingleBoneFrame.Apply(firstFrame, lastFrame, bonesMap[boneName], blend);
        }
    }
}
