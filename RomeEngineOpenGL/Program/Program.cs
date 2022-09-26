using System;
using System.IO;

namespace RomeEngineOpenGL
{
    class Program
    {
        static void Main()
        {
            using (StreamWriter log = new StreamWriter("./log.txt"))
            {
                try
                {
                    var mainWindow = new MainWindow(1366, 800, log);
                    mainWindow.VSync = OpenTK.VSyncMode.Adaptive;
                    mainWindow.Run();
                }
                catch (Exception ex)
                {
                    log.WriteLine(ex);
                };
            }
        }
    }
}
