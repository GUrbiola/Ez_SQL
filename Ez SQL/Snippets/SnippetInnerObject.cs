using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez_SQL.DataBaseObjects;

namespace Ez_SQL.Snippets
{
    public class SnippetInnerObject
    {
        public ISqlObject Object { get; set; }
        public string Id { get; set; }
        public string Name { get { return String.Format("$OBJ:{0}$", Id); } }
        public string Alias { get; set; }
        public string AllFieldsText { get { return String.Format("$OBJF*:{0}", Id); } }
        public string FieldsText { get { return String.Format("$OBJF:{0}", Id); } }
        public string AllParamsText { get { return String.Format("$OBJP*:{0}", Id); } }
        public string ParamsText { get { return String.Format("$OBJP:{0}", Id); } }
    }
}

