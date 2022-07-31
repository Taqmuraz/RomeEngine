using System;
using OneEngine;
using OneEngine.UI;

namespace OneEngineGame
{
    public abstract class EditorMenu
    {
        public Action OnClose { get; set; }
        public abstract void Draw(EditorCanvas canvas);

        public Rect Rect { get; set; } = new Rect(Vector2.zero, Screen.Size);

        protected void Close()
        {
            if (OnClose != null) OnClose();
        }
    }
}