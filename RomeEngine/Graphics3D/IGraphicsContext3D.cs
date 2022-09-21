namespace RomeEngine
{
    public interface IGraphicsContext3D
    {
        Texture LoadTexture(string fileName);
        int LoadMesh(IMesh mesh);
    }
}
