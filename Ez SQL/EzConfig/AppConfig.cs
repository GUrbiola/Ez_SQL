using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL.EzConfig
{
    /// <summary>
    /// Holds application-level configuration options that control runtime behavior.
    /// Currently exposes a single safety flag; intended to be extended as needed.
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// Gets or sets whether the application should warn the user before executing
        /// potentially destructive SQL statements (DROP, DELETE, TRUNCATE).
        /// </summary>
        public bool CheckForDangerousExecutions { get; set; }
    }
}
