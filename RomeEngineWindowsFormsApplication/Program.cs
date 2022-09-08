using System;
using System.Windows.Forms;
using System.Linq;
using OneEngineGame;
using System.IO;

namespace OneEngineWindowsFormsApplication
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (StreamWriter log = new StreamWriter("./log.txt"))
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new GameWindow(log));
                }
                catch (Exception ex)
                {
                    log.WriteLine(ex);
                };
            }
        }
    }
}
