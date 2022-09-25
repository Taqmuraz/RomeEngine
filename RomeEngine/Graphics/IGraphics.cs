namespace RomeEngine
{
    public enum TextureType
    {
        Albedo,
        Normalmap
    }
    public enum CullingMode
    {
        None,
        Back,
        Front
    }
    public interface IGraphics
    {
        void Clear(Color32 color);

        void SetProjectionMatrix(Matrix4x4 projection);
        void SetViewMatrix(Matrix4x4 view);
        void SetModelMatrix(Matrix4x4 model);

        void SetCulling(CullingMode cullingMode);

        void SetTexture(Texture texture, TextureType type);
        void DrawMesh(IMeshIdentifier meshIdentifier);
        void DrawSkinnedMesh(IMeshIdentifier meshIdentifier, ISkinnedMeshInfo skinnedMeshInfo);
        void DrawDynamicMesh(IMesh mesh);
    }
}
