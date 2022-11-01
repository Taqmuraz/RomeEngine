namespace RomeEngine
{
    public interface IGraphicsContext
    {
        Texture LoadTexture(string fileName);
        IMeshIdentifier LoadMesh(IMesh mesh);
        void UnloadMesh(IMeshIdentifier identifier);
    }
}
