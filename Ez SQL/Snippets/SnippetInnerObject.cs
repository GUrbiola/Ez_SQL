using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez_SQL.DataBaseObjects;

namespace Ez_SQL.Snippets
{
    /// <summary>
    /// Represents a SQL Server database object that has been bound to a snippet placeholder slot.
    /// Each instance tracks the bound <see cref="ISqlObject"/>, its slot identifier, and
    /// an optional user alias. The computed properties produce the placeholder token strings
    /// used inside a snippet script body to reference the object's name, fields, or parameters
    /// at expansion time.
    /// </summary>
    public class SnippetInnerObject
    {
        /// <summary>Gets or sets the SQL Server database object bound to this slot.</summary>
        public ISqlObject Object { get; set; }

        /// <summary>Gets or sets the unique slot identifier for this object within a snippet.</summary>
        public string Id { get; set; }

        /// <summary>Gets the object-reference placeholder token: <c>$OBJ:{Id}$</c>.</summary>
        public string Name { get { return String.Format("$OBJ:{0}$", Id); } }

        /// <summary>Gets or sets an optional user-provided alias for this object in the snippet.</summary>
        public string Alias { get; set; }

        /// <summary>Gets the all-fields placeholder token (all fields): <c>$OBJF*:{Id}</c>.</summary>
        public string AllFieldsText { get { return String.Format("$OBJF*:{0}", Id); } }

        /// <summary>Gets the selected-fields placeholder token: <c>$OBJF:{Id}</c>.</summary>
        public string FieldsText { get { return String.Format("$OBJF:{0}", Id); } }

        /// <summary>Gets the all-parameters placeholder token: <c>$OBJP*:{Id}</c>.</summary>
        public string AllParamsText { get { return String.Format("$OBJP*:{0}", Id); } }

        /// <summary>Gets the selected-parameters placeholder token: <c>$OBJP:{Id}</c>.</summary>
        public string ParamsText { get { return String.Format("$OBJP:{0}", Id); } }
    }
}

