namespace RomeEngine.IO
{
    public interface IColladaBuilder
    {
        void BuildGameObject(GameObject gameObject, ColladaEntity rootEntity, ColladaParsingInfo info);
    }
}
