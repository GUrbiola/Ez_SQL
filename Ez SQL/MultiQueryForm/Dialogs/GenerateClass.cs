using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ez_SQL.DataBaseObjects;
using Ez_SQL.Common_Code;
using View = Ez_SQL.DataBaseObjects.View;

namespace Ez_SQL.MultiQueryForm.Dialogs
{
    public partial class GenerateClass : Form
    {
        private bool PocoGenerator { get; set; }
        //private bool ClassGenerator { get { return !PocoGenerator; } set { PocoGenerator = !value; } }
        public Table BaseSQLTable { get; set; }
        public View BaseSQLView { get; set; }
        public string ModelName { get { return String.IsNullOrEmpty(txtClassName.Text) ? (BaseSQLTable == null ? BaseSQLView.Name : BaseSQLTable.Name) : txtClassName.Text; } }
        public string SettingsFileName { get { return MainForm.DataStorageDir + "\\Poco.cfg"; } }
        public GenerateClassModelSettings DialogSettings
        {
            get
            {
                GenerateClassModelSettings curSettings = new GenerateClassModelSettings()
                    {
                        AutoImplementedProperties = radAutoImplementedProperties.Checked,
                        DataMemberDecoration = chkDataMemberDecoration.Checked,
                        FieldCount = chkFieldCount.Checked,
                        FieldType = chkFieldType.Checked,
                        IntegerIndexer = chkIntegerIndexer.Checked,
                        PkAlias = chkPKAlias.Checked,
                        PkConstructor = chkPKConstructor.Checked,
                        StaticDataTableConverters = chkLoadFromDataTable.Checked,
                        StringIndexer = chkStringIndexer.Checked,
                        ToObjectArray = chkObjectArray.Checked,
                        ToObjectDictionary = chkDictionary.Checked,
                        ToObjectList = chkObjectList.Checked,
                        IncludeComments = chkIncludeComments.Checked
                    };
                return curSettings;
            }
        }
        public string GenerateCSharpClassFromTable(ISqlObject table, GenerateClassModelSettings settings, string modelName = "")
        {
            ISqlObject curTable = new Table();
            //curTable.IsScriptLoaded = table.IsScriptLoaded;
            //curTable.Kind = table.Kind;
            curTable.Comment = table.Comment;
            curTable.Id = curTable.Id;
            curTable.Name = table.Name;
            curTable.Schema = table.Schema;
            curTable.Childs = new List<ISqlChild>();

            foreach (ISqlChild child in table.Childs)
            {
                CheckBox _chkInclude = panelFields.Controls[child.Name + "_chkInclude"]  as CheckBox;
                if(_chkInclude != null && _chkInclude.Checked)
                    curTable.Childs.Add(child);
            }

            List<Field> Pks = new List<Field>();
            foreach (ISqlChild child in curTable.Childs)
            {
                CheckBox _chkKey = panelFields.Controls[child.Name + "_chkKey"] as CheckBox;
                if (_chkKey != null && _chkKey.Checked)
                    Pks.Add(child as Field);
            }
            
            string TableName, aux;
            StringBuilder SbBll = new StringBuilder();
            
            TableName = String.IsNullOrEmpty(modelName) ? curTable.Name : modelName;

            if (settings.IncludeComments)
            {
                SbBll.AppendLine("/// <summary>".Indent());
                SbBll.AppendLine("/// Business layer class to manipulate the table: ".Indent() + curTable.Name);
                SbBll.AppendLine("/// </summary>".Indent());
            }

            SbBll.AppendLine(String.Format("public class {0}".Indent(), TableName));
            SbBll.AppendLine("{".Indent());

            #region Properties of the class(and variables... if required)
            SbBll.AppendLine("#region Properties".Indent(2));
            for (int i = 0; i < curTable.Childs.Count; i++)
            {
                Field F = curTable.Childs[i] as Field;
                if (F == null)
                    continue;

                if (QueryForm.SqlVsCSharp.ContainsKey(F.Type))
                {
                    if (F.Nullable && !QueryForm.SqlVsCSharp[F.Type].Equals("string", StringComparison.CurrentCultureIgnoreCase))
                        F.CSharpType = QueryForm.SqlVsCSharp[F.Type] + "?";
                    else
                        F.CSharpType = QueryForm.SqlVsCSharp[F.Type];
                }
                else
                {
                    F.CSharpType = "??";
                }

                F.DefaultValue = F.Nullable ? "null" : DefaultValueFor(F.CSharpType.EndsWith("?") ? F.CSharpType.Replace("?", "") : F.CSharpType);

                CheckBox _chkRequired = panelFields.Controls[F.Name + "_chkRequired"] as CheckBox;
                CheckBox _chkLength = panelFields.Controls[F.Name + "_chkLength"] as CheckBox;
                TextBox _txtDisplay = panelFields.Controls[F.Name + "_txtDisplay"] as TextBox;

                if (settings.AutoImplementedProperties)
                {
                    if (settings.IncludeComments)
                    {
                        SbBll.AppendLine("/// <summary>".Indent(2));
                        SbBll.AppendLine(String.Format("/// Field Map, From {0}.{1} {2} -> To {3} {1}".Indent(2), curTable.Name, F.Name, F.Type, F.CSharpType));
                        SbBll.AppendLine("/// </summary>".Indent(2));
                    }

                    if (_chkRequired.Checked)
                        SbBll.AppendLine("[Required(ErrorMessage = \"This is a required field.\")]".Indent(2));

                    if (_chkLength.Checked)
                        SbBll.AppendLine(("[StringLength(" + F.Precision + ")]").Indent(2));

                    if (!_txtDisplay.Text.IsEmpty())
                        SbBll.AppendLine(("[DisplayName(\"" + _txtDisplay.Text + "\")]").Indent(2));

                    if (settings.DataMemberDecoration)
                        SbBll.AppendLine("[DataMember]".Indent(2));

                    SbBll.AppendLine(String.Format("public {0} {1} {{ get; set; }}".Indent(2), F.CSharpType, F.Name));

                    if (F.IsPrimaryKey && Pks.Count == 1 && !F.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (settings.IncludeComments)
                        {
                            SbBll.AppendLine("/// <summary>".Indent(2));
                            SbBll.AppendLine("/// Alias for primary key field".Indent(2));
                            SbBll.AppendLine("/// </summary>".Indent(2));
                        }

                        SbBll.AppendLine(String.Format("public {0} {1} {{ get{{ return {2}; }} set {{ {2} = value; }} }}".Indent(2), F.CSharpType, "Id", F.Name));
                    }
                }
                else
                {
                    SbBll.AppendLine(String.Format("private {0} _{1} = {2};".Indent(2), F.CSharpType, F.Name, F.DefaultValue));
                    if (settings.IncludeComments)
                    {
                        SbBll.AppendLine("/// <summary>".Indent(2));
                        SbBll.AppendLine(String.Format("/// Field Map, From {0}.{1} {2} -> To {3} {1}".Indent(2), curTable.Name, F.Name, F.Type, F.CSharpType));
                        SbBll.AppendLine("/// </summary>".Indent(2));
                    }

                    if (_chkRequired.Checked)
                        SbBll.AppendLine("[Required(ErrorMessage = \"This is a required field.\")]".Indent(2));

                    if (_chkLength.Checked)
                        SbBll.AppendLine(("[StringLength(" + F.Precision + ")]").Indent(2));

                    if (!_txtDisplay.Text.IsEmpty())
                        SbBll.AppendLine(("[DisplayName(\"" + _txtDisplay.Text + "\")]").Indent(2));
                    
                    if (settings.DataMemberDecoration)
                        SbBll.AppendLine("[DataMember]".Indent(2));

                    SbBll.AppendLine(String.Format("public {0} {1}".Indent(2), F.CSharpType, F.Name));
                    SbBll.AppendLine("{".Indent(2));
                    SbBll.AppendLine(String.Format("get {{ return _{0}; }}".Indent(3), F.Name));
                    SbBll.AppendLine(String.Format("set {{ _{0} = value; }}".Indent(3), F.Name));
                    SbBll.AppendLine("}".Indent(2));

                    if (F.IsPrimaryKey && Pks.Count == 1 && !F.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (settings.IncludeComments)
                        {
                            SbBll.AppendLine("/// <summary>".Indent(2));
                            SbBll.AppendLine("/// Alias for primary key field".Indent(2));
                            SbBll.AppendLine("/// </summary>".Indent(2));
                        }

                        SbBll.AppendLine(String.Format("public {0} {1}".Indent(2), F.CSharpType, "Id"));
                        SbBll.AppendLine("{".Indent(2));
                        SbBll.AppendLine(String.Format("get {{ return _{0}; }}".Indent(3), F.Name));
                        SbBll.AppendLine(String.Format("set {{ _{0} = value; }}".Indent(3), F.Name));
                        SbBll.AppendLine("}".Indent(2));
                    }              
                }

            }

            if (settings.FieldCount)
            {
                if (settings.IncludeComments)
                {
                    SbBll.AppendLine("/// <summary>".Indent(2));
                    SbBll.AppendLine("/// Total number of fields, for this table".Indent(2));
                    SbBll.AppendLine("/// </summary>".Indent(2));
                }

                SbBll.AppendLine(String.Format("public int FieldCount {{ get {{ return {0}; }} }}".Indent(2), curTable.Childs.Count.ToString()));
            }
            SbBll.AppendLine("#endregion".Indent(2));
            #endregion

            #region Class constructors
            SbBll.AppendLine("#region Class contructors, default, and one with only the Id(PK)".Indent(2));
            if (settings.IncludeComments)
            {
                SbBll.AppendLine("/// <summary>".Indent(2));
                SbBll.AppendLine("/// Basic constructor, parameterless".Indent(2));
                SbBll.AppendLine("/// </summary>".Indent(2));
            }

            SbBll.AppendLine(String.Format("public {0}()".Indent(2), TableName));
            SbBll.AppendLine("{".Indent(2));
            SbBll.AppendLine("}".Indent(2));

            if (settings.PkConstructor && Pks.Count > 0)
            {
                if (settings.IncludeComments)
                {
                    SbBll.AppendLine("/// <summary>".Indent(2));
                    SbBll.AppendLine("/// Constructor with only PK fields".Indent(2));
                    SbBll.AppendLine("/// </summary>".Indent(2));
                }

                foreach (Field Pk in Pks)
                    SbBll.AppendLine(String.Format("/// <param name=\"{0}\">Field that is part of the PK of the record.</param>".Indent(2), Pk.Name));
                aux = String.Format("public {0}(".Indent(2), TableName);
                
                foreach (Field Pk in Pks)
                    aux += String.Format("{0} {1}, ", Pk.CSharpType, Pk.Name);
                SbBll.AppendLine(aux.Substring(0, aux.Length - 2) + ")");
                SbBll.AppendLine("{".Indent(2));
                foreach (Field Pk in Pks)
                    SbBll.AppendLine(String.Format("this.{0} = {0};".Indent(3), Pk.Name));
                SbBll.AppendLine("}".Indent(2));
            }
            
            SbBll.AppendLine("#endregion".Indent(2));
            #endregion

            #region Static converters, create a list of objects from a DataTable
            if (settings.StaticDataTableConverters)
            {
                SbBll.AppendLine("#region Static function to convert and create from a datatable objects".Indent(2));
                if (settings.IncludeComments)
                {
                    SbBll.AppendLine("/// <summary>".Indent(2));
                    SbBll.AppendLine("/// Converts a datatable to a list of objects of this class.".Indent(2));
                    SbBll.AppendLine("/// </summary>".Indent(2));
                    SbBll.AppendLine("/// <param name=\"table\">The datatable to convert.</param>".Indent(2));
                    SbBll.AppendLine("/// <returns>List of objects of this class, created from ther datatable</returns>".Indent(2));
                }

                SbBll.AppendLine(String.Format("public static List<{0}> ConvertToObject(DataTable table)".Indent(2), TableName));
                SbBll.AppendLine("{".Indent(2));
                SbBll.AppendLine("return ConvertToObject(table, -1);".Indent(3));
                SbBll.AppendLine("}".Indent(2));

                if (settings.IncludeComments)
                {
                    SbBll.AppendLine("/// <summary>".Indent(2));
                    SbBll.AppendLine("/// Converts a datatable to a list of objects of this class.".Indent(2));
                    SbBll.AppendLine("/// </summary>".Indent(2));
                    SbBll.AppendLine("/// <param name=\"table\">The datatable to convert.</param>".Indent(2));
                    SbBll.AppendLine("/// <param name=\"firstRecords\">Number of records to convert, if -1 then all the records are converted.</param>".Indent(2));
                    SbBll.AppendLine("/// <returns>List of objects of this class, created from ther datatable</returns>".Indent(2));
                }

                SbBll.AppendLine(String.Format("public static List<{0}> ConvertToObject(DataTable table, int firstRecords)".Indent(2), TableName));
                SbBll.AppendLine("{".Indent(2));
                SbBll.AppendLine(String.Format("List<{0}> back = new List<{0}>();".Indent(3), TableName));
                SbBll.AppendLine("int i, t;".Indent(3));
                SbBll.AppendLine("".Indent(3));
                SbBll.AppendLine("if (table == null || table.Rows.Count == 0)".Indent(3));
                SbBll.AppendLine("return back;".Indent(4));
                SbBll.AppendLine("".Indent(3));
                SbBll.AppendLine("if(firstRecords > 0)".Indent(3));
                SbBll.AppendLine("t = Math.Min(firstRecords, table.Rows.Count);".Indent(4));
                SbBll.AppendLine("else".Indent(3));
                SbBll.AppendLine("t = table.Rows.Count;".Indent(4));
                SbBll.AppendLine("".Indent(3));

                SbBll.AppendLine("for (i = 0; i < t; i++)".Indent(3));
                SbBll.AppendLine("{".Indent(3));
                SbBll.AppendLine(String.Format("{0} obj = new {0}();".Indent(4), TableName));
                for (int i = 0; i < curTable.Childs.Count; i++)
                {
                    Field F = curTable.Childs[i] as Field;
                    if (F == null)
                        continue;
                    switch (F.CSharpType.Replace("?", ""))
                    {
                        case "int":
                            SbBll.AppendLine(String.Format("if (table.Rows[i][\"{0}\"] != DBNull.Value && !String.IsNullOrEmpty(table.Rows[i][\"{0}\"].ToString()))".Indent(4), F.Name));
                            SbBll.AppendLine(String.Format("obj.{0} = int.Parse(table.Rows[i][\"{0}\"].ToString());".Indent(4), F.Name));
                            break;
                        case "string":
                            SbBll.AppendLine(String.Format("obj.{0} = table.Rows[i][\"{0}\"].ToString();".Indent(4), F.Name));
                            break;
                        case "DateTime":
                            SbBll.AppendLine(String.Format("if (table.Rows[i][\"{0}\"] != DBNull.Value && !String.IsNullOrEmpty(table.Rows[i][\"{0}\"].ToString()))".Indent(4), F.Name));
                            SbBll.AppendLine(String.Format("obj.{0} = DateTime.Parse(table.Rows[i][\"{0}\"].ToString());".Indent(4), F.Name));
                            break;
                        case "float":
                            SbBll.AppendLine(String.Format("if (table.Rows[i][\"{0}\"] != DBNull.Value && !String.IsNullOrEmpty(table.Rows[i][\"{0}\"].ToString()))".Indent(4), F.Name));
                            SbBll.AppendLine(String.Format("obj.{0} = Single.Parse(table.Rows[i][\"{0}\"].ToString());".Indent(4), F.Name));
                            break;
                        case "double":
                            SbBll.AppendLine(String.Format("if (table.Rows[i][\"{0}\"] != DBNull.Value && !String.IsNullOrEmpty(table.Rows[i][\"{0}\"].ToString()))".Indent(4), F.Name));
                            SbBll.AppendLine(String.Format("obj.{0} = Double.Parse(table.Rows[i][\"{0}\"].ToString());".Indent(4), F.Name));
                            break;
                        case "bool":
                            SbBll.AppendLine(String.Format("if (table.Rows[i][\"{0}\"] != DBNull.Value && !String.IsNullOrEmpty(table.Rows[i][\"{0}\"].ToString()))".Indent(4), F.Name));
                            SbBll.AppendLine(String.Format("obj.{0} = bool.Parse(table.Rows[i][\"{0}\"].ToString());".Indent(4), F.Name));
                            break;
                    }
                }
                SbBll.AppendLine("back.Add(obj);".Indent(4));
                SbBll.AppendLine("}".Indent(3));
                SbBll.AppendLine("return back;".Indent(3));
                SbBll.AppendLine("}".Indent(2));
                SbBll.AppendLine("#endregion".Indent(2));
            }
            #endregion

            #region Instance converters, object array, object list or object dictionary
            if (settings.ToObjectArray || settings.ToObjectDictionary || settings.ToObjectList)
            {
                SbBll.AppendLine("#region Methods to convert the current object to an array/list/dictionary of his fields".Indent(2));

                #region Conversion to an array of objects
                if (settings.ToObjectArray)
                {
                    if (settings.IncludeComments)
                    {
                        SbBll.AppendLine("/// <summary>".Indent(2));
                        SbBll.AppendLine(String.Format("/// Converts the current object to an array of his fields values ({0} -> object[]).".Indent(2), TableName));
                        SbBll.AppendLine("/// </summary>".Indent(2));
                        SbBll.AppendLine("/// <returns>Arreglo de objetos(object[]) creado a partir del objeto actual.</returns>".Indent(2));
                    }

                    SbBll.AppendLine("public object[] ToParams()".Indent(2));
                    SbBll.AppendLine("{".Indent(2));
                    SbBll.AppendLine("List<object> objs = new List<object>();".Indent(3));
                    for (int i = 0; i < curTable.Childs.Count; i++)
                    {
                        Field F = curTable.Childs[i] as Field;
                        if (F != null)
                            SbBll.AppendLine(String.Format("objs.Add({0} == {1} ? null : (object){0});".Indent(3), F.Name, F.DefaultValue));
                    }
                    SbBll.AppendLine("return objs.ToArray();".Indent(3));
                    SbBll.AppendLine("}".Indent(2));
                }
                #endregion

                #region Conversion to a list of objects
                if (settings.ToObjectList)
                {
                    if (settings.IncludeComments)
                    {
                        SbBll.AppendLine("/// <summary>".Indent(2));
                        SbBll.AppendLine(String.Format("/// Converts the current object to alist of his field values ({0} -> <![CDATA[List<object>]]>).".Indent(2), TableName));
                        SbBll.AppendLine("/// </summary>".Indent(2));
                        SbBll.AppendLine("/// <returns>List of objects created from the current object.</returns>".Indent(2));
                    }

                    SbBll.AppendLine("public List<object> ToList()".Indent(2));
                    SbBll.AppendLine("{".Indent(2));
                    SbBll.AppendLine("List<object> objs = new List<object>();".Indent(3));
                    for (int i = 0; i < curTable.Childs.Count; i++)
                    {
                        Field F = curTable.Childs[i] as Field;
                        if (F != null)
                            SbBll.AppendLine(String.Format("objs.Add({0} == {1} ? null : (object){0});".Indent(3), F.Name, F.DefaultValue));
                    }
                    SbBll.AppendLine("return objs;".Indent(3));
                    SbBll.AppendLine("}".Indent(2));
                }
                #endregion

                #region Conversion to a dictionary
                if (settings.ToObjectDictionary)
                {
                    if (settings.IncludeComments)
                    {
                        SbBll.AppendLine("/// <summary>".Indent(2));
                        SbBll.AppendLine(String.Format("/// Converts the current object to a dictionary of his fields(<![CDATA[Dictionary<string, object>]]>) ({0} -> <![CDATA[Dictionary<string, object>]]>).".Indent(2), TableName));
                        SbBll.AppendLine("/// </summary>".Indent(2));
                        SbBll.AppendLine("/// <returns>Object dictionary, the key is the name of the field(<![CDATA[Dictionary<string, object>]]>).</returns>".Indent(2));
                    }

                    SbBll.AppendLine("public Dictionary<string, object> ToDictionary()".Indent(2));
                    SbBll.AppendLine("{".Indent(2));
                    SbBll.AppendLine("Dictionary<string, object> Dict = new Dictionary<string, object>();".Indent(3));
                    for (int i = 0; i < curTable.Childs.Count; i++)
                    {
                        Field F = curTable.Childs[i] as Field;
                        if (F != null)
                            SbBll.AppendLine(String.Format("Dict.Add(\"{0}\", {0} == {1} ? DBNull.Value : (object){0});".Indent(3),F.Name, F.DefaultValue));
                    }
                    SbBll.AppendLine("return Dict;".Indent(3));
                    SbBll.AppendLine("}".Indent(2));
                }
                #endregion

                SbBll.AppendLine("#endregion".Indent(2));
            }
            #endregion

            #region Indexers of the class, by field number(position on the table) and by field name(case insensitive)

            if (settings.IntegerIndexer || settings.StringIndexer)
            {
                SbBll.AppendLine("#region Indexers of the class, by field number(position on the table) and by field name(case insensitive)".Indent(2));

                #region Integer indexer
                if (settings.IntegerIndexer)
                {
                    if (settings.IncludeComments)
                    {
                        SbBll.AppendLine("/// <summary>".Indent(2));
                        SbBll.AppendLine("/// Gets or sets the value of the field with an \"index == i\" as an object.".Indent(2));
                        SbBll.AppendLine("/// </summary>".Indent(2));
                    }

                    SbBll.AppendLine("public object this[int i]".Indent(2));
                    SbBll.AppendLine("{".Indent(2));
                    SbBll.AppendLine("get".Indent(3));
                    SbBll.AppendLine("{".Indent(3));
                    SbBll.AppendLine(String.Format("if(i >= 0 && i < {0})".Indent(4), curTable.Childs.Count.ToString()));
                    SbBll.AppendLine("{".Indent(4));
                    SbBll.AppendLine("switch(i)".Indent(5));
                    SbBll.AppendLine("{".Indent(5));
                    for (int i = 0; i < curTable.Childs.Count; i++)
                    {
                        Field F = curTable.Childs[i] as Field;
                        if (F != null)
                        {
                            SbBll.AppendLine(String.Format("case {0}:".Indent(6), i.ToString()));
                            SbBll.AppendLine(String.Format("return {0};".Indent(7), F.Name));
                        }
                    }
                    SbBll.AppendLine("}".Indent(5));
                    SbBll.AppendLine("}".Indent(4));
                    SbBll.AppendLine("return null;".Indent(4));
                    SbBll.AppendLine("}".Indent(3));


                    SbBll.AppendLine("set".Indent(3));
                    SbBll.AppendLine("{".Indent(3));
                    SbBll.AppendLine(String.Format("if(i >= 0 && i < {0})".Indent(4), curTable.Childs.Count.ToString()));
                    SbBll.AppendLine("{".Indent(4));
                    SbBll.AppendLine("switch(i)".Indent(5));
                    SbBll.AppendLine("{".Indent(5));
                    for (int i = 0; i < curTable.Childs.Count; i++)
                    {
                        Field F = curTable.Childs[i] as Field;
                        if (F != null)
                        {
                            SbBll.AppendLine(String.Format("case {0}:".Indent(6), i.ToString()));
                            SbBll.AppendLine(String.Format("if(value is {0})".Indent(7), F.CSharpType));
                            SbBll.AppendLine("{".Indent(7));
                            SbBll.AppendLine(String.Format("{0} = ({1})value;".Indent(8), F.Name, F.CSharpType));
                            SbBll.AppendLine("}".Indent(7));
                            SbBll.AppendLine("else".Indent(7));
                            SbBll.AppendLine("{".Indent(7));
                            SbBll.AppendLine("try".Indent(8));
                            SbBll.AppendLine("{".Indent(8));
                            SbBll.AppendLine(String.Format("{0} = ({1})value;".Indent(9), F.Name, F.CSharpType));
                            SbBll.AppendLine("}".Indent(8));
                            SbBll.AppendLine("catch(Exception ex)".Indent(8));
                            SbBll.AppendLine("{".Indent(8));
                            SbBll.AppendLine(String.Format("throw new Exception(\"Data type mismatch for the field: {0}/index: {1}\", ex);".Indent(9), F.Name, i));
                            SbBll.AppendLine("}".Indent(8));
                            SbBll.AppendLine("}".Indent(7));
                            SbBll.AppendLine("break;".Indent(7));
                        }
                    }
                    SbBll.AppendLine("}".Indent(5));
                    SbBll.AppendLine("}".Indent(4));
                    SbBll.AppendLine("}".Indent(3));
                    SbBll.AppendLine("}".Indent(2));
                }
                #endregion

                #region String indexer
                if (settings.StringIndexer)
                {
                    if (settings.IncludeComments)
                    {
                        SbBll.AppendLine("/// <summary>".Indent(2));
                        SbBll.AppendLine("/// Gets or sets the value of the field whose name is: \"fieldName\" as an object (case insensitive.)".Indent(2));
                        SbBll.AppendLine("/// </summary>".Indent(2));
                    }

                    //string aux = field.ToLower();
                    SbBll.AppendLine("public object this[string fieldName]".Indent(2));
                    SbBll.AppendLine("{".Indent(2));

                    SbBll.AppendLine("get".Indent(3));
                    SbBll.AppendLine("{".Indent(3));
                    SbBll.AppendLine("string aux = fieldName.ToLower();".Indent(4));
                    SbBll.AppendLine("switch(aux)".Indent(4));
                    SbBll.AppendLine("{".Indent(4));
                    for (int i = 0; i < curTable.Childs.Count; i++)
                    {
                        Field F = curTable.Childs[i] as Field;
                        if (F != null)
                        {
                            SbBll.AppendLine(String.Format("case \"{0}\":".Indent(5), F.Name.ToLower()));
                            SbBll.AppendLine(String.Format("return {0};".Indent(6), F.Name));
                        }
                    }
                    SbBll.AppendLine("}".Indent(4));
                    SbBll.AppendLine("return null;".Indent(4));
                    SbBll.AppendLine("}".Indent(3));

                    SbBll.AppendLine("set".Indent(3));
                    SbBll.AppendLine("{".Indent(3));
                    SbBll.AppendLine("string aux = fieldName.ToLower();".Indent(4));
                    SbBll.AppendLine("switch(aux)".Indent(4));
                    SbBll.AppendLine("{".Indent(4));
                    for (int i = 0; i < curTable.Childs.Count; i++)
                    {
                        Field F = curTable.Childs[i] as Field;
                        if (F != null)
                        {
                            SbBll.AppendLine(String.Format("case \"{0}\":".Indent(5), F.Name.ToLower()));
                            SbBll.AppendLine(String.Format("if(value is {0})".Indent(6), F.CSharpType));
                            SbBll.AppendLine("{".Indent(6));
                            SbBll.AppendLine(String.Format("{0} = ({1}) value;".Indent(6), F.Name, F.CSharpType));
                            SbBll.AppendLine("}".Indent(6));
                            SbBll.AppendLine("else".Indent(6));
                            SbBll.AppendLine("{".Indent(6));
                            SbBll.AppendLine("try".Indent(7));
                            SbBll.AppendLine("{".Indent(7));
                            SbBll.AppendLine(String.Format("{0} = ({1})value;".Indent(8), F.Name, F.CSharpType));
                            SbBll.AppendLine("}".Indent(7));
                            SbBll.AppendLine("catch(Exception ex)".Indent(7));
                            SbBll.AppendLine("{".Indent(7));
                            SbBll.AppendLine(String.Format("throw new Exception(\"Data type mismatch for the field: {0} - index: {0}\", ex);".Indent(8), F.Name, i));
                            SbBll.AppendLine("}".Indent(7));
                            SbBll.AppendLine("}".Indent(6));
                            SbBll.AppendLine("break;".Indent(6));
                        }
                    }
                    SbBll.AppendLine("}".Indent(4));
                    SbBll.AppendLine("}".Indent(3));
                    SbBll.AppendLine("}".Indent(2));
                }
                #endregion

                SbBll.AppendLine("#endregion".Indent(2));
            }

            #endregion

            #region Field type method
            if (settings.FieldType)
            {
                SbBll.AppendLine("#region FieldType Method".Indent(2));
                SbBll.AppendLine("public string FieldType(string fieldName)".Indent(2));
                SbBll.AppendLine("{".Indent(2));
                SbBll.AppendLine("switch(fieldName.ToLower())".Indent(3));
                SbBll.AppendLine("{".Indent(3));
                for (int i = 0; i < curTable.Childs.Count; i++)
                {
                    Field F = curTable.Childs[i] as Field;
                    if (F == null || !QueryForm.SqlVsCSharp.ContainsKey(F.Type))
                        continue;
                    
                    SbBll.AppendLine(String.Format("case \"{0}\":".Indent(4), F.Name.ToLower()));
                    SbBll.AppendLine(String.Format("return \"{0}\";".Indent(5), QueryForm.SqlVsCSharp[F.Type].ToLower()));
                }
                SbBll.AppendLine("}".Indent(3));
                SbBll.AppendLine("return \"string\";".Indent(3));
                SbBll.AppendLine("}".Indent(2));
                SbBll.AppendLine("#endregion".Indent(2));
            }
            #endregion

            SbBll.AppendLine("}".Indent());

            return SbBll.ToString();
        }
        public string GenerateCSharpCode { get { return GenerateCSharpClassFromTable((ISqlObject) BaseSQLTable ?? BaseSQLView, DialogSettings, txtClassName.Text); }  }

