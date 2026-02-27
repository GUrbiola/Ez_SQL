using System;
using Ez_SQL.DataBaseObjects;

namespace Ez_SQL.AdditionalForms
{
    /// <summary>
    /// Generates the SQL source for an UPDATE stored procedure from a table template.
    /// The template file (e.g. <c>SP_Update.sql</c>) contains named placeholders replaced
    /// with table-specific values: <c>@TableName@</c>, <c>@Schema@</c>,
    /// <c>@Params@</c> (all columns as parameters), <c>@Updates@</c> (SET clause for non-PK columns),
    /// <c>@Filter@</c> (WHERE clause using PKs), and <c>@Id@</c>.
    /// Primary-key columns appear in <c>@Params@</c> and <c>@Filter@</c> but not in <c>@Updates@</c>.
    /// </summary>
    public class SPUpdateGenerator
    {
        /// <summary>Gets or sets the SQL template string whose placeholders will be replaced.</summary>
        public String TemplateString { get; set; }

        /// <summary>Gets or sets the SQL Server table used to drive placeholder substitution.</summary>
        public Table DbTable { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="SPUpdateGenerator"/> with the given template and table.
        /// </summary>
        /// <param name="script">The raw SQL template text containing placeholder tokens.</param>
        /// <param name="table">The table whose metadata is used to fill in the placeholders.</param>
        public SPUpdateGenerator(string script, Table table)
        {
            this.TemplateString = script;
            this.DbTable = table;
        }

        /// <summary>
        /// Replaces all template placeholders with table-specific values and returns the completed SQL script.
        /// </summary>
        /// <returns>The fully rendered UPDATE stored procedure SQL script.</returns>
        public override string ToString()
        {
            TemplateString = TemplateString.Replace("@TableName@", DbTable.Name);
            TemplateString = TemplateString.Replace("@Params@", GetParams());
            TemplateString = TemplateString.Replace("@Schema@", DbTable.Schema);
            TemplateString = TemplateString.Replace("@Updates@", GetUpdates());
            TemplateString = TemplateString.Replace("@Filter@", GetFilter());
            TemplateString = TemplateString.Replace("@Id@", "1");

            return TemplateString;
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
                        back += "[" + tableChild.Name + "] = @" + tableChild.Name;
                    }
                    else
                    {
                        back += Environment.NewLine + "\t\t\t\tAND [" + tableChild.Name + "] = @" + tableChild.Name;
                    }
                }
            }

            return back;
        }
        private string GetUpdates()
        {
            string back = "";
            foreach (ISqlChild tableChild in DbTable.Childs)
            {
                if (!tableChild.IsPrimaryKey)
                {
                    if (String.IsNullOrEmpty(back))
                    {
                        back += "[" + tableChild.Name + "] = @" + tableChild.Name;
                    }
                    else
                    {
                        back += Environment.NewLine + "\t\t\t\t,[" + tableChild.Name + "] = @" + tableChild.Name;
                    }
                }
            }

            return back;
        }
        private string GetParams()
        {
            string back = "";
            foreach (ISqlChild tableChild in DbTable.Childs)
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

            if (!String.IsNullOrEmpty(back))
                back += " )";

            return back;
        }
    }
}