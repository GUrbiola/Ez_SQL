using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Ez_SQL.EzConfig;

namespace Ez_SQL
{
    /// <summary>
    /// Application entry point. Bootstraps the Windows Forms application and launches
    /// <see cref="MainForm"/> as the main window.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Gets the directory from which the application was launched.
        /// Used by other components to locate configuration files and assets relative to the install location.
        /// </summary>
        public static string StartPath { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <summary>
        /// The main entry point for the application.
        /// Captures the startup path, enables visual styles, and starts the main form.
        /// </summary>
        [STAThread]
        static void Main()
        {
            StartPath = Application.StartupPath;


            //#region Get the configuration for the app
            //Propiedades prop = new Propiedades();
            //prop.FileName = MainForm.DataStorageDir + "\\EzConfig.cfg";
            //if (File.Exists(prop.FileName))
            //{
            //    prop.LoadData();
            //}
            //else
            //{
            //    prop.AddProperty("CheckForDangerousExecutions", "1");
            //    prop.SaveData();
            //}
            //_ApplicationConfiguration = new AppConfig();
            //_ApplicationConfiguration.CheckForDangerousExecutions = prop.GetValue("CheckForDangerousExecutions") == "1";
            //#endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            //Application.Run(new SideToSideTester());
        }
    }
}
