using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL.MultiQueryForm.Dialogs
{
    public class GenerateClassModelSettings
    {
        public bool AutoImplementedProperties { get; set; }
        public bool DataMemberDecoration { get; set; }
        public bool PkAlias { get; set; }
        public bool FieldCount { get; set; }
        public bool PkConstructor { get; set; }
        public bool StaticDataTableConverters { get; set; }
        public bool ToObjectArray { get; set; }
        public bool ToObjectList { get; set; }
        public bool ToObjectDictionary { get; set; }
        public bool IntegerIndexer { get; set; }
        public bool StringIndexer { get; set; }
        public bool FieldType { get; set; }
    }
}
