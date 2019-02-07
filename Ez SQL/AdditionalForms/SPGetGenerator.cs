using System;
using Ez_SQL.DataBaseObjects;

namespace Ez_SQL.AdditionalForms
{
    public class SPGetGenerator
    {
        public String TemplateString { get; set; }
        public Table DbTable { get; set; }
        public SPGetGenerator(string script, Table table)
        {
            this.TemplateString = script;
            this.DbTable = table;
        }
        public override string ToString()
        {
            TemplateString = TemplateString.Replace("@TableName@", DbTable.Name);
            TemplateString = TemplateString.Replace("@Params@", GetParams());
            TemplateString = TemplateString.Replace("@Fields@", GetFields());
            TemplateString = TemplateString.Replace("@Schema@", DbTable.Schema);
            TemplateString = TemplateString.Replace("@Filter@", GetFilter());
            TemplateString = TemplateString.Replace("@Id@", "1");

            return TemplateString;
        }
        private string GetParams()
        {
            string back = "";
            foreach (ISqlChild tableChild in DbTable.Childs)
            {
                if (tableChild.IsPrimaryKey)
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
                if (String.IsNullOrEmpty(back))
                {
                    back = "[" + tableChild.Name + "]";
                }
                else
                {
                    back += Environment.NewLine + "\t\t\t\t, [" + tableChild.Name + "]";
                }

            }

            return back;
        }
        private string GetFilter()
        {
            string back = "";
            foreach (ISqlChild tableChild in DbTable.Childs)
            {
                if (tableChild.IsPrimaryKey)
                {
                    if (String.IsNullOrEmpty(back))
                    {
                        back += "([" + tableChild.Name + "] = @" + tableChild.Name + " OR @" + tableChild.Name + " IS NULL)";
                    }
                    else
                    {
                        back += Environment.NewLine + "\t\t\t\tAND ([" + tableChild.Name + "] = @" + tableChild.Name + " OR @" + tableChild.Name + " IS NULL)";
                    }
                }
            }

            return back;
        }

    }
}