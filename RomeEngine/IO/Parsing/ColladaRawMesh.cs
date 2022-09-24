using System;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaRawMesh : ColladaStackContainingObject<ColladaVertexBuffer>
    {
        string indices;
        string id;

        public ColladaRawMesh(string id)
        {
            this.id = id;
        }
        ColladaVertexBuffer CurrentBuffer => CurrentElement;

        public override string ToString() => id;

        public void WriteIndices(string value)
        {
            indices = value;
        }

        public void WriteBuffer(string value)
        {
            CurrentBuffer.Buffer = value;
        }

        public void WriteAttribute(ColladaVertexAttribute attribute)
        {
            CurrentBuffer.Attribute = attribute;
        }
        public void WriteAttributeProperty(string name, string type)
        {
            CurrentBuffer.Attribute.AddProperty(name, type);
        }

        static char[] separators = new char[] { ' ' };
        static T[] ReadBuffer<T>(string buffer, Func<string, T> parseFunc)
        {
            return buffer.Split(separators, StringSplitOptions.RemoveEmptyEntries).Select(v => parseFunc(v)).ToArray();
        }

        public IMesh BuildMesh()
        {
            var buffers = Elements.ToArray();
            float[] positionsBuffer = ReadBuffer(buffers[0].Buffer, v => v.ToFloat());
            float[] texcoordBuffer = ReadBuffer(buffers[2].Buffer, v => v.ToFloat());
            float[] normalsBuffer = ReadBuffer(buffers[1].Buffer, v => v.ToFloat());

            return null;
        }
    }
}
