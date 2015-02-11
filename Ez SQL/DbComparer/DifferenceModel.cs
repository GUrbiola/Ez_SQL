using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez_SQL.DataBaseObjects;

namespace Ez_SQL.DbComparer
{
    public enum DifferenceType
    {
        Add,
        Update,
        Delete,
        None
    }

    public class DifferenceModel
    {
        public string SourceScript { get; set; }
        public string DestinationScript { get; set; }
        public DifferenceType DiffType { get; set; }
        public ObjectType ObjectKind { get; set; }
        public string Name { get; set; }
    }
}
