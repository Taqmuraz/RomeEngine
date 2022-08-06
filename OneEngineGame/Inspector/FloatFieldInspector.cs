using System;

namespace OneEngineGame
{
    public sealed class FloatFieldInspector : PrimitiveTypeFieldInspector
    {
        public override bool CanInspect(Type type)
        {
            return type == typeof(float);
        }
        protected override object ParseString(string value)
        {
            if (float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float result)) return result;
            else return 0f;
        }
    }
}