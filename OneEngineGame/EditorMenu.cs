using System;
using OneEngine.UI;

namespace OneEngineGame
{
    public abstract class EditorMenu
    {
        public Action OnClose { get; set; }
        public abstract void Draw(Canvas canvas);

        protected void Close()
        {
            if (OnClose != null) OnClose();
        }
    }
}