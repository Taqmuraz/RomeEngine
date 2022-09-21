namespace RomeEngine
{
    public interface IGraphicsContext
    {
        Texture LoadTexture(string fileName);
        int LoadMesh(IMesh mesh);
    }
}
