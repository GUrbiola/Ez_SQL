using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL.Models
{
    /// <summary>
    /// Represents the saved state of a single query tab session,
    /// pairing a connection string with the SQL text the user was working on.
    /// Used for persisting and restoring open tabs between application runs.
    /// </summary>
    public class Session
    {
        /// <summary>Gets or sets the ADO.NET connection string for this session's SQL Server connection.</summary>
        public string Connection { get; set; }

        /// <summary>Gets or sets the SQL query text that was active when the session was saved.</summary>
        public string Query { get; set; }
    }

}
