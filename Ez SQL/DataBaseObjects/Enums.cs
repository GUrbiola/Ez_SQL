using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL.DataBaseObjects
{
    public enum ObjectType { Schema, Table, View, Procedure, ScalarFunction, TableFunction, Alias };
    public enum ChildType { Field, Parameter };
}
