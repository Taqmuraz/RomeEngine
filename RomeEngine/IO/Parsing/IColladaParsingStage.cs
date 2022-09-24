namespace RomeEngine.IO
{
    public interface IColladaParsingStage : IColladaParsingContext
    {
        void UpdateGameObject(GameObject gameObject);
    }
}
