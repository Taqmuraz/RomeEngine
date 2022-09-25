namespace RomeEngine.IO
{
    public sealed class ColladaController : ColladaStackContainingObject<ColladaSkin>
    {
        public ColladaController(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
    }
}
