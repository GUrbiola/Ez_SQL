using System;
using System.Collections;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Structure
{
    public interface IDiffList
    {
        int Count();
        IComparable GetByIndex(int index);
    }
}
