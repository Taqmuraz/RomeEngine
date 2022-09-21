namespace RomeEngine
{
    public struct Vertex
    {
        public Vertex(Vector3 position, Vector3 normal, Vector2 uV)
        {
            Position = position;
            Normal = normal;
            UV = uV;
        }

        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 UV { get; set; }
    }
}
