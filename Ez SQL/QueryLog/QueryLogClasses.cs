using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL.QueryLog
{
    /// <summary>
    /// A deserialized representation of a single query log entry as loaded from the XML log file.
    /// Used by <see cref="HistoricForm"/> to display history. Contains the connection context,
    /// execution metrics, the SQL script, and any errors raised during execution.
    /// </summary>
    public class QueryInfo
    {
        /// <summary>Gets or sets the connection details associated with this query execution.</summary>
        public ConnectionInfo Conx { get { return _Conx; } set { _Conx = value; } }
        private ConnectionInfo _Conx;

        /// <summary>Gets or sets the execution timing and result metrics for this query.</summary>
        public ExecutionInfo Exec { get { return _Exec; } set { _Exec = value; } }
        private ExecutionInfo _Exec;

        /// <summary>Gets or sets the list of SQL errors raised during execution, if any.</summary>
        public List<ErrorInfo> Mistakes { get { return _Mistakes; } set { _Mistakes = value; } }
        public List<ErrorInfo> _Mistakes;

        /// <summary>Gets or sets the SQL script that was executed.</summary>
        public string Script { get { return _Script; } set { _Script = value; } }
        private string _Script;

        /// <summary>Gets or sets the unique integer key assigned when loading the log (used for grid selection).</summary>
        public int Key { get { return _Key; } set { _Key = value; } }
        private int _Key;

        /// <summary>Initializes a new empty <see cref="QueryInfo"/> with default-constructed sub-objects.</summary>
        public QueryInfo()
        {
            Conx = new ConnectionInfo();
            Exec = new ExecutionInfo();
            Mistakes = new List<ErrorInfo>();
            Script = "";
        }

        /// <summary>Initializes a new <see cref="QueryInfo"/> with all fields explicitly provided.</summary>
        public QueryInfo(ConnectionInfo Conx, ExecutionInfo Exec, List<ErrorInfo> Mistakes, string Script)
        {
            this.Conx = Conx;
            this.Exec = Exec;
            this.Mistakes = Mistakes;
            this.Script = Script;
        }
    }

    /// <summary>
    /// Holds the connection context (group, name, server, database) for a query log entry.
    /// Used within <see cref="QueryInfo"/> to capture which connection was active when the query ran.
    /// Note: this is a separate <c>ConnectionInfo</c> from <see cref="Ez_SQL.ConnectionManagement.ConnectionInfo"/>.
    /// </summary>
    public class ConnectionInfo
    {
        /// <summary>Gets or sets the connection group name.</summary>
        public string Group { get { return _Group; } set { _Group = value; } }
        private string _Group;

        /// <summary>Gets or sets the connection friendly name.</summary>
        public string Name { get { return _Name; } set { _Name = value; } }
        private string _Name;

        /// <summary>Gets or sets the SQL Server instance name.</summary>
        public string Server { get { return _Server; } set { _Server = value; } }
        private string _Server;

        /// <summary>Gets or sets the database name.</summary>
        public string Db { get { return _Db; } set { _Db = value; } }
        private string _Db;

        /// <summary>Initializes a new empty <see cref="ConnectionInfo"/>.</summary>
        public ConnectionInfo() { }

        /// <summary>Initializes a new <see cref="ConnectionInfo"/> with all fields provided.</summary>
        public ConnectionInfo(string Group, string Name, string Server, string Db)
        {
            this.Group = Group;
            this.Name = Name;
            this.Server = Server;
            this.Db = Db;
        }
    }

    /// <summary>
    /// Holds the execution metrics for a single query log entry: start/end timestamps,
    /// elapsed milliseconds, success flag, rows affected, rows read, and result-set count.
    /// </summary>
    public class ExecutionInfo
    {
        /// <summary>Gets or sets the time the query began executing.</summary>
        public DateTime StartTime { get { return _StartTime; } set { _StartTime = value; } }
        private DateTime _StartTime;

        /// <summary>Gets or sets the time the query finished executing.</summary>
        public DateTime EndTime { get { return _EndTime; } set { _EndTime = value; } }
        private DateTime _EndTime;

        /// <summary>Gets or sets the elapsed execution time in milliseconds.</summary>
        public int Lapse { get { return _Lapse; } set { _Lapse = value; } }
        private int _Lapse;

        /// <summary>Gets or sets whether the query executed without errors (1 = success, 0 = error).</summary>
        public int Correct { get { return _Correct; } set { _Correct = value; } }
        private int _Correct;

        /// <summary>Gets or sets the number of rows affected by the command (for non-query statements).</summary>
        public int RecordsAffected { get { return _RecordsAffected; } set { _RecordsAffected = value; } }
        private int _RecordsAffected;

        /// <summary>Gets or sets the total number of rows returned across all result sets.</summary>
        public int RecordsRead { get { return _RecordsRead; } set { _RecordsRead = value; } }
        private int _RecordsRead;

        /// <summary>Gets or sets the number of DataGridView result grids produced by the query.</summary>
        public int GridCount { get { return _GridCount; } set { _GridCount = value; } }
        private int _GridCount;

        /// <summary>Initializes a new empty <see cref="ExecutionInfo"/>.</summary>
        public ExecutionInfo() { }

        /// <summary>Initializes a new <see cref="ExecutionInfo"/> with all fields provided.</summary>
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

    /// <summary>
    /// Represents a single SQL error entry from the log: the line number where the error occurred
    /// and the error message text.
    /// </summary>
    public class ErrorInfo
    {
        /// <summary>Gets or sets the 1-based line number where the SQL error was raised.</summary>
        public int Line { get { return _Line; } set { _Line = value; } }
        private int _Line;

        /// <summary>Gets or sets the error message text.</summary>
        public string Message { get { return _Message; } set { _Message = value; } }
        private string _Message;

        /// <summary>Initializes a new empty <see cref="ErrorInfo"/>.</summary>
        public ErrorInfo() { }

        /// <summary>Initializes a new <see cref="ErrorInfo"/> with the given line and message.</summary>
        public ErrorInfo(int Line, string Message)
        {
            this.Line = Line;
            this.Message = Message;
        }
    }

    /// <summary>
    /// The live record written to the XML log file while a query is executing.
    /// Uses <see cref="ToString()"/> to serialize itself to a <c>&lt;Query&gt;</c> XML fragment
    /// that is appended to <c>LogFile.xml</c>. Text content is XML-entity-encoded via
    /// <see cref="ValidXmlText"/> to prevent malformed XML.
    /// </summary>
    public class QueryRecord
    {
        /// <summary>Gets or sets a sequential identifier for this query record.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the connection group name.</summary>
        public string Group { get; set; }
        /// <summary>Gets or sets the connection friendly name.</summary>
        public string Name { get; set; }
        /// <summary>Gets or sets the SQL Server instance name.</summary>
        public string Server { get; set; }
        /// <summary>Gets or sets the database name.</summary>
        public string Db { get; set; }
        /// <summary>Gets or sets the time the query began executing.</summary>
        public DateTime StartTime { get; set; }
        /// <summary>Gets or sets the time the query finished executing.</summary>
        public DateTime EndTime { get; set; }
        /// <summary>Gets or sets the elapsed execution time in milliseconds.</summary>
        public int Lapse { get; set; }
        /// <summary>Gets or sets whether the query executed without errors (1 = success, 0 = error).</summary>
        public int Correct { get; set; }
        /// <summary>Gets or sets the number of rows affected by the command.</summary>
        public int RecordsAffected { get; set; }
        /// <summary>Gets or sets the total number of rows returned across all result sets.</summary>
        public int RecordsRead { get; set; }
        /// <summary>Gets or sets the number of result grids produced.</summary>
        public int GridCount { get; set; }
        /// <summary>Gets or sets the SQL script that was executed.</summary>
        public string Code { get; set; }
        private List<ErrorRecord> _Errors;
        /// <summary>
        /// Initializes a new <see cref="QueryRecord"/> for the given connection context and SQL script.
        /// Sets <see cref="StartTime"/> to <c>DateTime.Now</c> and creates an empty error list.
        /// </summary>
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
                StartTime.ToString("yyyyMMdd HH:mm:ss.fff"),
                EndTime.ToString("yyyyMMdd HH:mm:ss.fff"),
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
        /// <summary>Appends a new <see cref="ErrorRecord"/> to this record's error list.</summary>
        public void AddError(int line, string message)
        {
            _Errors.Add(new ErrorRecord(line, message));
        }
        /// <summary>Returns <c>true</c> if at least one error was recorded during execution.</summary>
        public bool HasErrors()
        {
            return _Errors.Count > 0;
        }
        /// <summary>
        /// Encodes special XML characters in <paramref name="Text"/> to entity references
        /// (<c>&lt;</c>, <c>&gt;</c>, <c>&quot;</c>, <c>&apos;</c>, <c>&amp;</c>)
        /// so the text can be safely embedded in XML attributes or elements.
        /// </summary>
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
        /// <summary>Reverses the XML entity encoding applied by <see cref="ValidXmlText"/>.</summary>
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
    /// <summary>
    /// A write-time error record appended to a <see cref="QueryRecord"/> while a query executes.
    /// Contains the line number and error message of a single SQL error.
    /// </summary>
    public class ErrorRecord
    {
        /// <summary>Gets or sets the 1-based line number where the SQL error was raised.</summary>
        public int Line { get; set; }

        /// <summary>Gets or sets the SQL error message text.</summary>
        public string Message { get; set; }

        /// <summary>Initializes a new empty <see cref="ErrorRecord"/>.</summary>
        public ErrorRecord() { }

        /// <summary>Initializes a new <see cref="ErrorRecord"/> with the given line and message.</summary>
        public ErrorRecord(int Line, string Message)
        {
            this.Line = Line;
            this.Message = Message;
        }
    }

}
