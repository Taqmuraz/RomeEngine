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
            OnMenuClosed();
            if (OnClose != null) OnClose();
        }

        protected virtual void OnMenuClosed() { }

        public static TMenu ShowMenu<TMenu>(EditorCanvas canvas, Action<TMenu> callback) where TMenu : EditorMenu, new()
        {
            var menu = new TMenu();
            var routine = Routine.StartRoutine(new ActionRoutine(() => menu.Draw(canvas)));
            menu.OnClose = () =>
            {
                routine.Stop();
                callback(menu);
            };
            return menu;
        }
    }
}