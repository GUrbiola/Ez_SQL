using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL.ConnectionManagement
{
    /// <summary>
    /// Stores the display name and ADO.NET connection string for a single SQL Server connection.
    /// Instances are grouped inside a <see cref="ConnectionGroup"/> and persisted to XML via <see cref="Ez_SQL.Globals"/>.
    /// </summary>
    public class ConnectionInfo
    {
        /// <summary>Initializes a new, empty <see cref="ConnectionInfo"/>.</summary>
        public ConnectionInfo()
        {
        }

        /// <summary>The user-friendly label shown in the connection sidebar.</summary>
        public string Name;

        /// <summary>The full ADO.NET connection string used to open a <see cref="System.Data.SqlClient.SqlConnection"/>.</summary>
        public string ConnectionString;
    }
}
