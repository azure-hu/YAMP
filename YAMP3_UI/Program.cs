using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Azure.YAMP
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(currentDomain_AssemblyResolve);

            //AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(ShowAssemblies);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            

            //Application.Run(new frmSplash());
            //AppDomain.CurrentDomain.AssemblyLoad -= ShowAssemblies;
            PlayerUI3 _yamp = new PlayerUI3();
            try
            {
                Application.Run(_yamp);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }
            
        }

        static Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //This handler is called only when the common language runtime tries to bind to the assembly and fails.

            //Retrieve the list of referenced assemblies in an array of AssemblyName.
            Assembly MyAssembly, objExecutingAssemblies;
            string strTempAssmbPath = "";

            objExecutingAssemblies = Assembly.GetExecutingAssembly();
            AssemblyName[] arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies();

            //Loop through the array of referenced assembly names.
            foreach (AssemblyName strAssmbName in arrReferencedAssmbNames)
            {
                //Check for the assembly names that have raised the "AssemblyResolve" event.
                if (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) == args.Name.Substring(0, args.Name.IndexOf(",")))
                {
                    //Build the path of the assembly from where it has to be loaded.				
                    strTempAssmbPath = string.Format("{0}\\lib\\{1}.dll", AssemblyDirectory, args.Name.Substring(0, args.Name.IndexOf(",")));
                    break;
                }

            }
            //Load the assembly from the specified path. 					
            MyAssembly = Assembly.LoadFrom(strTempAssmbPath);

            //Return the loaded assembly.
            return MyAssembly;
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        /*private static void ShowAssemblies(object sender, AssemblyLoadEventArgs e)
        {
            // Store name of assembly in the queue
            frmSplash.AsmLoads.Enqueue(e.LoadedAssembly.GetName().Name);
        }*/
    }
}
