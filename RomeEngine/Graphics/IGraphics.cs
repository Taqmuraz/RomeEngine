namespace RomeEngine
{
    public enum TextureType
    {
        Albedo,
        Normalmap
    }
    public interface IGraphics
    {
        void Clear(Color32 color);

        void SetProjectionMatrix(Matrix4x4 projection);
        void SetViewMatrix(Matrix4x4 view);
        void SetModelMatrix(Matrix4x4 model);

        void SetTexture(Texture texture, TextureType type);
        void DrawMesh(IMeshIdentifier meshIdentifier);
        void DrawDynamicMesh(IMesh mesh);
    }
}
