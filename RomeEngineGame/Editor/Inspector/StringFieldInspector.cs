using System;

namespace RomeEngineGame
{
    public sealed class StringFieldInspector : PrimitiveTypeFieldInspector
    {
        public override bool CanInspect(Type type)
        {
            return type == typeof(string);
        }

        protected override object ParseString(string value)
        {
            return value;
        }
        protected override string ObjectToString(object value)
        {
            return value == null ? "null" : value.ToString();
        }
    }
}