using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using System.Data;

namespace Ez_SQL.MultiQueryForm
{
    /// <summary>
    /// Delegate fired when a SQL query begins or finishes execution.
    /// </summary>
    /// <param name="Query">The SQL text being executed.</param>
    /// <param name="Hora">The timestamp at which the event fired.</param>
    public delegate void ProcessingQuery(string Query, DateTime Hora);

    /// <summary>
    /// Handles the execution of SQL queries against a SQL Server connection.
    /// Supports both synchronous (<see cref="ExecuteDataset"/>, <see cref="ExecuteTable"/>)
    /// and asynchronous (<see cref="AsyncExecuteDataSet"/>) execution on a background thread.
    /// Results are stored in <see cref="Results"/> and execution timing is exposed via <see cref="ExecutionLapse"/>.
    /// </summary>
    public class QueryExecutor
    {
        private DateTime Start, End;
        private bool _OnExecution;
        private Thread Executor;
        private SqlCommand cmd = null;
        private SqlDataAdapter da = null;
        private SqlDataReader rdr = null;
        private string curquery;
        private bool _CancelExecution;

        /// <summary>Gets or sets the list of informational messages received via <c>SqlConnection.InfoMessage</c> during execution.</summary>
        public List<string> _Messages;
        /// <inheritdoc cref="_Messages"/>
        public List<string> Messages
        {
            get { return _Messages; }
            set { _Messages = value; }
        }

        /// <summary>
        /// Gets or sets the last status message from the executor.
        /// Set to <c>"OK"</c> on success, or the exception message on failure.
        /// </summary>
        public string LastMessage
        {
            get { return _LastMessage; }
            set { _LastMessage = value; }
        }
        private string _LastMessage;

        /// <summary>Gets or sets the <see cref="SqlConnection"/> used to execute queries.</summary>
        public SqlConnection Connection
        {
            get { return _Connection; }
            set { _Connection = value; }
        }
        private SqlConnection _Connection;

        /// <summary>Gets or sets the last non-SQL exception thrown during execution.</summary>
        public Exception NrEx
        {
            get { return _NrEx; }
            set { _NrEx = value; }
        }
        private Exception _NrEx;

        /// <summary>Gets or sets the last <see cref="SqlException"/> thrown during execution.</summary>
        public SqlException _SqlEx;
        /// <inheritdoc cref="_SqlEx"/>
        public SqlException SqlEx
        {
            get { return _SqlEx; }
            set { _SqlEx = value; }
        }

        /// <summary>Gets or sets the <see cref="DataSet"/> containing all result sets from the last query execution.</summary>
        public DataSet Results
        {
            get { return _Results; }
            set { _Results = value; }
        }
        private DataSet _Results;

        /// <summary>Gets or sets the query command timeout in seconds (currently unused; the async path uses 0 for no timeout).</summary>
        public int TimeOut
        {
            get { return _TimeOut; }
            set { _TimeOut = value; }
        }
        private int _TimeOut;

        /// <summary>
        /// Gets or sets the result code of the last async execution:
        /// 0 = cancelled, 1 = success, -1 = error.
        /// </summary>
        public int AsyncResult
        {
            get { return _AsyncResult; }
            set { _AsyncResult = value; }
        }
        private int _AsyncResult;

        /// <summary>Raised when an asynchronous query execution begins.</summary>
        public event ProcessingQuery StartExec;
        /// <summary>Raised when an asynchronous query execution completes (success, error, or cancel).</summary>
        public event ProcessingQuery FinishExec;

        public string ConnectionString
        {
            get
            {
                if (Connection != null)
                    return Connection.ConnectionString;
                return "";
            }
            set
            {
                Connection.ConnectionString = value;
            }
        }
        public TimeSpan ExecutionLapse
        {
            get
            {
                if (Start <= End)
                    return End.Subtract(Start);
                return new TimeSpan(0);
            }
        }
        private bool Executing
        {
            get
            {
                return _OnExecution;
            }
            set
            {
                if (_OnExecution == value)
                    return;
                _OnExecution = value;
                if (value)
                    Start = DateTime.Now;
                else
                    End = DateTime.Now;
            }
        }
        public bool OnExecution
        {
            get { return Executing; }
        }
        public bool Error
        {
            get { return !LastMessage.Equals("OK", StringComparison.CurrentCultureIgnoreCase); }
        }
        public Exception LastException
        {
            get { return NrEx; }
        }
        public SqlException LastSqlException
        {
            get { return SqlEx; }
        }
        public string Server
        {
            get
            {
                if (Connection != null)
                    return Connection.DataSource;
                return "";
            }
        }
        public string DataBase
        {
            get
            {
                if (Connection != null)
                    return Connection.Database;
                return "";
            }
        }

