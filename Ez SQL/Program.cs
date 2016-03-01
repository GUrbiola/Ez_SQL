using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Ez_SQL.EzConfig;

namespace Ez_SQL
{
    static class Program
    {
        public static string StartPath { get; private set; }

        /// <summary>
        /// The main entry point for the application.
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
