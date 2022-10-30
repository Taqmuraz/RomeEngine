namespace RomeEngine.IO
{
    public sealed class ColladaSingleBoneFrame : Serializable, ISourceObject
    {
        ISerializable ISourceObject.CloneSourceReference() => this;

        [SerializeField] Vector3 localPosition;
        [SerializeField] Vector3 localRotation;
        [SerializeField] Vector3 localScale;
        [SerializeField] float timeCode;

        ColladaSingleBoneFrame()
        {
        }

        public ColladaSingleBoneFrame(float timeCode, Matrix4x4 matrix)
        {
            this.timeCode = timeCode;
            this.localPosition = (Vector3)matrix.column_3;
            this.localRotation = matrix.GetEulerRotation();
            this.localScale = matrix.GetScale();
        }

        public float TimeCode => timeCode;

        public static void Apply(ColladaSingleBoneFrame a, ColladaSingleBoneFrame b, ITransform bone, float blend)
        {
            bone.LocalPosition = Vector3.Lerp(a.localPosition, b.localPosition, blend);
            bone.LocalRotation = Vector3.LerpRotation(a.localRotation, b.localRotation, blend);
            bone.LocalScale = Vector3.Lerp(a.localScale, b.localScale, blend);
        }

        public ColladaSingleBoneFrame CopyWithTimeCode(float timeCode)
        {
            return new ColladaSingleBoneFrame()
            {
                timeCode = timeCode,
                localPosition = this.localPosition,
                localRotation = this.localRotation,
                localScale = this.localScale
            };
        }
    }
}
