namespace RomeEngine.IO
{
    public sealed class ColladaSemantic
    {
        public ColladaSemantic(string value, string id)
        {
            Value = value;
            Id = id;
        }

        public string Value { get; }
        public string Id { get; }
    }
}
