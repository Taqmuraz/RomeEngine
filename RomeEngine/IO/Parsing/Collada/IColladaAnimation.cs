namespace RomeEngine.IO
{
    public interface IColladaAnimation
    {
        ReadOnlyArray<ColladaSingleBoneAnimation> BoneAnimations { get; }
    }
}