using System;
using Ez_SQL.DataBaseObjects;

namespace Ez_SQL.AdditionalForms
{
    /// <summary>
    /// Generates the SQL source for an INSERT stored procedure from a table template.
    /// The template file (e.g. <c>SP_Add.sql</c>) contains named placeholders that are
    /// replaced with table-specific values: <c>@TableName@</c>, <c>@Schema@</c>,
    /// <c>@Params@</c> (SP parameter list), <c>@Fields@</c> (INSERT field list),
    /// <c>@Values@</c> (VALUES clause), and <c>@Id@</c> (SCOPE_IDENTITY() or 1).
    /// Identity columns are excluded from all generated lists.
    /// </summary>
    public class SPAddGenerator
    {
        /// <summary>Gets or sets the SQL template string whose placeholders will be replaced.</summary>
        public String TemplateString { get; set; }

        /// <summary>Gets or sets the SQL Server table used to drive placeholder substitution.</summary>
        public Table DbTable { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="SPAddGenerator"/> with the given template and table.
        /// </summary>
        /// <param name="script">The raw SQL template text containing placeholder tokens.</param>
        /// <param name="table">The table whose metadata is used to fill in the placeholders.</param>
        public SPAddGenerator(string script, Table table)
        {
            this.TemplateString = script;
            this.DbTable = table;
        }

        /// <summary>
        /// Replaces all template placeholders with table-specific values and returns the completed SQL script.
        /// </summary>
        /// <returns>The fully rendered INSERT stored procedure SQL script.</returns>
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