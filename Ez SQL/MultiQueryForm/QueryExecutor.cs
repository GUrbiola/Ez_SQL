using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using System.Data;

namespace Ez_SQL.MultiQueryForm
{
    public delegate void ProcessingQuery(string Query, DateTime Hora);
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
        public List<string> _Messages;
        public List<string> Messages
        {
            get { return _Messages; }
            set { _Messages = value; }
        }
        private string _LastMessage;
        public string LastMessage
        {
            get { return _LastMessage; }
            set { _LastMessage = value; }
        }
        private SqlConnection _Connection;
        public SqlConnection Connection
        {
            get { return _Connection; }
            set { _Connection = value; }
        }
        private Exception _NrEx;
        public Exception NrEx
        {
            get { return _NrEx; }
            set { _NrEx = value; }
        }
        public SqlException _SqlEx;
        public SqlException SqlEx
        {
            get { return _SqlEx; }
            set { _SqlEx = value; }
        }
        private DataSet _Results;
        public DataSet Results
        {
            get { return _Results; }
            set { _Results = value; }
        }
        private int _TimeOut;
        public int TimeOut
        {
            get { return _TimeOut; }
            set { _TimeOut = value; }
        }
        private int _AsyncResult;
        public int AsyncResult
        {
            get { return _AsyncResult; }
            set { _AsyncResult = value; }
        }

        public event ProcessingQuery StartExec;
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
        public void CancelExecute()
        {
            _CancelExecution = true;
        }
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
