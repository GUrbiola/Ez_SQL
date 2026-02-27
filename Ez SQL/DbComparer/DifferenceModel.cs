using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez_SQL.DataBaseObjects;

namespace Ez_SQL.DbComparer
{
    /// <summary>
    /// Describes the type of difference found between a source and destination database object.
    /// </summary>
    public enum DifferenceType
    {
        /// <summary>The object exists in the source but not the destination and must be added.</summary>
        Add,
        /// <summary>The object exists in both databases but its script differs and must be updated.</summary>
        Update,
        /// <summary>The object exists in the destination but not the source and should be deleted.</summary>
        Delete,
        /// <summary>No difference detected; the object is identical in both databases.</summary>
        None
    }

    /// <summary>
    /// Represents a single detected difference between a source and destination database object
    /// when comparing two SQL Server databases. Produced by <see cref="DbComparer"/> during a comparison run.
    /// </summary>
    public class DifferenceModel
    {
        /// <summary>Gets or sets the DDL script of the object from the source database.</summary>
        public string SourceScript { get; set; }

        /// <summary>Gets or sets the DDL script of the object from the destination database.</summary>
        public string DestinationScript { get; set; }

        /// <summary>Gets or sets the kind of difference: Add, Update, Delete, or None.</summary>
        public DifferenceType DiffType { get; set; }

        /// <summary>Gets or sets the SQL Server object type (Table, View, Procedure, etc.).</summary>
        public ObjectType ObjectKind { get; set; }

        /// <summary>Gets or sets the name of the database object that differs.</summary>
        public string Name { get; set; }
    }
}
