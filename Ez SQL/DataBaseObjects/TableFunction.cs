using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Ez_SQL.DataBaseObjects
{
    public class TableFunction : ISqlObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private ObjectType _Kind;
        public ObjectType Kind { get { return _Kind; } }
        private string _Script;
        public string Script { get { return _Script; } }
        public void LoadScript(SqlCommand cmd)
        {
            SqlDataReader rdr = null;

            string sql_view = "select text from syscomments where id = @DBId order by colid";
            sql_view = sql_view.Replace("@DBId", Id.ToString());
            try
            {
                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                    cmd.Connection.Open();
                cmd.CommandText = sql_view;
                rdr = cmd.ExecuteReader();
                StringBuilder sc1 = new StringBuilder();
                while (rdr.Read())
                    sc1.Append(rdr[0].ToString());
                _Script = sc1.ToString();
            }
            catch (Exception ex)
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                rdr.Dispose();
                if (cmd != null && cmd.Connection != null && cmd.Connection.State == System.Data.ConnectionState.Open)
                    cmd.Connection.Close();
                cmd.Dispose();
                _Script = "";
                throw ex;
            }
            finally
            {
                rdr.Close();
                rdr.Dispose();
                cmd.Connection.Close();
                cmd.Dispose();
            }
        }
        public string Schema { get; set; }
        public string Comment { get; set; }
        public List<ISqlChild> Childs { get; set; }
        public TableFunction()
        {
            Childs = new List<ISqlChild>();
            _Kind = ObjectType.TableFunction;
            _Script = "";
        }
        public bool IsScriptLoaded { get { return !String.IsNullOrEmpty(_Script); } }


        public string Description
        {
            get { return String.Format("Database table function: {0}, schema: {1}", Name, Schema); }
        }
        public int ImageIndex
        {
            get { return 9; }
        }
        public bool InsertAction(ICSharpCode.TextEditor.TextArea textArea, char ch)
        {
            string ToWrite;
            if (ch == '1')
                ToWrite = String.Format("{0} (", Name);
            else
                ToWrite = String.Format("{0}.{1} (", Schema, Name);

            foreach (ISqlChild Ch in Childs.Where(X => X.Kind == ChildType.Parameter))
                ToWrite += String.Format("{0} /*{1}*/, ", Ch.Name, Ch.Type, Environment.NewLine);
            if (ToWrite.EndsWith(", "))
                ToWrite = ToWrite.Substring(0, ToWrite.Length - 2);

            textArea.InsertString(ToWrite + ")");
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
