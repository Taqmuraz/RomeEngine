﻿using System;
using OneEngine;

namespace OneEngineGame
{
    public abstract class PrimitiveTypeFieldInspector : IFieldInspector
    {
        public abstract bool CanInspect(Type type);

        public void Inspect(string name, object value, Action<object> setter, Type type, EditorCanvas canvas, InspectorMenu inspectorMenu)
        {
            inspectorMenu.AllocateField(out Rect nameRect, out Rect valueRect);
            canvas.DrawText(name, nameRect, inspectorMenu.NameTextOptions);
            if (canvas.DrawButton(ObjectToString(value), valueRect, inspectorMenu.ValueTextOptions))
            {
                OnButtonPress(name, value, setter, type, canvas);
            }
        }
        protected virtual string ObjectToString(object value)
        {
            return value.ToString();
        }
        protected virtual void OnButtonPress(string name, object value, Action<object> setter, Type type, EditorCanvas canvas)
        {
            var textInputMenu = EditorMenu.ShowMenu<StringInputMenu>(canvas, menu => setter(ParseString(menu.InputString)));
            textInputMenu.Header = name;
        }
        protected abstract object ParseString(string value);
    }
}