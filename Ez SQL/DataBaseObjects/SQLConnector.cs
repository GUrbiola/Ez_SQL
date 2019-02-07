using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Ez_SQL.Common_Code;
using Ez_SQL.Custom_Controls;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace Ez_SQL.DataBaseObjects
{
    public delegate void EndingLoad();
    public class SqlConnector
    {
        private bool _Loaded;
        public bool Loaded { get { return _Loaded; } }
        private bool _FullLoaded;
        public bool FullLoaded { get { return _FullLoaded; } }        
        private string _ConnectionString;
        public string ConnectionString 
        {
            get { return _ConnectionString; }
            set { _ConnectionString = value; }
        }
        public bool _IsBusy;
        public bool IsBusy 
        { 
            get { return _IsBusy; }
            set { _IsBusy = value; } 
        }
        private SqlConnection _Connection;
        public SqlConnection Connection 
        {
            get { return _Connection; }
            set { _Connection = value; }
        }
        public bool Busy
        {
            get
            {
                if (Loader == null || !Loader.IsBusy)
                    return false;
                return true;
            }
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

        public bool TablesLoaded { get; set; }
        public bool ViewsLoaded { get; set; }
        public bool SPsLoaded { get; set; }
        public bool TableFunctionsLoaded { get; set; }
        public bool ScalarFunctionsLoaded { get; set; }

        #region Loading dialog
        public event EndingLoad OnEndedLoad;
        LoadingInfo LoadDialog;
        BackgroundWorker Loader;
        PopupControl.Popup ISenseRect;
        #endregion
        public SqlConnector(string ConnectionString)
        {
            Connection = new SqlConnection(ConnectionString);
            this.ConnectionString = ConnectionString;
            
            TablesLoaded = false;
            ViewsLoaded = false;
            SPsLoaded = false;
            TableFunctionsLoaded = false;
            ScalarFunctionsLoaded = false;
        }
        private List<ISqlObject> _DbObjects = new List<ISqlObject>();
        public List<ISqlObject> DbObjects 
        {
            get { return _DbObjects; }
            set { _DbObjects = value; }
        }

        
        internal void Initialize(Point Location, bool FullLoad = false)
        {
            try
            {
                Connection.Open();
                Connection.Close();
            }
            catch (Exception ex)
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                MessageBox.Show("Error on database connection: " + ex.Message, "Database connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Loader = new BackgroundWorker();
            Loader.WorkerReportsProgress = true;
            Loader.WorkerSupportsCancellation = true;
            Loader.ProgressChanged += Loader_ProgressChanged;
            if (!FullLoad)
            {
                Loader.DoWork += Loader_DoWork;
                Loader.RunWorkerCompleted += Loader_RunWorkerCompleted;
            }
            else
            {
                Loader.DoWork += Loader_DoWork2;
                Loader.RunWorkerCompleted += Loader_RunWorkerCompleted2;
            }

            //Inicio de Carga
            ISenseRect = new PopupControl.Popup(LoadDialog = new LoadingInfo());
            ISenseRect.AutoClose = false;
            ISenseRect.FocusOnOpen = false;
            ISenseRect.Opacity = 0.65;
            ISenseRect.ShowingAnimation = PopupControl.PopupAnimations.TopToBottom | PopupControl.PopupAnimations.Slide;
            ISenseRect.HidingAnimation = PopupControl.PopupAnimations.Blend;
            ISenseRect.AnimationDuration = 100;
            LoadDialog.SetInfo(this.Server, "Connecting to database...");
            ISenseRect.Show(Location);

            Loader.RunWorkerAsync();

        }

        public int LoadTables(bool FullLoad = false)
        {
            string sql;
            List<Table> Tables = new List<Table>();
            sql = @"
SELECT DISTINCT
	ISNULL(sch.name, 'dbo') as 'Schema',
	sobj.id as TableId,
	sobj.name as TableName,
	cols.colid as ColumnId,
	cols.name as ColumnName,
	type_name(cols.xusertype) as ColumnType,
	isnull(cols.prec, 0) as Precision,
	isnull(cols.Scale, 0) as Scale,
	isnull(cols.isnullable, 1) as Nullable,
	isnull(cols.iscomputed, 0) as Calculated,
	isnull(comm.text, '') as DefaultValue,
	case when pk.xtype is null then '0' else '1' end as PKey,
	case when fk.fkey is null then '0' else '1' end as FKey,
	isnull(fk.rkeyid, 0) as ReferenceID,
	isnull(fk2.name, '') as ReferenceTable,
	isnull(cols2.name, '') as ReferenceFieldName,
	isnull(cols2.colid, 0) as ReferenceFieldId,
	'' as IndexName,--isnull(indx.name, '') as IndexName,
	isnull(COLUMNPROPERTY(sobj.id,cols.name,'IsIdentity'), 0) IsIdentity,
	IDENT_SEED(sch.name + '.' + sobj.name) as Seed,
	IDENT_INCR(sch.name + '.' + sobj.name) as Increment
FROM   
	sysobjects sobj INNER JOIN syscolumns cols ON sobj.id = cols.id
	LEFT JOIN sysforeignkeys fk ON fk.fkeyid = cols.id AND fk.fkey = cols.colid
	LEFT JOIN syscolumns cols2 ON cols2.id = fk.rkeyid AND cols2.colid = fk.rkey
	LEFT JOIN sysobjects fk2 ON fk.rkeyid = fk2.id
	LEFT JOIN syscomments comm ON cols.cdefault = comm.id OR (cols.id = comm.id and cols.colid = comm.number)
	LEFT JOIN sysindexkeys ik ON ik.id = cols.id AND ik.colid = cols.colid
	LEFT JOIN sysindexes indx ON indx.id = ik.id AND indx.indid = ik.indid
	LEFT JOIN sysobjects pk ON indx.name = pk.name AND pk.parent_obj = indx.id AND pk.xtype = 'PK'
	LEFT JOIN Sys.Objects ObjAux ON sobj.id = ObjAux.object_id
	LEFT JOIN Sys.Schemas sch ON ObjAux.schema_id = sch.schema_id
WHERE   
	sobj.xtype = 'U'   
	and sobj.name <> 'sysdiagrams'
    --and sobj.id = 709577566
order by   
	ISNULL(sch.name, 'dbo'), sobj.name, cols.colid";

            //DbObjects.RemoveAll(X => X.Kind == ObjectType.Table);

            //Get the schema for the tables
            DataTable Info = new DataTable();
            SqlCommand cmd = new SqlCommand(sql, Connection);
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.SelectCommand.Connection.Open();
                da.Fill(Info);
                da.SelectCommand.Connection.Close();
            }
            catch (Exception)
            {
                cmd.Connection.Close();
                return 0;
            }
            cmd.Dispose();

            if (Tables != null)
                Tables.Clear();
            else
                Tables = new List<Table>();

            Table CurObj = null;
            string curtable = "";
            int i, rowcount = Info.Rows.Count;
            for (i = 0; i < rowcount; i++)
            {
                if (!curtable.Equals(Info.Rows[i]["TableName"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    CurObj = new Table()
                    {
                        Name = Info.Rows[i]["TableName"].ToString(),
                        Comment = "",
                        Id = Convert.ToInt32(Info.Rows[i]["TableId"]),
                        Schema = Info.Rows[i]["Schema"].ToString()
                    };
                    curtable = CurObj.Name;
                    Tables.Add(CurObj);
                }

                ISqlChild Exists;
                Exists = CurObj.Childs.Where(X => X.Id == Convert.ToInt32(Info.Rows[i]["ColumnId"])).FirstOrDefault();
                if (Exists != null && Exists.Id > 0)
                {
                    if (!Exists.IsPrimaryKey)
                        Exists.IsPrimaryKey = Convert.ToInt32(Info.Rows[i]["PKey"]) == 1;
                }
                else
                {
                    Field Field = new Field()
                    {
                        Comment = "",
                        Computed = Convert.ToInt32(Info.Rows[i]["Calculated"]) == 1,
                        DefaultValue = Info.Rows[i]["DefaultValue"].ToString(),
                        Id = Convert.ToInt32(Info.Rows[i]["ColumnId"]),
                        ForeignKey = Convert.ToInt32(Info.Rows[i]["ReferenceFieldId"]),
                        IsForeignKey = Convert.ToInt32(Info.Rows[i]["ReferenceFieldId"]) > 0,
                        IsIdentity = Convert.ToInt32(Info.Rows[i]["IsIdentity"]) == 1,
                        IsPrimaryKey = Convert.ToInt32(Info.Rows[i]["PKey"]) == 1,
                        Name = Info.Rows[i]["ColumnName"].ToString(),
                        Nullable = Convert.ToInt32(Info.Rows[i]["Nullable"]) == 1,
                        Parent = CurObj,
                        Precision = Convert.ToInt32(Info.Rows[i]["Precision"]),
                        Type = Info.Rows[i]["ColumnType"].ToString()
                    };
                    if (Field.IsForeignKey)
                    {
                        Field.ReferenceChildName = Info.Rows[i]["ReferenceFieldName"].ToString();
                        Field.ReferenceParentName = Info.Rows[i]["ReferenceTable"].ToString();
                    }


                    if (Field.IsIdentity)
                    {
                        Field.Increment = Convert.ToInt32(Info.Rows[i]["Increment"]);
                        Field.Seed = Convert.ToInt32(Info.Rows[i]["Seed"]);
                    }
                    else
                    {
                        Field.Increment = 0;
                        Field.Seed = 0;
                    }
                    CurObj.Childs.Add(Field);
                }
            }
            if (FullLoad)
            {
                foreach (ISqlObject Table in Tables)
                {
                    Table.LoadScript(null);
                }
            }
            DbObjects.AddRange(Tables);
            TablesLoaded = true;
            return Tables.Count;
        }
        public int LoadViews(bool FullLoad = false)
        {
            List<View> Views = new List<View>();
            string sql;
            sql = @"
SELECT  
	ISNULL(sch.name, 'dbo') as 'Schema',
	sobj.id as ViewId, 
	sobj.name as ViewName, 
	cols.name as FieldName,    
	type_name(cols.xusertype) as Type,    
	isnull(cols.prec, 0) as Length,    
	isnull(cols.Scale, 0) as Scale,    
	isnull(cols.isnullable, 1) as Nullable,    
	isnull(cols.iscomputed, 0) as Calculated	 
FROM  
	sysobjects sobj INNER JOIN syscolumns cols ON sobj.id=cols.id 
	LEFT JOIN Sys.Objects ObjAux ON sobj.id = ObjAux.object_id
	LEFT JOIN Sys.Schemas sch ON ObjAux.schema_id = sch.schema_id
WHERE  
	sobj.xtype = 'V' 
ORDER BY
	sobj.id, cols.colid";

            DataTable Info = new DataTable();
            SqlCommand cmd = new SqlCommand(sql, Connection);
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.SelectCommand.Connection.Open();
                da.Fill(Info);
                da.SelectCommand.Connection.Close();
            }
            catch (Exception)
            {
                cmd.Connection.Close();
                return 0;
            }
            cmd.Dispose();

            //DbObjects.RemoveAll(X => X.Kind == ObjectType.View);

            View CurObj = null;
            string curview = "";
            int i, rowcount = Info.Rows.Count;
            for (i = 0; i < rowcount; i++)
            {
                if (!curview.Equals(Info.Rows[i]["ViewName"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                {//agregar solo el campo
                    CurObj = new View()
                        {
                            Name = Info.Rows[i]["ViewName"].ToString(),
                            Id = Convert.ToInt32(Info.Rows[i]["ViewId"]),
                            Schema = Info.Rows[i]["Schema"].ToString(),
                            Comment = ""
                        };
                    curview = CurObj.Name;
                    Views.Add(CurObj);
                }

                Field Field = new Field()
                {
                    Comment = "",
                    Computed = false,
                    DefaultValue = "",
                    ForeignKey = 0,
                    Id = 0,
                    IdentityScript = "",
                    Increment = 0,
                    IsForeignKey = false,
                    IsIdentity = false,
                    IsPrimaryKey = false,
                    Name = Info.Rows[i]["FieldName"].ToString(),
                    Nullable = true,
                    Parent = CurObj,
                    Precision = Convert.ToInt32(Info.Rows[i]["Length"]),
                    ReferenceChild = null,
                    ReferenceChildName = "",
                    ReferenceParent = null,
                    ReferenceParentName = "",
                    Seed = 0,
                    Type = Info.Rows[i]["Type"].ToString()
                };

                CurObj.Childs.Add(Field);
            }
            if (FullLoad)
            {
                cmd.Connection.Open();
                try
                {
                    foreach (ISqlObject View in Views)
                    {
                        View.LoadScript(cmd);
                    }
                }
                catch (Exception)
                {
                    ;
                }
                finally
                {
                    cmd.Connection.Close();
                }

            }
            DbObjects.AddRange(Views);
            ViewsLoaded = true;
            return Views.Count;
        }
        public int LoadProcedures(bool FullLoad = false)
        {
            List<Procedure> Procedures = new List<Procedure>();
            string sql = @"
SELECT  
	ISNULL(sch.name, 'dbo') as 'Schema',
	sobj.id as ProcedureId, 
	sobj.name as ProcedureName, 
	cols.name as ParamName,    
	type_name(cols.xusertype) as Type,    
	isnull(cols.prec, 0) as Length,    
	isnull(cols.Scale, 0) as Scale,    
	isnull(cols.isnullable, 1) as Nullable,    
	isnull(cols.iscomputed, 0) as Calculated	 
FROM  
	sysobjects sobj LEFT OUTER JOIN syscolumns cols ON sobj.id=cols.id 
	LEFT JOIN Sys.Objects ObjAux ON sobj.id = ObjAux.object_id
	LEFT JOIN Sys.Schemas sch ON ObjAux.schema_id = sch.schema_id
WHERE  
	sobj.xtype = 'P' 
	AND sobj.category = 0
ORDER BY
	sobj.id, cols.colid";

            DataTable Info = new DataTable();
            SqlCommand cmd = new SqlCommand(sql, Connection);
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.SelectCommand.Connection.Open();
                da.Fill(Info);
                da.SelectCommand.Connection.Close();
            }
            catch (Exception)
            {
                cmd.Connection.Close();
                return 0;
            }
            cmd.Dispose();

            //DbObjects.RemoveAll(X => X.Kind == ObjectType.Procedure);

            Procedure CurObj = null;
            string curproc = "";
            int i, rowcount = Info.Rows.Count;
            for (i = 0; i < rowcount; i++)
            {
                if (!curproc.Equals(Info.Rows[i]["ProcedureName"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                {//agregar solo el campo
                    CurObj = new Procedure()
                    {
                        Name = Info.Rows[i]["ProcedureName"].ToString(),
                        Id = Convert.ToInt32(Info.Rows[i]["ProcedureId"]),
                        Schema = Info.Rows[i]["Schema"].ToString(),
                        Comment = ""
                    };
                    curproc = CurObj.Name;
                    Procedures.Add(CurObj);
                }

                if (Info.Rows[i]["ParamName"] == DBNull.Value || String.IsNullOrEmpty(Info.Rows[i]["ParamName"].ToString()))
                    continue;

                Parameter Param = new Parameter()
                {
                    Comment = "",
                    Computed = false,
                    DefaultValue = "",
                    ForeignKey = 0,
                    Id = 0,
                    IdentityScript = "",
                    Increment = 0,
                    IsForeignKey = false,
                    IsIdentity = false,
                    IsPrimaryKey = false,
                    Name = Info.Rows[i]["ParamName"].ToString(),
                    Nullable = true,
                    Parent = CurObj,
                    Precision = Convert.ToInt32(Info.Rows[i]["Length"]),
                    ReferenceChild = null,
                    ReferenceChildName = "",
                    ReferenceParent = null,
                    ReferenceParentName = "",
                    Seed = 0,
                    Type = Info.Rows[i]["Type"].ToString()
                };

                CurObj.Childs.Add(Param);
            }
            if (FullLoad)
            {
                cmd.Connection.Open();
                try
                {
                    foreach (ISqlObject Proc in Procedures)
                    {
                        Proc.LoadScript(cmd);
                    }
                }
                catch (Exception)
                {
                    ;
                }
                finally
                {
                    cmd.Connection.Close();
                }

            }
            DbObjects.AddRange(Procedures);
            SPsLoaded = true;
            return Procedures.Count;
        }
        public int LoadScalarFunctions(bool FullLoad = false)
        {
            List<ScalarFunction> Functions = new List<ScalarFunction>();
            string sql = @"
SELECT  
	ISNULL(sch.name, 'dbo') as 'Schema',
	sobj.id as FunctionId, 
	sobj.name as FunctionName, 
	cols.name as ParamName,    
	type_name(cols.xusertype) as Type,    
	isnull(cols.prec, 0) as Length,    
	isnull(cols.Scale, 0) as Scale,    
	isnull(cols.isnullable, 1) as Nullable,    
	isnull(cols.iscomputed, 0) as Calculated	 
FROM  
	sysobjects sobj LEFT OUTER JOIN syscolumns cols ON sobj.id=cols.id 
	LEFT JOIN Sys.Objects ObjAux ON sobj.id = ObjAux.object_id
	LEFT JOIN Sys.Schemas sch ON ObjAux.schema_id = sch.schema_id
WHERE  
	sobj.xtype = 'FN' 
ORDER BY
	sobj.id, cols.colid";

            DataTable Info = new DataTable();
            SqlCommand cmd = new SqlCommand(sql, Connection);
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.SelectCommand.Connection.Open();
                da.Fill(Info);
                da.SelectCommand.Connection.Close();
            }
            catch (Exception)
            {
                cmd.Connection.Close();
                return 0;
            }
            cmd.Dispose();

            //DbObjects.RemoveAll(X => X.Kind == ObjectType.ScalarFunction);

            ScalarFunction CurObj = null;
            string curfunc = "";
            int i, rowcount = Info.Rows.Count;
            for (i = 0; i < rowcount; i++)
            {
                if (!curfunc.Equals(Info.Rows[i]["FunctionName"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                {//agregar solo el campo
                    CurObj = new ScalarFunction()
                    {
                        Name = Info.Rows[i]["FunctionName"].ToString(),
                        Id = Convert.ToInt32(Info.Rows[i]["FunctionId"]),
                        Schema = Info.Rows[i]["Schema"].ToString(),
                        Comment = ""
                    };
                    curfunc = CurObj.Name;
                    Functions.Add(CurObj);
                }

                if (Info.Rows[i]["ParamName"] == DBNull.Value || String.IsNullOrEmpty(Info.Rows[i]["ParamName"].ToString()))
                    continue;

                Parameter Param = new Parameter()
                {
                    Comment = "",
                    Computed = false,
                    DefaultValue = "",
                    ForeignKey = 0,
                    Id = 0,
                    IdentityScript = "",
                    Increment = 0,
                    IsForeignKey = false,
                    IsIdentity = false,
                    IsPrimaryKey = false,
                    Name = Info.Rows[i]["ParamName"].ToString(),
                    Nullable = true,
                    Parent = CurObj,
                    Precision = Convert.ToInt32(Info.Rows[i]["Length"]),
                    ReferenceChild = null,
                    ReferenceChildName = "",
                    ReferenceParent = null,
                    ReferenceParentName = "",
                    Seed = 0,
                    Type = Info.Rows[i]["Type"].ToString()
                };

                CurObj.Childs.Add(Param);
            }
            if (FullLoad)
            {
                cmd.Connection.Open();
                try
                {
                    foreach (ISqlObject Funct in Functions)
                    {
                        Funct.LoadScript(cmd);
                    }
                }
                catch (Exception)
                {
                    ;
                }
                finally
                {
                    cmd.Connection.Close();
                }

            }
            DbObjects.AddRange(Functions);
            ScalarFunctionsLoaded = true;
            return Functions.Count;
        }
        public int LoadTableFunctions(bool FullLoad = false)
        {
            List<TableFunction> Functions = new List<TableFunction>();
            string sql = @"
SELECT  
	ISNULL(sch.name, 'dbo') as 'Schema',
	sobj.id as FunctionId, 
	sobj.name as FunctionName, 
	cols.name as ParamName,    
	type_name(cols.xusertype) as Type,    
	isnull(cols.prec, 0) as Length,    
	isnull(cols.Scale, 0) as Scale,    
	isnull(cols.isnullable, 1) as Nullable,    
	isnull(cols.iscomputed, 0) as Calculated	 
FROM  
	sysobjects sobj LEFT OUTER JOIN syscolumns cols ON sobj.id=cols.id 
	LEFT JOIN Sys.Objects ObjAux ON sobj.id = ObjAux.object_id
	LEFT JOIN Sys.Schemas sch ON ObjAux.schema_id = sch.schema_id
WHERE  
	sobj.xtype = 'TF' 
ORDER BY
	sobj.id, cols.colid";

            DataTable Info = new DataTable();
            SqlCommand cmd = new SqlCommand(sql, Connection);
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.SelectCommand.Connection.Open();
                da.Fill(Info);
                da.SelectCommand.Connection.Close();
            }
            catch (Exception)
            {
                cmd.Connection.Close();
                return 0;
            }
            cmd.Dispose();

            //DbObjects.RemoveAll(X => X.Kind == ObjectType.ScalarFunction);

            TableFunction CurObj = null;
            string curfunc = "";
            int i, rowcount = Info.Rows.Count;
            for (i = 0; i < rowcount; i++)
            {
                if (!curfunc.Equals(Info.Rows[i]["FunctionName"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                {//agregar solo el campo
                    CurObj = new TableFunction()
                    {
                        Name = Info.Rows[i]["FunctionName"].ToString(),
                        Id = Convert.ToInt32(Info.Rows[i]["FunctionId"]),
                        Schema = Info.Rows[i]["Schema"].ToString(),
                        Comment = ""
                    };
                    curfunc = CurObj.Name;
                    Functions.Add(CurObj);
                }

                if (Info.Rows[i]["ParamName"] == DBNull.Value || String.IsNullOrEmpty(Info.Rows[i]["ParamName"].ToString()))
                    continue;

                string name = Info.Rows[i]["ParamName"].ToString();
                if (name.StartsWith("@"))
                {
                    Parameter Param = new Parameter()
                    {
                        Comment = "",
                        Computed = false,
                        DefaultValue = "",
                        ForeignKey = 0,
                        Id = 0,
                        IdentityScript = "",
                        Increment = 0,
                        IsForeignKey = false,
                        IsIdentity = false,
                        IsPrimaryKey = false,
                        Name = Info.Rows[i]["ParamName"].ToString(),
                        Nullable = true,
                        Parent = CurObj,
                        Precision = Convert.ToInt32(Info.Rows[i]["Length"]),
                        ReferenceChild = null,
                        ReferenceChildName = "",
                        ReferenceParent = null,
                        ReferenceParentName = "",
                        Seed = 0,
                        Type = Info.Rows[i]["Type"].ToString()
                    };
                    CurObj.Childs.Add(Param);
                }
                else
                {
                    Field Field = new Field()
                    {
                        Comment = "",
                        Computed = false,
                        DefaultValue = "",
                        ForeignKey = 0,
                        Id = 0,
                        IdentityScript = "",
                        Increment = 0,
                        IsForeignKey = false,
                        IsIdentity = false,
                        IsPrimaryKey = false,
                        Name = Info.Rows[i]["ParamName"].ToString(),
                        Nullable = true,
                        Parent = CurObj,
                        Precision = Convert.ToInt32(Info.Rows[i]["Length"]),
                        ReferenceChild = null,
                        ReferenceChildName = "",
                        ReferenceParent = null,
                        ReferenceParentName = "",
                        Seed = 0,
                        Type = Info.Rows[i]["Type"].ToString()
                    };
                    CurObj.Childs.Add(Field);
                }

            }
            if (FullLoad)
            {
                cmd.Connection.Open();
                try
                {
                    foreach (ISqlObject Funct in Functions)
                    {
                        Funct.LoadScript(cmd);
                    }
                }
                catch (Exception)
                {
                    ;
                }
                finally
                {
                    cmd.Connection.Close();
                }

            }
            DbObjects.AddRange(Functions);
            TableFunctionsLoaded = true;
            return Functions.Count;
        }

        private void Loader_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            e.Result = LoadInfo();
            if (bw.CancellationPending)
            {
                e.Cancel = true;
            }
        }
        private void Loader_DoWork2(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            e.Result = LoadInfo(true);
            if (bw.CancellationPending)
            {
                e.Cancel = true;
            }
        }
        private void Loader_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (ISenseRect != null)
            {
                ISenseRect.Close();
                ISenseRect.Dispose();
            }
            if (LoadDialog != null)
            {
                LoadDialog.Dispose();
                LoadDialog = null;
            }
            _Loaded = true;
            _FullLoaded = false;
            if (OnEndedLoad != null)
                OnEndedLoad();
        }
        private void Loader_RunWorkerCompleted2(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (ISenseRect != null)
            {
                ISenseRect.Close();
                ISenseRect.Dispose();
            }
            if (LoadDialog != null)
            {
                LoadDialog.Dispose();
                LoadDialog = null;
            }
            _Loaded = true;
            _FullLoaded = true;
            if (OnEndedLoad != null)
                OnEndedLoad();
        }
        private void Loader_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            string[] Data = e.UserState.ToString().Split(new char[] { '|' });
            try
            {
                LoadDialog.SetInfo(Data[0], Data[1]);
                LoadDialog.Refresh();
            }
            catch (Exception exception)
            {
                Thread.Sleep(1000);
            }
        }
        private object LoadInfo(bool FullLoad = true)
        {
            List<ISqlObject> Schemas = new List<ISqlObject>();
            int WaitTime = 250;
            DbObjects.Clear();
            int ActResult;
            try
            {
                Loader.ReportProgress(0, String.Format("{0}|Reading tables from database", DataBase));
                ActResult = LoadTables(FullLoad);
                Loader.ReportProgress(0, String.Format("{1}|Read {0} tables.", ActResult, DataBase));
                Thread.Sleep(WaitTime);
                Loader.ReportProgress(0, String.Format("{0}|Reading views from database", DataBase));
                ActResult = LoadViews(FullLoad);
                Loader.ReportProgress(0, String.Format("{1}|Read {0} views", ActResult, DataBase));
                Thread.Sleep(WaitTime);
                Loader.ReportProgress(0, String.Format("{0}|Reading stored procedure from database", DataBase));
                ActResult = LoadProcedures(FullLoad);
                Loader.ReportProgress(0, String.Format("{1}|Read {0} stored procedures", ActResult, DataBase));
                Thread.Sleep(WaitTime);
                Loader.ReportProgress(0, String.Format("{0}|Reading scalar functions from database", DataBase));
                ActResult = LoadScalarFunctions(FullLoad);
                Loader.ReportProgress(0, String.Format("{1}|Read {0} scalar functions", ActResult, DataBase));
                Thread.Sleep(WaitTime);
                Loader.ReportProgress(0, String.Format("{0}|Reading table functions from database", DataBase));
                ActResult = LoadTableFunctions(FullLoad);
                Loader.ReportProgress(0, String.Format("{1}|Read {0} table functions", ActResult, DataBase));
                Thread.Sleep(WaitTime);
                Loader.ReportProgress(0, String.Format("{0}|Extracted all the info from the database, {1} objects loaded", DataBase, DbObjects.Count));
                Thread.Sleep(WaitTime);

                foreach (ISqlObject Sch in _DbObjects.DistinctBy(X => X.Schema))
                {
                    Schemas.Add
                        (new Scheme()
                            {
                                Name = Sch.Schema,
                                Schema = "",
                                Text = Sch.Schema
                            }
                        );
                }
                _DbObjects.AddRange(Schemas);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Loading database info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            return 1;
        }
        public bool ExecuteNonQuery(string query)
        {
            SqlCommand cmd = new SqlCommand(query, Connection);
            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception)
            {
                cmd.Connection.Close();
                return false;
            }
            cmd.Dispose();

            return true;
        }
        public object ExecuteScalar(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Connection);
            object result = null;
            try
            {
                cmd.Connection = Connection;
                cmd.CommandText = sql;
                cmd.Connection.Open();

                cmd.CommandType = CommandType.Text;
                result = cmd.ExecuteScalar();
            }
            catch (SqlException sqlex)
            {
                result = null;
            }
            catch (Exception ex)
            {
                result = null;
            }
            finally
            {
                if (cmd != null)
                {
                    if (cmd.Connection.State == ConnectionState.Open)
                        cmd.Connection.Close();
                }
            }
            return result;
        }
        public DataSet ExecuteDataSet(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Connection);
            SqlDataAdapter da = null;
            DataSet result = new DataSet();

            try
            {
                cmd.Connection = Connection;
                cmd.CommandText = sql;
                da = new SqlDataAdapter(cmd);
                cmd.CommandType = CommandType.Text;

                da.SelectCommand.Connection.Open();
                da.Fill(result);
            }
            catch (SqlException sqlex)
            {
                result = null;
            }
            catch (Exception ex)
            {
                result = null;
            }
            finally
            {
                if (da != null && da.SelectCommand != null)
                {
                    if (da.SelectCommand.Connection.State == ConnectionState.Open)
                        da.SelectCommand.Connection.Close();
                }
            }
            return result;
        }
        public DataTable ExecuteTable(string sql, string tableName = "")
        {
            DataTable aux = null;
            DataSet info;

            info = ExecuteDataSet(sql);
            if (info != null && info.Tables.Count > 0)
            {
                if (!String.IsNullOrEmpty(tableName))
                {
                    info.Tables[0].TableName = tableName;
                }
                aux = info.Tables[0];
            }

            return aux;
        }
        public List<string> ExecuteColumn(string sql)
        {
            List<string> back = new List<string>();
            DataTable aux;
            aux = ExecuteTable(sql);
            if (aux != null && aux.Rows.Count > 0)
            {
                int rc = aux.Rows.Count;
                for (int i = 0; i < rc; i++)
                    back.Add(aux.Rows[i][0].ToString());
            }
            return back;
        }

        internal ISqlObject IsTableTypeObject(string TableName)
        {
            List<string> Data = TableName.Split('.').ToList();

            if (Data.Count == 1)
            {
                if (Data[0].Contains('('))//if contains a '(' can be a table valued function, so remove everything after that including it
                {
                    if (Data[0].IndexOf('(') > 0)
                        Data[0] = Data[0].Substring(0, Data[0].IndexOf('(') - 1);
                    else
                        return null;
                }
                return DbObjects.FirstOrDefault
                    (
                        X =>
                            (X.Kind == ObjectType.Table || X.Kind == ObjectType.TableFunction || X.Kind == ObjectType.View)
                            && X.Schema.Equals("dbo", StringComparison.CurrentCultureIgnoreCase)
                            && X.Name.Equals(Data[0], StringComparison.CurrentCultureIgnoreCase)
                    );
            }
            else if (Data.Count == 2)
            {
                if (Data[1].Contains('('))//if contains a '(' can be a table valued function, so remove everything after that including it
                {
                    if (Data[1].IndexOf('(') > 0)
                        Data[1] = Data[1].Substring(0, Data[1].IndexOf('(') - 1);
                    else
                        return null;
                }
                return DbObjects.FirstOrDefault
                    (
                        X =>
                            (X.Kind == ObjectType.Table || X.Kind == ObjectType.TableFunction || X.Kind == ObjectType.View)
                            && X.Schema.Equals(Data[0], StringComparison.CurrentCultureIgnoreCase)
                            && X.Name.Equals(Data[1], StringComparison.CurrentCultureIgnoreCase)
                    );
            }

            return null;
        }
        internal ISqlObject IsTableValuedFunction(string FunctionName)
        {
            List<string> Data = FunctionName.Split('.').ToList();

            if (Data.Count == 1)
            {
                if (Data[0].Contains('('))//if contains a '(' can be a table valued function, so remove everything after that including it
                {
                    if (Data[0].IndexOf('(') > 0)
                        Data[0] = Data[0].Substring(0, Data[0].IndexOf('(') - 1);
                    else
                        return null;
                }
                return DbObjects.FirstOrDefault
                    (
                        X =>
                            X.Kind == ObjectType.TableFunction
                            && X.Schema.Equals("dbo", StringComparison.CurrentCultureIgnoreCase)
                            && X.Name.Equals(Data[0], StringComparison.CurrentCultureIgnoreCase)
                    );
            }
            else if (Data.Count == 2)
            {
                if (Data[1].Contains('('))//if contains a '(' can be a table valued function, so remove everything after that including it
                {
                    if (Data[1].IndexOf('(') > 0)
                        Data[1] = Data[1].Substring(0, Data[1].IndexOf('(') - 1);
                    else
                        return null;
                }
                return DbObjects.FirstOrDefault
                    (
                        X =>
                            X.Kind == ObjectType.TableFunction
                            && X.Schema.Equals(Data[0], StringComparison.CurrentCultureIgnoreCase)
                            && X.Name.Equals(Data[1], StringComparison.CurrentCultureIgnoreCase)
                    );
            }

            return null;
        }
        internal ISqlObject IsStoredProcedure(string spName)
        {
            List<string> Data = spName.Split('.').ToList();

            if (Data.Count == 1)
            {
                if (Data[0].Contains('('))//if contains a '(' can be a table valued function, so remove everything after that including it
                {
                    if (Data[0].IndexOf('(') > 0)
                        Data[0] = Data[0].Substring(0, Data[0].IndexOf('(') - 1);
                    else
                        return null;
                }
                return DbObjects.FirstOrDefault
                    (
                        X =>
                            X.Kind == ObjectType.Procedure
                            && X.Schema.Equals("dbo", StringComparison.CurrentCultureIgnoreCase)
                            && X.Name.Equals(Data[0], StringComparison.CurrentCultureIgnoreCase)
                    );
            }
            else if (Data.Count == 2)
            {
                if (Data[1].Contains('('))//if contains a '(' can be a table valued function, so remove everything after that including it
                {
                    if (Data[1].IndexOf('(') > 0)
                        Data[1] = Data[1].Substring(0, Data[1].IndexOf('(') - 1);
                    else
                        return null;
                }
                return DbObjects.FirstOrDefault
                    (
                        X =>
                            X.Kind == ObjectType.Procedure
                            && X.Schema.Equals(Data[0], StringComparison.CurrentCultureIgnoreCase)
                            && X.Name.Equals(Data[1], StringComparison.CurrentCultureIgnoreCase)
                    );
            }
            return null;
        }

        internal ISqlObject IsSqlObject(string ObjectName)
        {
            List<string> Data = ObjectName.Split('.').ToList();

            if (Data.Count == 1)
            {
                if (Data[0].Contains('('))//if contains a '(' can be a table valued function, so remove everything after that including it
                {
                    if (Data[0].IndexOf('(') > 0)
                        Data[0] = Data[0].Substring(0, Data[0].IndexOf('(') - 1);
                    else
                        return null;
                }
                return DbObjects.Find
                    (
                        X =>
                            X.Schema.Equals("dbo", StringComparison.CurrentCultureIgnoreCase)
                            && X.Name.Equals(Data[0], StringComparison.CurrentCultureIgnoreCase)
                    );
            }
            else if (Data.Count == 2)
            {
                if (Data[1].Contains('('))//if contains a '(' can be a table valued function, so remove everything after that including it
                {
                    if (Data[1].IndexOf('(') > 0)
                        Data[1] = Data[1].Substring(0, Data[1].IndexOf('(') - 1);
                    else
                        return null;
                }
                return DbObjects.Find
                    (
                        X =>
                            X.Schema.Equals(Data[0], StringComparison.CurrentCultureIgnoreCase)
                            && X.Name.Equals(Data[1], StringComparison.CurrentCultureIgnoreCase)
                    );
            }

            return null;
        }
    }
}
