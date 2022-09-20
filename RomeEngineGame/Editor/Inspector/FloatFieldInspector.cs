using RomeEngine;
using System;

namespace RomeEngineGame
{
    public sealed class FloatFieldInspector : PrimitiveTypeFieldInspector
    {
        public override bool CanInspect(Type type)
        {
            return type == typeof(float);
        }
        protected override object ParseString(string value)
        {
            return value.ToFloat();
        }
    }
}