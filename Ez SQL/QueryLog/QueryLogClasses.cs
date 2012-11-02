using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL.QueryLog
{
    public class QueryInfo
    {
        private ConnectionInfo _Conx;
        public ConnectionInfo Conx { get { return _Conx; } set { _Conx = value; } }
        private ExecutionInfo _Exec;
        public ExecutionInfo Exec { get { return _Exec; } set { _Exec = value; } }
        public List<ErrorInfo> _Mistakes;
        public List<ErrorInfo> Mistakes { get { return _Mistakes; } set { _Mistakes = value; } }
        private string _Script;
        public string Script { get { return _Script; } set { _Script = value; } }
        private int _Key;
        public int Key { get { return _Key; } set { _Key = value; } }
        public QueryInfo()
        {
            Conx = new ConnectionInfo();
            Exec = new ExecutionInfo();
            Mistakes = new List<ErrorInfo>();
            Script= "";
        }
        public QueryInfo(ConnectionInfo Conx, ExecutionInfo Exec, List<ErrorInfo> Mistakes, string Script)
        {
            this.Conx = Conx;
            this.Exec = Exec;
            this.Mistakes = Mistakes;
            this.Script = Script;
        }
    }
    public class ConnectionInfo
    {
        private string _Group;
        public string Group { get { return _Group; } set { _Group = value; } }
        private string _Name;
        public string Name { get { return _Name; } set { _Name = value; } }
        private string _Server;
        public string Server { get { return _Server; } set { _Server = value; } }
        private string _Db;
        public string Db { get { return _Db; } set { _Db = value; } }
        public ConnectionInfo()
        {

        }
        public ConnectionInfo(string Group, string Name, string Server, string Db)
        {
            this.Group = Group;
            this.Name = Name;
            this.Server = Server;
            this.Db = Db;
        }
    }
    public class ExecutionInfo
    {
        private DateTime _StartTime;
        public DateTime StartTime { get { return _StartTime; } set { _StartTime = value; } }
        private DateTime _EndTime;
        public DateTime EndTime { get { return _EndTime; } set { _EndTime = value; } }
        private int _Lapse;
        public int Lapse { get { return _Lapse; } set { _Lapse = value; } }
        private int _Correct;
        public int Correct { get { return _Correct; } set { _Correct = value; } }
        private int _RecordsAffected;
        public int RecordsAffected { get { return _RecordsAffected; } set { _RecordsAffected = value; } }
        private int _RecordsRead;
        public int RecordsRead { get { return _RecordsRead; } set { _RecordsRead = value; } }
        private int _GridCount;
        public int GridCount { get { return _GridCount; } set { _GridCount = value; } }
        public ExecutionInfo()
        {

        }
        public ExecutionInfo(DateTime StartTime, DateTime EndTime, int Lapse, int Correct, int RecordsAffected, int RecordsRead, int GridCount)
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;
            this.Lapse = Lapse;
            this.Correct = Correct;
            this.RecordsAffected = RecordsAffected;
            this.RecordsRead = RecordsRead;
            this.GridCount = GridCount;
        }
    }
    public class ErrorInfo
    {
        private int _Line;
        public int Line { get { return _Line; } set { _Line = value; } }
        private string _Message;
        public string Message { get { return _Message; } set { _Message = value; } }
        public ErrorInfo()
        {
        }
        public ErrorInfo(int Line, string Message)
        {
            this.Line = Line;
            this.Message = Message;
        }
    }
    public class QueryRecord
    {
        public int Id { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public string Db { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Lapse { get; set; }
        public int Correct { get; set; }
        public int RecordsAffected { get; set; }
        public int RecordsRead { get; set; }
        public int GridCount { get; set; }
        public string Code { get; set; }
        private List<ErrorRecord> _Errors;
        public QueryRecord(string Group, string Name, string Server, string Db, string Code)
        {
            this.Group = Group;
            this.Name = Name;
            this.Server = Server;
            this.Db = Db;
            this.Code = Code;
            this.StartTime = DateTime.Now;
            this._Errors = new List<ErrorRecord>();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t<Query>");
            //datos de conexion
            sb.AppendFormat("{0}{0}<Connection Group=\"{2}\" Name=\"{3}\" Server=\"{4}\" Db=\"{5}\" />{1}",
                "\t",
                Environment.NewLine,
                Group,
                Name,
                Server,
                Db
                );
            //datos de ejecucion
            sb.AppendFormat("{0}{0}<Execution StartTime=\"{2}\" EndTime=\"{3}\" Lapse=\"{4}\" Correct=\"{5}\" RecordsAffected=\"{6}\" RecordsRead=\"{7}\" GridCount=\"{8}\" />{1}",
                "\t",
                Environment.NewLine,
                StartTime.ToString("yyyyMMdd hh:mm:ss.fff"),
                EndTime.ToString("yyyyMMdd hh:mm:ss.fff"),
                Lapse,
                Correct,
                RecordsAffected,
                RecordsRead,
                GridCount
                );
            //luego el script que se ejecuto... o intento ejecutar
            sb.AppendFormat("{0}{0}<Code>\"{2}\"</Code>{1}", "\t", Environment.NewLine, ValidXmlText(Code));
            //por ultimo los datos de los errores... si es que los hubo
            foreach (ErrorRecord er in _Errors)
                sb.AppendFormat("{0}{0}<Error Line=\"{2}\" Message=\"{3}\"/>{1}", "\t", Environment.NewLine, er.Line, ValidXmlText(er.Message));
            sb.AppendLine("\t</Query>");
            return sb.ToString();
        }
        public void AddError(int line, string message)
        {
            _Errors.Add(new ErrorRecord(line, message));
        }
        public bool HasErrors()
        {
            return _Errors.Count > 0;
        }
        public string ValidXmlText(string Text)
        {
            StringBuilder sb = new StringBuilder(Text);
            sb = sb.Replace("<", "&lt;");
            sb = sb.Replace(">", "&gt;");
            sb = sb.Replace("\"", "&quot;");
            sb = sb.Replace("'", "&apos;");
            sb = sb.Replace("&", "&amp;");
            return sb.ToString();
        }
        public string TextFromXml(string Text)
        {
            StringBuilder sb = new StringBuilder(Text);
            sb = sb.Replace("&lt;", "<");
            sb = sb.Replace("&gt;", ">");
            sb = sb.Replace("&quot;", "\"");
            sb = sb.Replace("&apos;", "'");
            sb = sb.Replace("&amp;", "&");
            return sb.ToString();
        }
    }
    public class ErrorRecord
    {
        public int Line { get; set; }
        public string Message { get; set; }
        public ErrorRecord()
        {
        }
        public ErrorRecord(int Line, string Message)
        {
            this.Line = Line;
            this.Message = Message;
        }
    }

}
