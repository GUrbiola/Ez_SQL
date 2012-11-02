using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using System.Data.SqlClient;

namespace Ez_SQL.DataBaseObjects
{
    public class Scheme : ISqlObject
    {
        public Scheme()
        {
            Childs = new List<ISqlChild>();
            _Kind = ObjectType.Schema;
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
            return;
        }
        public bool IsScriptLoaded { get { return true; } }

        public string Description
        {
            get { return String.Format("Database schema: {0}", Schema); }
        }
        public int ImageIndex
        {
            get { return 0; }
        }
        public bool InsertAction(ICSharpCode.TextEditor.TextArea textArea, char ch)
        {
            textArea.InsertString(String.Format("{0}", Name));
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
