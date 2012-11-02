using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL.ConnectionManagement
{
    public class ConnectionGroup
    {
        public ConnectionGroup()
        {
            Connections = new List<ConnectionInfo>();
        }
        public string Name;
        public List<ConnectionInfo> Connections;
    }
}
