using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace Ez_SQL.DataBaseObjects
{
    public class Table : ISqlObject
    {
        public Table()
        {
            Childs = new List<ISqlChild>();
            _Kind = ObjectType.Table;
            _Script = "";
        }
        public int Id { get; set; }
        public string Name { get; set; }
        private ObjectType _Kind;
        public ObjectType Kind { get { return _Kind; } }
        private string _Script;
        public string Script { get { return _Script; } }
        public string Schema { get; set; }
        public string Comment { get; set; }
        public List<ISqlChild> Childs { get; set; }
        public void LoadScript(SqlCommand cmd = null)
        {
            //Generate script for the creation of table
            StringBuilder sc = new StringBuilder();
            sc.AppendLine(String.Format("CREATE TABLE {0}.{1}", Schema, Name));
            sc.AppendLine("(");

            foreach (ISqlChild child in Childs)
            {
                
                if (Childs.IndexOf(child) > 0)
                    sc.Append(String.Format("\t{0}{1}", ",", child.Name));
                else
                    sc.Append(String.Format("\t{0}", child.Name));

                if (child.Computed)
                {
                    sc.AppendLine(" AS " + child.DefaultValue);
                }
                else
                {
                    sc.Append(String.Format(" {0}", child.Type.ToUpper()));
                    if (child.Type.ToLower().Trim() == "varchar" || child.Type.ToLower().Trim() == "char" || child.Type.ToLower().Trim() == "nvarchar")
                    {
                        sc.Append(String.Format("({0})", child.Precision));
                    }

                    if (!child.Nullable || child.IsPrimaryKey)
                        sc.Append(" NOT NULL ");
                    else
                        sc.Append(" NULL ");

                    if (child.IsIdentity)
                        sc.Append(String.Format(" identity( {0}, {1} )", child.Seed.ToString(), child.Increment.ToString()));

                    if (!child.Computed && !String.IsNullOrEmpty(child.DefaultValue))
                    {
                        sc.Append(" DEFAULT " + child.DefaultValue);
                    }
                }
                sc.AppendLine();
            }

            string fields = "";
            //create primary key   
            foreach (ISqlChild child in Childs)
                if (child.IsPrimaryKey)
                    fields += child.Name + ", ";

            if (!String.IsNullOrEmpty(fields))
                sc.AppendLine(String.Format("\t,CONSTRAINT PK_{0} PRIMARY KEY({1})", Name, fields.Substring(0, fields.Length - 2)));

            List<string> ParentReferences = new List<string>();
            //create foreign keys
            foreach (ISqlChild child in Childs)
            {
                if (child.IsForeignKey)
                {
                    //,CONSTRAINT FK_CompensationDetails_Id FOREIGN KEY(CompensationPeriod) REFERENCES Id(CompensationPeriods)
                    sc.AppendLine(String.Format("\t,CONSTRAINT FK_{0}_{1}{5} FOREIGN KEY({2}) REFERENCES {3}({4})",
                                                                Name,
                                                                child.ReferenceParentName,
                                                                child.Name,
                                                                child.ReferenceParentName,
                                                                child.ReferenceChildName,
                                                                !ParentReferences.Contains(child.ReferenceParentName) ? "" : ParentReferences.Count(x => x == child.ReferenceParentName).ToString()
                                                                ));
                    ParentReferences.Add(child.ReferenceParentName);
                }
            }
            sc.AppendLine(")");
            _Script = sc.ToString();
        }
        public bool IsScriptLoaded { get { return !String.IsNullOrEmpty(_Script); } }

        public string Description
        {
            get { return String.Format("Database table: {0}, schema: {1}", Name, Schema); }
        }
        public int ImageIndex
        {
            get { return 1; }
        }
        public bool InsertAction(ICSharpCode.TextEditor.TextArea textArea, char ch)
        {
            if(ch == '1')
                textArea.InsertString(String.Format("{0}", Name));
            else
                textArea.InsertString(String.Format("{0}.{1}", Schema, Name));
            return false;
        }
        public double Priority
        {
            get { return 1.0; }
        }
        public string Text
        {
            get { return Name; }
            set { Name = value; }
        }
    }
}
