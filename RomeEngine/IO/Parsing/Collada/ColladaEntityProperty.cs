namespace RomeEngine.IO
{
    public sealed class ColladaEntityProperty
    {
        public ColladaEntityProperty(string name, string value)
        {
            Name = name;
            Value = value.TrimStart('#');
        }

        public string Name { get; }
        public string Value { get; }

        public int GetInt() => int.Parse(Value);
        public float GetFloat() => Value.ToFloat();

        public override string ToString() => $"{Name}={Value}";

        public static bool operator ==(ColladaEntityProperty a, ColladaEntityProperty b) => a.Value == b.Value;
        public static bool operator !=(ColladaEntityProperty a, ColladaEntityProperty b) => a.Value != b.Value;

        public override bool Equals(object obj)
        {
            return obj is ColladaEntityProperty property && this == property;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
