using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL.DataBaseObjects
{
    /// <summary>
    /// Identifies the SQL Server database object category for an <see cref="ISqlObject"/> instance.
    /// Used throughout the object browser and code-generation dialogs to distinguish object types.
    /// </summary>
    public enum ObjectType { Schema, Table, View, Procedure, ScalarFunction, TableFunction, Alias };

    /// <summary>
    /// Identifies the child element category for an <see cref="ISqlChild"/> instance:
    /// a column (<see cref="Field"/>) or a stored-procedure / function parameter (<see cref="Parameter"/>).
    /// </summary>
    public enum ChildType { Field, Parameter };
}