        /// <summary>
        /// Initializes the executor with a new <see cref="SqlConnection"/> built from <paramref name="ConnectionString"/>.
        /// Clears all error state and attaches an <c>InfoMessage</c> handler to capture server print output.
        /// Must be called before any query execution methods.
        /// </summary>
        /// <param name="ConnectionString">The ADO.NET connection string to use for all queries.</param>
        public void Initialize(string ConnectionString)
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.InfoMessage += Connection_InfoMessage;
            Messages = new List<string>();
            NrEx = null;
            SqlEx = null;
            LastMessage = "OK";
            _OnExecution = false;
        }
        void Connection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            Messages.Add(String.Format("{0} - {1}", DateTime.Now.ToString("hh:mm:ss.fff"), e.Message));
        }
        /// <summary>
        /// Executes <paramref name="Query"/> asynchronously on a background thread.
        /// Results are stored in <see cref="Results"/> when the thread finishes.
        /// Raises <see cref="StartExec"/> immediately and <see cref="FinishExec"/> on completion.
        /// Does nothing if an execution is already in progress.
        /// </summary>
        /// <param name="Query">The SQL text to execute.</param>
        public void AsyncExecuteDataSet(string Query)
        {
            if (!String.IsNullOrEmpty(Query) && Query.Trim().Length > 0)
            {
                if (!OnExecution)
                {
                    Executor = new Thread(AsyncExecQuery);
                    curquery = Query;
                    NrEx = null;
                    SqlEx = null;
                    Executing = true;
                    _CancelExecution = false;
                    Messages.Clear();
                    if (StartExec != null)
                        StartExec(Query, DateTime.Now);
                    Executor.Start();
                }
                else
                {
                    LastMessage = "There ia already a query on execution, must wait until it ends.";
                }
            }
            else
            {
                LastMessage = "There is no script to execute.";
            }

        }
        /// <summary>
        /// Signals the currently running async query to stop reading rows at the next 1-second polling checkpoint.
        /// The query is not interrupted mid-row; use <see cref="ExtremeStop"/> for an immediate abort.
        /// </summary>
        public void CancelExecute()
        {
            _CancelExecution = true;
        }
        /// <summary>
        /// Immediately cancels the active <see cref="SqlCommand"/>, waits for the executor thread to finish,
        /// and fires <see cref="FinishExec"/>. Use this for a hard stop when <see cref="CancelExecute"/> is insufficient.
        /// </summary>
        public void ExtremeStop()
        {
            if (Executor.IsAlive)
            {
                if (cmd != null)
                {
                    try
                    {
                        cmd.Cancel();
                        cmd.Dispose();
                    }
                    catch (Exception)
                    {
                        ;
                    }
                }
                AsyncResult = 0;
                Executor.Join();
                Executing = false;
                if (FinishExec != null)
                    FinishExec("", DateTime.Now);
            }
        }
        /// <summary>
        /// Opens and immediately closes the connection to verify that it is reachable.
        /// Sets <see cref="NrEx"/> or <see cref="SqlEx"/> on failure.
        /// </summary>
        /// <returns><c>true</c> if the connection test succeeded; <c>false</c> otherwise.</returns>
        public bool TestConnection()
        {
            if (Connection != null && Connection.ConnectionString != null && Connection.ConnectionString.Length > 0)
            {
                try
                {
                    Connection.Open();
                }
                catch (SqlException sqlex)
                {
                    SqlEx = sqlex;
                    LastMessage = sqlex.Message;
                }
                catch (Exception ex)
                {
                    NrEx = ex;
                    LastMessage = ex.Message;
                }
                finally
                {
                    if (Connection.State == System.Data.ConnectionState.Open)
                        Connection.Close();
                }
                return !Error;
            }
            return true;
        }

        private void AsyncExecQuery()
        {
            DateTime LastCheck;
            Results = new DataSet();
            cmd = null;
            rdr = null;
            int indexxx;
            try
            {
                cmd = new SqlCommand(curquery, Connection);
                cmd.CommandTimeout = 0;
                cmd.Connection.Open();
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                indexxx = 0;
                LastCheck = DateTime.Now;
                do
                {
                    indexxx++;
                    // Create new data table
                    DataTable schemaTable = rdr.GetSchemaTable();
                    DataTable dataTable = new DataTable();
                    if (schemaTable != null)
                    {// A query returning records was executed
                        for (int i = 0; i < schemaTable.Rows.Count; i++)
                        {
                            DataRow dataRow = schemaTable.Rows[i];
                            // Create a column name that is unique in the data table
                            string columnName = (string)dataRow["ColumnName"]; //+ "<C" + i + "/>";
                            if (dataTable.Columns.Contains(columnName))
                            {
                                int index = 1;
                                foreach (DataColumn Col in dataTable.Columns)
                                    if (Col.ColumnName.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
                                        index++;
                                columnName += index.ToString();
                            }
                            // Add the column definition to the data table
                            DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                            dataTable.Columns.Add(column);
                        }
                        Results.Tables.Add(dataTable);
                        // Fill the data table we just created
                        while (rdr.Read())
                        {
                            DataRow dataRow = dataTable.NewRow();
                            for (int i = 0; i < rdr.FieldCount; i++)
                                dataRow[i] = rdr.GetValue(i);
                            dataTable.Rows.Add(dataRow);
                            if (DateTime.Now.Subtract(LastCheck) > new TimeSpan(0, 0, 1))
                            {
                                if (_CancelExecution)
                                {
                                    rdr.Close();
                                    LastMessage = "OK";
                                    AsyncResult = 0;
                                    break;
                                }
                                else
                                {
                                    LastCheck = DateTime.Now;
                                }
                            }
                        }

                        DataTable NonQ1 = new DataTable("NonQuery" + indexxx.ToString());
                        NonQ1.Columns.Add(new DataColumn("RowsAffected"));
                        DataRow DRx1 = NonQ1.NewRow();
                        DRx1[0] = Math.Max(rdr.RecordsAffected, 0);
                        NonQ1.Rows.Add(DRx1);
                        Results.Tables.Add(NonQ1);
                    }
                    else
                    {
                        // No records were returned
                        DataTable NonQ2 = new DataTable("NonQuery" + indexxx.ToString());
                        NonQ2.Columns.Add(new DataColumn("RowsAffected"));
                        DataRow DRx2 = NonQ2.NewRow();
                        DRx2[0] = Math.Max(rdr.RecordsAffected, 0);
                        NonQ2.Rows.Add(DRx2);
                        Results.Tables.Add(NonQ2);
                    }
                } while (rdr.NextResult());
                rdr.Close();
                LastMessage = "OK";
            }
            catch (SqlException sqlex)
            {
                AsyncResult = -1;
                LastMessage = sqlex.Message;
                SqlEx = sqlex;
            }
            catch (Exception ex)
            {
                AsyncResult = -1;
                LastMessage = ex.Message;
                NrEx = ex;
            }
            finally
            {
                Executing = false;
                if (rdr != null)
                {
                    if (!rdr.IsClosed)
                        rdr.Close();
                    rdr.Dispose();
                }
                if (cmd != null)
                    cmd.Dispose();
            }

            Executing = false;
            if (FinishExec != null)
                FinishExec(curquery, DateTime.Now);

            AsyncResult = 1;
        }

        /// <summary>
        /// Synchronously executes <paramref name="sql"/> and returns all result sets as a <see cref="DataSet"/>.
        /// Returns <c>null</c> if another query is already running.
        /// </summary>
        /// <param name="sql">The SQL query to execute.</param>
        /// <returns>A <see cref="DataSet"/> with one <see cref="System.Data.DataTable"/> per result set, or <c>null</c> on error.</returns>
        public DataSet ExecuteDataset(string sql)
        {
            DataSet dataSet = null;
            if (OnExecution)
            {
                LastMessage = "There ia already a query on execution, must wait until it ends.";
                return null;
            }

            try
            {
                da = new SqlDataAdapter();
                cmd = Connection.CreateCommand();
                cmd.Connection = Connection;
                cmd.CommandText = sql;

                dataSet = new DataSet();
                da.SelectCommand = cmd;
                Executing = true;
                da.Fill(dataSet);
                LastMessage = "OK";
            }
            catch (SqlException sqlex)
            {
                LastMessage = sqlex.Message;
                SqlEx = sqlex;
            }
            catch (Exception ex)
            {
                LastMessage = ex.Message;
                NrEx = ex;
            }
            finally
            {
                Executing = false;
                if (da != null)
                    da.Dispose();
                if (cmd != null)
                    cmd.Dispose();
            }
            return dataSet;
        }
        /// <summary>
        /// Convenience wrapper around <see cref="ExecuteDataset"/> that returns only the first result table.
        /// </summary>
        /// <param name="sql">The SQL query to execute.</param>
        /// <returns>The first <see cref="System.Data.DataTable"/> from the result set, or <c>null</c> if the query returned no tables.</returns>
        public DataTable ExecuteTable(string sql)
        {
            DataSet aux = null;
            DataTable result = null;

            aux = ExecuteDataset(sql);

            if (aux != null && aux.Tables.Count > 0)
                result = aux.Tables[0];

            return result;
        }
    }
}
