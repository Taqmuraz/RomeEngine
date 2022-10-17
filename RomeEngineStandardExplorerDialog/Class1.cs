using RomeEngine;
using System;
using System.IO;
using System.Windows.Forms;

namespace RomeEngineStandardExplorerDialog
{
    public static class StandardFileDialog
    {
        public static void ShowFileOpenDialog(string root, string title, Action<string> callback)
        {
            var thread = new System.Threading.Thread(() =>
            {
                var dialog = new OpenFileDialog();
                dialog.Title = title;
                dialog.InitialDirectory = Path.GetFullPath(root);
                dialog.Multiselect = false;
                dialog.FileOk += (s, e) => Routine.StartRoutineDelayed(new SingleCallRoutine(() => callback(dialog.FileName)));
                dialog.ShowDialog();
            });
            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
        }

        public static void ShowFileWriteDialog(string root, string fileName, string title, Action<string> callback)
        {
            var thread = new System.Threading.Thread(() =>
            {
                var dialog = new SaveFileDialog();
                dialog.Title = title;
                dialog.InitialDirectory = Path.GetFullPath(root);
                dialog.FileName = fileName;
                dialog.FileOk += (s, e) => Routine.StartRoutineDelayed(new SingleCallRoutine(() => callback(dialog.FileName)));
                dialog.ShowDialog();
            });
            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
        }
    }
}
