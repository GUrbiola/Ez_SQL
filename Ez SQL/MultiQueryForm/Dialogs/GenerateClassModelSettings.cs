using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL.MultiQueryForm.Dialogs
{
    /// <summary>
    /// Configuration options for C# class code generation from a SQL query result or database table.
    /// Serialized to/from a settings file so that user choices persist between sessions.
    /// </summary>
    public class GenerateClassModelSettings
    {
        /// <summary>When true, properties are generated as auto-implemented (get; set;) instead of field-backed.</summary>
        public bool AutoImplementedProperties { get; set; }

        /// <summary>When true, properties are decorated with [DataMember] for WCF/serialization support.</summary>
        public bool DataMemberDecoration { get; set; }

        /// <summary>When true, a static string constant alias for the primary key field is emitted.</summary>
        public bool PkAlias { get; set; }

        /// <summary>When true, a static integer constant with the field count is included in the generated class.</summary>
        public bool FieldCount { get; set; }

        /// <summary>When true, a constructor accepting the primary key value is generated.</summary>
        public bool PkConstructor { get; set; }

        /// <summary>When true, static DataTable → List and DataTable → Dictionary converter methods are included.</summary>
        public bool StaticDataTableConverters { get; set; }

        /// <summary>When true, a ToObjectArray() method converting the instance to object[] is generated.</summary>
        public bool ToObjectArray { get; set; }

        /// <summary>When true, a static method converting a DataTable to List&lt;T&gt; is generated.</summary>
        public bool ToObjectList { get; set; }

        /// <summary>When true, a static method converting a DataTable to Dictionary&lt;key,T&gt; is generated.</summary>
        public bool ToObjectDictionary { get; set; }

        /// <summary>When true, an integer indexer property is included in the generated class.</summary>
        public bool IntegerIndexer { get; set; }

        /// <summary>When true, a string (field-name) indexer property is included in the generated class.</summary>
        public bool StringIndexer { get; set; }

        /// <summary>When true, field type information is embedded as a static enum or constant.</summary>
        public bool FieldType { get; set; }

        /// <summary>When true, XML documentation comments are added to the generated class and its members.</summary>
        public bool IncludeComments { get; set; }
    }
}
