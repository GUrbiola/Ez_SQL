using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace Ez_SQL.DataBaseObjects
{
    /// <summary>
    /// Represents a child element of an <see cref="ISqlObject"/>: either a table column (<see cref="Field"/>)
    /// or a stored-procedure/function parameter (<see cref="Parameter"/>).
    /// Extends <see cref="ICSharpCode.TextEditor.Gui.CompletionWindow.ICompletionData"/> so child elements
    /// can be inserted into the text editor via auto-complete.
    /// </summary>
    public interface ISqlChild : ICompletionData
    {
        /// <summary>Gets or sets the SQL Server column or parameter ID.</summary>
        int Id { get; set; }

        /// <summary>Gets or sets the column or parameter name.</summary>
        string Name { get; set; }

        /// <summary>Gets whether this is a <see cref="ChildType.Field"/> (column) or <see cref="ChildType.Parameter"/>.</summary>
        ChildType Kind { get; }

        /// <summary>Gets or sets the SQL data type (e.g., <c>varchar</c>, <c>int</c>).</summary>
        string Type { get; set; }

        /// <summary>Gets or sets the maximum length or numeric precision of the column/parameter.</summary>
        int Precision { get; set; }

        /// <summary>Gets or sets whether the column allows NULL values.</summary>
        bool Nullable { get; set; }

        /// <summary>Gets or sets whether this is a computed column.</summary>
        bool Computed { get; set; }

        /// <summary>Gets or sets whether this column is part of the table's primary key.</summary>
        bool IsPrimaryKey { get; set; }

        /// <summary>Gets or sets the identity seed value (used when <see cref="IsIdentity"/> is <c>true</c>).</summary>
        int Seed { get; set; }

        /// <summary>Gets or sets the identity increment value (used when <see cref="IsIdentity"/> is <c>true</c>).</summary>
        int Increment { get; set; }

        /// <summary>Gets or sets whether this column is an IDENTITY column.</summary>
        bool IsIdentity { get; set; }

        /// <summary>Gets the T-SQL identity specification fragment (e.g., <c>IDENTITY(1,1)</c>).</summary>
        string IdentityScript { get; }

        /// <summary>Gets or sets an optional user comment for this column or parameter.</summary>
        string Comment { get; set; }

        /// <summary>Gets or sets the <see cref="ISqlObject"/> that owns this child element.</summary>
        ISqlObject Parent { get; set; }

        /// <summary>Gets whether this column participates in a foreign key constraint.</summary>
        bool IsForeignKey { get; }

        /// <summary>Gets or sets the foreign key constraint ID.</summary>
        int ForeignKey { get; set; }

        /// <summary>Gets or sets the column default value expression or computed expression text.</summary>
        string DefaultValue { get; set; }

        /// <summary>Gets or sets the parent table referenced by this foreign key.</summary>
        ISqlObject ReferenceParent { get; set; }

        /// <summary>Gets or sets the name of the parent table referenced by this foreign key.</summary>
        string ReferenceParentName { get; set; }

        /// <summary>Gets or sets the column in the referenced parent table that this foreign key points to.</summary>
        ISqlChild ReferenceChild { get; set; }

        /// <summary>Gets or sets the name of the column in the referenced parent table.</summary>
        string ReferenceChildName { get; set; }
    }
}
