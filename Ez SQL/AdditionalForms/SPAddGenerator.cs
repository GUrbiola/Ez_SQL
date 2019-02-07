using System;
using Ez_SQL.DataBaseObjects;

namespace Ez_SQL.AdditionalForms
{
    public class SPAddGenerator
    {
        public String TemplateString { get; set; }
        public Table DbTable { get; set; }
        public SPAddGenerator(string script, Table table)
        {
            this.TemplateString = script;
            this.DbTable = table;
        }
        public override string ToString()
        {
            TemplateString = TemplateString.Replace("@TableName@", DbTable.Name);
            TemplateString = TemplateString.Replace("@Params@", GetParams());
            TemplateString = TemplateString.Replace("@Schema@", DbTable.Schema);
            TemplateString = TemplateString.Replace("@Fields@", GetFields());
            TemplateString = TemplateString.Replace("@Values@", GetValues());
            TemplateString = TemplateString.Replace("@Id@", DbTable.Childs.Exists(x => x.IsPrimaryKey && x.IsIdentity) ? "SCOPE_IDENTITY()" : "1");

            return TemplateString;
        }
        private string GetParams()
        {
            string back = "";
            foreach (ISqlChild tableChild in DbTable.Childs)
            {
                if (tableChild.IsIdentity)
                {
                    continue;
                }
                else
                {
                    if (String.IsNullOrEmpty(back))
                    {
                        back = "( @" + tableChild.Name + " " + tableChild.Type.ToUpper();
                        if (
                            tableChild.Type.Equals("varchar", StringComparison.CurrentCultureIgnoreCase)
                            || tableChild.Type.Equals("nvarchar", StringComparison.CurrentCultureIgnoreCase)
                            || tableChild.Type.Equals("char", StringComparison.CurrentCultureIgnoreCase)
                            || tableChild.Type.Equals("nchar", StringComparison.CurrentCultureIgnoreCase)
                            || tableChild.Type.Equals("binary", StringComparison.CurrentCultureIgnoreCase)
                            || tableChild.Type.Equals("varbinary", StringComparison.CurrentCultureIgnoreCase)
                        )
                        {
                            back += "(" + tableChild.Precision + ")";

                        }
                    }
                    else
                    {
                        back += ", @" + tableChild.Name + " " + tableChild.Type.ToUpper();
                        if (
                            tableChild.Type.Equals("varchar", StringComparison.CurrentCultureIgnoreCase)
                            || tableChild.Type.Equals("nvarchar", StringComparison.CurrentCultureIgnoreCase)
                            || tableChild.Type.Equals("char", StringComparison.CurrentCultureIgnoreCase)
                            || tableChild.Type.Equals("nchar", StringComparison.CurrentCultureIgnoreCase)
                            || tableChild.Type.Equals("binary", StringComparison.CurrentCultureIgnoreCase)
                            || tableChild.Type.Equals("varbinary", StringComparison.CurrentCultureIgnoreCase)
                        )
                        {
                            back += "(" + tableChild.Precision + ")";

                        }
                    }

                }
            }

            if (!String.IsNullOrEmpty(back))
                back += " )";

            return back;
        }
        private string GetFields()
        {
            string back = "";
            foreach (ISqlChild tableChild in DbTable.Childs)
            {
                if (tableChild.IsIdentity)
                {
                    continue;
                }
                else
                {
                    if (String.IsNullOrEmpty(back))
                    {
                        back = "( [" + tableChild.Name + "]";
                    }
                    else
                    {
                        back += ", [" + tableChild.Name + "]";
                    }
                }
            }

            if (!String.IsNullOrEmpty(back))
                back += " )";

            return back;
        }
        private string GetValues()
        {
            string back = "";
            foreach (ISqlChild tableChild in DbTable.Childs)
            {
                if (tableChild.IsIdentity)
                {
                    continue;
                }
                else
                {
                    if (String.IsNullOrEmpty(back))
                    {
                        back = "( @" + tableChild.Name;
                    }
                    else
                    {
                        back += ", @" + tableChild.Name;
                    }
                }
            }

            if (!String.IsNullOrEmpty(back))
                back += " )";

            return back;
        }
    }
}