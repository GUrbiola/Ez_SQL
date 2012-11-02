using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL
{
    public enum ProgressType { SimpleMessage, ProgressSimple, ProgressComplete, Clear }
    public class ProgressObject
    {
        public string Message { get; set; }
        public string GeneralMessage { get; set; }
        public int GeneralProgress { get; set; }
        public string CurrentMessage { get; set; }
        public int CurrentProgress { get; set; }
        public ProgressType Type { get; set; }
    }
}
