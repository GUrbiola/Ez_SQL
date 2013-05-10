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
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
