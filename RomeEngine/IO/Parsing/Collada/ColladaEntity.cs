using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaEntity
    {
        ColladaEntity[] children;

        public ColladaEntity(string type, string value, ColladaNodeAttribute[] properties, ColladaEntity[] children)
        {
            Type = type.ToLower();
            Value = value;
            Properties = new ColladaPropertiesCollection(properties.Select(p => new ColladaEntityProperty(p.Name, p.Value)).ToArray());
            this.children = children;
        }

        public IEnumerable<ColladaEntity> Children => children;
        public string Type { get; }
        public string Value { get; }
        public ColladaPropertiesCollection Properties { get; }

        public ColladaEntityCollection Search(string type)
        {
            return new ColladaEntityCollection(this.TraceElement(c => c.Children).Where(t => t.Type == type));
        }

        public override string ToString() => $"{Type}, {children.Length} children, {string.Join(", ", Properties.Select(p => p.ToString()))} properties";

        public ColladaEntityCollection this[string type] => new ColladaEntityCollection(children.Where(c => c.Type == type));
    }
}
