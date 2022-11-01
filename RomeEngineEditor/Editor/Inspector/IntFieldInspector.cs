using System;

namespace RomeEngineEditor
{
    public sealed class IntFieldInspector : PrimitiveTypeFieldInspector
    {
        public override bool CanInspect(Type type)
        {
            return type == typeof(int);
        }
        protected override object ParseString(string value)
        {
            if (int.TryParse(value, out int result)) return result;
            else return 0;
        }
    }
}