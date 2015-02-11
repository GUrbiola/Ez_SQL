using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez_SQL.Extensions;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace Ez_SQL.DataBaseObjects
{
    public class Field : ISqlChild
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SafeScriptName 
        {
            get
            {
                if (Name.LastIndexOf(' ') >= 0 || Name.IsReserved())
                {
                    return String.Format("[{0}]", Name);
                }
                return Name;
            }
        }
        private ChildType _Kind;
        public ChildType Kind { get { return _Kind; } }
        public string Type { get; set; }
        public int Precision { get; set; }
        public bool Nullable { get; set; }
        public bool Computed { get; set; }
        public bool IsPrimaryKey { get; set; }
        public int Seed { get; set; }
        public int Increment { get; set; }
        public bool IsIdentity { get; set; }
        public string IdentityScript { get; set; }
        public string Comment { get; set; }
        public ISqlObject Parent { get; set; }
        public bool IsForeignKey { get; set; }
        public int ForeignKey { get; set; }
        public string DefaultValue { get; set; }
        public ISqlObject ReferenceParent { get; set; }
        public string ReferenceParentName { get; set; }
        public ISqlChild ReferenceChild { get; set; }
        public string ReferenceChildName { get; set; }
        public string CSharpType { get; set; }
        public Field()
        {
            _Kind = ChildType.Field;
        }


        public string Description
        {
            get { return String.Format("Table field: {0}, Table: {1}, Schema: {2}", Name, Parent.Name, Parent.Schema); }
        }
        public int ImageIndex
        {
            get { return 5; }
        }
        public bool InsertAction(ICSharpCode.TextEditor.TextArea textArea, char ch)
        {
            textArea.InsertString(String.Format("{0}", Name));
            return false;
        }
        public double Priority
        {
            get { return 2.0; }
        }
        public string Text
        {
            get { return Name; }
            set { Name = value; }
        }
    }
}