        public GenerateClass()
        {
            InitializeComponent();
            LoadSettings();
            PocoGenerator = false;
        }

        public GenerateClass(Table table)
        {
            InitializeComponent();
            BaseSQLTable = table;
            LoadSettings();
            PocoGenerator = true;
            txtClassName.Text = table.Name;
        }
        public GenerateClass(Ez_SQL.DataBaseObjects.View view)
        {
            InitializeComponent();
            BaseSQLView = view;
            LoadSettings();
            PocoGenerator = true;
            txtClassName.Text = view.Name;
        }

        private void LoadSettings()
        {
            GenerateClassModelSettings settings = null;
            if (File.Exists(SettingsFileName))
            {
                settings = SettingsFileName.DeserializeFromXmlFile() as GenerateClassModelSettings;
            }
            
            if(settings == null)
            {
                settings  = new GenerateClassModelSettings()
                    {
                        AutoImplementedProperties = false,
                        DataMemberDecoration = false,
                        FieldCount = false,
                        IntegerIndexer = false,
                        PkAlias = false,
                        PkConstructor = false,
                        StaticDataTableConverters = false,
                        StringIndexer = false,
                        ToObjectArray = false,
                        ToObjectDictionary = false,
                        ToObjectList = false,
                        FieldType = false,
                        IncludeComments = false
                    };
                settings.SerializeToXmlFile(SettingsFileName);
            }

            radAutoImplementedProperties.Checked = settings.AutoImplementedProperties;
            radVariableProperties.Checked = !settings.AutoImplementedProperties;
            chkDataMemberDecoration.Checked = settings.DataMemberDecoration;
            chkFieldCount.Checked = settings.FieldCount;
            chkFieldType.Checked = settings.FieldType;
                
            chkIntegerIndexer.Checked = settings.IntegerIndexer;
            chkStringIndexer.Checked = settings.StringIndexer;
            chkIndexers.Checked = settings.StringIndexer || settings.IntegerIndexer;

            chkPKAlias.Checked = settings.PkAlias;
            chkPKConstructor.Checked = settings.PkConstructor;
            chkLoadFromDataTable.Checked = settings.StaticDataTableConverters;

            chkObjectArray.Checked = settings.ToObjectArray;
            chkObjectList.Checked = settings.ToObjectList;
            chkDictionary.Checked = settings.ToObjectDictionary;
            chkInstanceConverters.Checked = settings.ToObjectArray || settings.ToObjectList || settings.ToObjectDictionary;
            chkIncludeComments.Checked = settings.IncludeComments;

            GenerateFieldOptions();
        }

