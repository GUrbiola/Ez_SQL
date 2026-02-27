using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL.ConnectionManagement
{
    /// <summary>
    /// Represents a named group of SQL Server connections shown as a top-level node
    /// in the connection sidebar. Groups allow users to organize connections by environment,
    /// project, or any other logical category.
    /// </summary>
    public class ConnectionGroup
    {
        /// <summary>Initializes a new <see cref="ConnectionGroup"/> with an empty connections list.</summary>
        public ConnectionGroup()
        {
            Connections = new List<ConnectionInfo>();
        }

        /// <summary>The display name of this group as shown in the connection tree.</summary>
        public string Name;

        /// <summary>The ordered list of <see cref="ConnectionInfo"/> entries that belong to this group.</summary>
        public List<ConnectionInfo> Connections;
    }
}
