using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace Ez_SQL.DataBaseObjects
{
    /// <summary>
    /// Represents a top-level SQL Server database object (table, view, procedure, function, schema, or alias)
    /// that can appear in the object browser and participate in auto-complete.
    /// Extends <see cref="ICSharpCode.TextEditor.Gui.CompletionWindow.ICompletionData"/> so instances can be inserted directly into the text editor.
    /// </summary>
    public interface ISqlObject : ICompletionData
    {
        /// <summary>Gets or sets the SQL Server object ID (<c>sys.objects.object_id</c>).</summary>
        int Id { get; set; }

        /// <summary>Gets or sets the unqualified object name.</summary>
        string Name { get; set; }

        /// <summary>Gets the <see cref="ObjectType"/> category of this object.</summary>
        ObjectType Kind { get; }

        /// <summary>Gets the DDL creation script for this object, populated by <see cref="LoadScript"/>.</summary>
        string Script { get; }

        /// <summary>Gets or sets the schema that owns this object (e.g., <c>dbo</c>).</summary>
        string Schema { get; set; }

        /// <summary>Gets or sets an optional user comment or description for this object.</summary>
        string Comment { get; set; }

        /// <summary>Gets or sets the child elements (columns or parameters) belonging to this object.</summary>
        List<ISqlChild> Childs { get; set; }

        /// <summary>Gets a value indicating whether <see cref="LoadScript"/> has been called and the script is non-empty.</summary>
        bool IsScriptLoaded { get; }

        /// <summary>
        /// Loads the DDL creation script for this object using the supplied <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="cmd">An open or openable <see cref="SqlCommand"/> pointing to the target database.</param>
        void LoadScript(SqlCommand cmd);
    }
}
