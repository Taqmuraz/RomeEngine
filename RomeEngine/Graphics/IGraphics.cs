namespace RomeEngine
{
    public interface IGraphics
    {
        void SetProjectionMatrix(Matrix4x4 projection);
        void SetViewMatrix(Matrix4x4 view);
        void SetModelMatrix(Matrix4x4 model);

        void SetTexture(Texture texture, int index);
        void DrawMesh(int meshIndex);
        void DrawDynamicMesh(IMesh mesh);
    }
}
