using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL.MultiQueryForm.Dialogs
{
    /// <summary>
    /// Configuration options for generating a C# data-access method that wraps
    /// a non-query stored procedure (INSERT / UPDATE / DELETE).
    /// Persisted to <c>NonQuerySp.cfg</c> by <see cref="NonQuerySp"/>.
    /// </summary>
    public class GenerateNonQuerySpModelSettings
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

        /// <summary>When true, the generated method returns a StoredProcedureResult (SPR) object instead of void.</summary>
        public bool ReturnSPR { get; set; }
    }
}