        private void GenerateFieldOptions()
        {
            List<Field> fields;
            if (BaseSQLView != null)
            {
                fields = BaseSQLView.Childs.Select(x => x as Field).ToList();
            }
            else
            {
                fields = BaseSQLTable.Childs.Select(x => x as Field).ToList();
            }

            int index = 0, initialY = 10, yStep = 31, tabIndex = 17;
            foreach (Field f in fields)
            {
                Label labFieldName;
                CheckBox chkInclude, chkKey, chkRequired, chkLength;
                NumericUpDown numLength;
                TextBox txtDisplay;

                labFieldName = new Label();
                panelFields.Controls.Add(labFieldName);
                labFieldName.AutoSize = true;
                labFieldName.Location = new System.Drawing.Point(9, (initialY + index * yStep) - 4);
                labFieldName.Name = f.Name + "_FieldName";
                labFieldName.Size = new System.Drawing.Size(46, 18);
                labFieldName.TabIndex = tabIndex;
                labFieldName.Text = f.Name;
                tabIndex++;

                chkInclude = new CheckBox();
                panelFields.Controls.Add(chkInclude);
                chkInclude.AutoSize = true;
                chkInclude.Location = new System.Drawing.Point(147, initialY + index * yStep);
                chkInclude.Name = f.Name + "_chkInclude";
                chkInclude.Size = new System.Drawing.Size(15, 14);
                chkInclude.TabIndex = tabIndex;
                chkInclude.Text = "";
                chkInclude.UseVisualStyleBackColor = true;
                chkInclude.Checked = true;
                tabIndex++;

                chkKey = new CheckBox();
                panelFields.Controls.Add(chkKey);
                chkKey.AutoSize = true;
                chkKey.Location = new System.Drawing.Point(189, initialY + index * yStep);
                chkKey.Name = f.Name + "_chkKey";
                chkKey.Size = new System.Drawing.Size(15, 14);
                chkKey.TabIndex = tabIndex;
                chkKey.Text = "";
                chkKey.UseVisualStyleBackColor = true;
                chkKey.Checked = f.IsPrimaryKey;
                tabIndex++;
                
                chkRequired = new CheckBox();
                panelFields.Controls.Add(chkRequired);
                chkRequired.AutoSize = true;
                chkRequired.Location = new System.Drawing.Point(236, initialY + index * yStep);
                chkRequired.Name = f.Name + "_chkRequired";
                chkRequired.Size = new System.Drawing.Size(15, 14);
                chkRequired.TabIndex = tabIndex;
                chkRequired.Text = "";
                chkRequired.UseVisualStyleBackColor = true;
                tabIndex++;

                chkLength = new CheckBox();
                panelFields.Controls.Add(chkLength);
                chkLength.AutoSize = true;
                chkLength.Location = new System.Drawing.Point(292, initialY + index * yStep);
                chkLength.Name = f.Name + "_chkLength";
                chkLength.Size = new System.Drawing.Size(15, 14);
                chkLength.TabIndex = tabIndex;
                chkLength.Text = "";
                chkLength.UseVisualStyleBackColor = true;
                tabIndex++;

                txtDisplay = new TextBox();
                panelFields.Controls.Add(txtDisplay);
                txtDisplay.Location = new System.Drawing.Point(324, (initialY + index * yStep) - 8);
                txtDisplay.Name = f.Name + "_txtDisplay";
                txtDisplay.Size = new System.Drawing.Size(236, 26);
                txtDisplay.TabIndex = tabIndex;
                tabIndex++;
                
                index++;
            }

        }

        private void SaveSettings()
        {
            GenerateClassModelSettings CurrentSettings = DialogSettings;
            CurrentSettings.SerializeToXmlFile(SettingsFileName);
        }

        private string DefaultValueFor(string Type)
        {
            string back = "0";
            switch (Type.ToLower())
            {
                case "string":
                    back = "String.Empty";
                    break;
                case "int":
                    back = "0";
                    break;
                case "float":
                case "double":
                    back = "0";
                    break;
                case "bool":
                case "boolean":
                    back = "false";
                    break;
                case "datetime":
                case "datetime2":
                    back = "new DateTime()";
                    break;
            }
            return back;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            SaveSettings();
            DialogResult = DialogResult.OK;
        }

        private void chkObjectArray_CheckedChanged(object sender, EventArgs e)
        {
            chkInstanceConverters.Checked = chkObjectArray.Checked || chkObjectList.Checked || chkDictionary.Checked;
        }

        private void chkIntegerIndexer_CheckedChanged(object sender, EventArgs e)
        {
            chkIndexers.Checked = chkIntegerIndexer.Checked || chkStringIndexer.Checked;
        }


    }
}
