using System;

namespace OneEngineGame
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
    }
}