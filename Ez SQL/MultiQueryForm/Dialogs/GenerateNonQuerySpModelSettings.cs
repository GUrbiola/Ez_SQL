using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL.MultiQueryForm.Dialogs
{
    public class GenerateNonQuerySpModelSettings
    {
        public bool InsideRegion { get; set; }
        public bool LogStart { get; set; }
        public bool LogEnd { get; set; }
        public bool LogException { get; set; }
        public bool SaveRowsReadCount { get; set; }
        public bool SaveRowsAffectedCount { get; set; }
        public bool UseTransaction { get; set; }
        public bool MeasureTimeElapsed { get; set; }
        public bool ReturnSPR { get; set; }
    }
}
