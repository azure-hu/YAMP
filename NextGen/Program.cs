using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace YAMP
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(ShowAssemblies);
            //Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new frmSplash());
            //AppDomain.CurrentDomain.AssemblyLoad -= ShowAssemblies;
            plEditWnd _yampPlEdit = new plEditWnd();
            MainWindow _yampMain = new MainWindow(ref _yampPlEdit);
            Application.Run(_yampMain);
        }

        /*private static void ShowAssemblies(object sender, AssemblyLoadEventArgs e)
        {
            // Store name of assembly in the queue
            frmSplash.AsmLoads.Enqueue(e.LoadedAssembly.GetName().Name);
        }*/
    }
}
