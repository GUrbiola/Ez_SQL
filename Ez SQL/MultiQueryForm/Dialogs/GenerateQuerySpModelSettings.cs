using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL.MultiQueryForm.Dialogs
{
    /// <summary>
    /// Configuration options for generating a C# data-access method that wraps
    /// a query stored procedure (SELECT), where the caller receives a typed result.
    /// Persisted to <c>QuerySp.cfg</c> by <see cref="QuerySp"/>.
    /// </summary>
    public class GenerateQuerySpModelSettings
    {
        /// <summary>When true, the generated method is wrapped in a #region block.</summary>
        public bool InsideRegion { get; set; }

        /// <summary>When true, a log call is emitted at the start of the generated method.</summary>
        public bool LogStart { get; set; }

        /// <summary>When true, a log call is emitted at the end of the generated method.</summary>
        public bool LogEnd { get; set; }

        /// <summary>When true, exception logging code is included in the catch block.</summary>
        public bool LogException { get; set; }

        /// <summary>When true, the number of rows read from the reader is saved to a local variable.</summary>
        public bool SaveRowsReadCount { get; set; }

        /// <summary>When true, the number of rows affected by the command is saved to a local variable.</summary>
        public bool SaveRowsAffectedCount { get; set; }

        /// <summary>When true, the generated method wraps the command in a database transaction.</summary>
        public bool UseTransaction { get; set; }

        /// <summary>When true, execution time is measured and stored in the generated method.</summary>
        public bool MeasureTimeElapsed { get; set; }

        /// <summary>
        /// When true, the return type of the generated method is a user-defined class (named by <see cref="ReturnName"/>).
        /// Mutually exclusive with <see cref="IsList"/> and <see cref="IsSPR"/>.
        /// </summary>
        public bool IsObject { get; set; }

        /// <summary>
        /// The name of the return type used in the generated method.
        /// Represents either a class name (when <see cref="IsObject"/> is true) or a primitive type name
        /// (when <see cref="IsList"/> is true).
        /// </summary>
        public string ReturnName { get; set; }

        /// <summary>
        /// When true, the generated method returns a <c>List&lt;T&gt;</c> of a primitive or class type.
        /// Mutually exclusive with <see cref="IsObject"/> and <see cref="IsSPR"/>.
        /// </summary>
        public bool IsList { get; set; }

        /// <summary>
        /// When true, the generated method returns a StoredProcedureResult (SPR) object.
        /// Mutually exclusive with <see cref="IsObject"/> and <see cref="IsList"/>.
        /// </summary>
        public bool IsSPR { get; set; }
    }
}
