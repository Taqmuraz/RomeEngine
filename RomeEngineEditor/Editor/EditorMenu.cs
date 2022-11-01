using System;
using RomeEngine;
using RomeEngine.UI;

namespace RomeEngineEditor
{
    public abstract class EditorMenu
    {
        public Action OnClose { get; set; }
        public abstract void Draw(EditorCanvas canvas);

        public Rect Rect { get; set; } = new Rect(Vector2.zero, Screen.Size);

        protected void Close()
        {
            OnMenuClosed();
            try
            {
                if (OnClose != null) OnClose();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        protected virtual void OnMenuClosed() { }

        public static TMenu ShowMenu<TMenu>(EditorCanvas canvas, Action<TMenu> callback, Rect rect) where TMenu : EditorMenu, new()
        {
            var menu = ShowMenu(canvas, callback);
            menu.Rect = rect;
            return menu;
        }

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