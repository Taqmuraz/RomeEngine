using System;

namespace RomeEngineGame
{
    public interface IFieldInspector
    {
        bool CanInspect(Type type);
        void Inspect(string name, object value, Action<object> setter, Type type, EditorCanvas canvas, InspectorMenu inspectorMenu);
    }
}