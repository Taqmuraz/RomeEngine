namespace RomeEngine.IO
{
    public sealed class ColladaVertexBuffer
    {
        public ColladaVertexBuffer(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public ColladaVertexAttribute Attribute { get; set; }
        public string Buffer { get; set; }
    }
}
