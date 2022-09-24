namespace RomeEngine.IO
{
    public sealed class TrianglesData
    {
        public TrianglesData(string materialName)
        {
            MaterialName = materialName;
        }
        public string Indices { get; set; }
        public string MaterialName { get; set; }
    }
}
