namespace Ez_SQL.QueryLog
{
    partial class HistoricForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            HDesarrollo.Controles.Grid.OpcionAH opcionAH46 = new HDesarrollo.Controles.Grid.OpcionAH();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoricForm));
            HDesarrollo.Controles.Grid.OpcionAH opcionAH47 = new HDesarrollo.Controles.Grid.OpcionAH();
            HDesarrollo.Controles.Grid.OpcionAH opcionAH48 = new HDesarrollo.Controles.Grid.OpcionAH();
            HDesarrollo.Controles.Grid.OpcionAH opcionAH49 = new HDesarrollo.Controles.Grid.OpcionAH();
            HDesarrollo.Controles.Grid.OpcionAH opcionAH50 = new HDesarrollo.Controles.Grid.OpcionAH();
            HDesarrollo.Controles.Grid.OpcionAH opcionAH51 = new HDesarrollo.Controles.Grid.OpcionAH();
            HDesarrollo.Controles.Grid.OpcionAH opcionAH52 = new HDesarrollo.Controles.Grid.OpcionAH();
            HDesarrollo.Controles.Grid.OpcionAH opcionAH53 = new HDesarrollo.Controles.Grid.OpcionAH();
            HDesarrollo.Controles.Grid.OpcionAH opcionAH54 = new HDesarrollo.Controles.Grid.OpcionAH();
            HDesarrollo.Controles.Grid.OpcionAH opcionAH55 = new HDesarrollo.Controles.Grid.OpcionAH();
            HDesarrollo.Controles.Grid.OpcionAH opcionAH56 = new HDesarrollo.Controles.Grid.OpcionAH();
            GridViewExtensions.GridFilterFactories.DefaultGridFilterFactory defaultGridFilterFactory4 = new GridViewExtensions.GridFilterFactories.DefaultGridFilterFactory();
            HDesarrollo.Controles.Grid.OpcionAH opcionAH57 = new HDesarrollo.Controles.Grid.OpcionAH();
            HDesarrollo.Controles.Grid.OpcionAH opcionAH58 = new HDesarrollo.Controles.Grid.OpcionAH();
            HDesarrollo.Controles.Grid.OpcionAH opcionAH59 = new HDesarrollo.Controles.Grid.OpcionAH();
            HDesarrollo.Controles.Grid.GridPrinter gridPrinter4 = new HDesarrollo.Controles.Grid.GridPrinter();
            HDesarrollo.Controles.Grid.OpcionAH opcionAH60 = new HDesarrollo.Controles.Grid.OpcionAH();
            this.MarkErrors = new HDesarrollo.Controles.Grid.CondicionMarcado();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SGrid = new HDesarrollo.Controles.Grid.SmartGrid();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TxtRightExec = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.TxtTReturn = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.TxtAffected = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TxtReaded = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtLapse = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TxtEnd = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TxtStart = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtDb = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtServer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtGroup = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ErrorGrid = new System.Windows.Forms.DataGridView();
            this.ScriptText = new ICSharpCode.TextEditor.TextEditorControl();
            this.ThisTabMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllButThisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorGrid)).BeginInit();
            this.ThisTabMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MarkErrors
            // 
            this.MarkErrors.CampoAComparar = 7;
            this.MarkErrors.CampoComparador1 = -1;
            this.MarkErrors.CampoComparador2 = -1;
            this.MarkErrors.Categoria = HDesarrollo.Controles.Grid.TipoCondicion.Numerica;
            this.MarkErrors.ComparacionNumerica = HDesarrollo.Controles.Grid.ComparadoresNumericos.Diferente;
            this.MarkErrors.ComparacionTexto = HDesarrollo.Controles.Grid.ComparadoresTexto.EmpiezaCon;
            this.MarkErrors.Fondo = System.Drawing.Color.Red;
            this.MarkErrors.FormatoRegistro = HDesarrollo.Controles.Grid.Formato.Especial;
            this.MarkErrors.Letra = System.Drawing.Color.White;
            this.MarkErrors.SensibilidadMayusculas = false;
            this.MarkErrors.ValorComparacion1 = "1";
            this.MarkErrors.ValorComparacion2 = null;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.SGrid);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(991, 492);
            this.splitContainer1.SplitterDistance = 272;
            this.splitContainer1.TabIndex = 6;
            // 
            // SGrid
            // 
            opcionAH46.Imagen = ((System.Drawing.Image)(resources.GetObject("opcionAH46.Imagen")));
            opcionAH46.TextoMenu = "Agregar...";
            opcionAH46.ToolTip = "Agregar un Nuevo Registro";
            opcionAH46.Visible = false;
            this.SGrid.BotonAgregar = opcionAH46;
            opcionAH47.Imagen = ((System.Drawing.Image)(resources.GetObject("opcionAH47.Imagen")));
            opcionAH47.TextoMenu = "Eliminar";
            opcionAH47.ToolTip = "Eliminar el(los) Registros Seleccionados";
            opcionAH47.Visible = false;
            this.SGrid.BotonEliminar = opcionAH47;
            opcionAH48.Imagen = ((System.Drawing.Image)(resources.GetObject("opcionAH48.Imagen")));
            opcionAH48.TextoMenu = "Opcion Extra 1";
            opcionAH48.ToolTip = "Opcion Extra 1";
            opcionAH48.Visible = false;
            this.SGrid.BotonExtra1 = opcionAH48;
            opcionAH49.Imagen = ((System.Drawing.Image)(resources.GetObject("opcionAH49.Imagen")));
            opcionAH49.TextoMenu = "Opcion Extra 2";
            opcionAH49.ToolTip = "Opcion Extra 2";
            opcionAH49.Visible = false;
            this.SGrid.BotonExtra2 = opcionAH49;
            opcionAH50.Imagen = ((System.Drawing.Image)(resources.GetObject("opcionAH50.Imagen")));
            opcionAH50.TextoMenu = "Opcion Extra 3";
            opcionAH50.ToolTip = "Opcion Extra 3";
            opcionAH50.Visible = false;
            this.SGrid.BotonExtra3 = opcionAH50;
            opcionAH51.Imagen = ((System.Drawing.Image)(resources.GetObject("opcionAH51.Imagen")));
            opcionAH51.TextoMenu = "Opcion Extra 4";
            opcionAH51.ToolTip = "Opcion Extra 4";
            opcionAH51.Visible = false;
            this.SGrid.BotonExtra4 = opcionAH51;
            opcionAH52.Imagen = ((System.Drawing.Image)(resources.GetObject("opcionAH52.Imagen")));
            opcionAH52.TextoMenu = "Opcion Extra 5";
            opcionAH52.ToolTip = "Opcion Extra 5";
            opcionAH52.Visible = false;
            this.SGrid.BotonExtra5 = opcionAH52;
            opcionAH53.Imagen = ((System.Drawing.Image)(resources.GetObject("opcionAH53.Imagen")));
            opcionAH53.TextoMenu = "Modificar";
            opcionAH53.ToolTip = "Modificar Registro Seleccionado";
            opcionAH53.Visible = false;
            this.SGrid.BotonModificar = opcionAH53;
            opcionAH54.Imagen = ((System.Drawing.Image)(resources.GetObject("opcionAH54.Imagen")));
            opcionAH54.TextoMenu = "Buscar en Tabla";
            opcionAH54.ToolTip = "Abre ventana auxiliar para realizar busquedas en la tabla actual";
            opcionAH54.Visible = true;
            this.SGrid.Busqueda = opcionAH54;
            opcionAH55.Imagen = ((System.Drawing.Image)(resources.GetObject("opcionAH55.Imagen")));
            opcionAH55.TextoMenu = "Elegir Columnas";
            opcionAH55.ToolTip = "Abre ventana donde se pueden elegir las columnas que se mostraran en este control" +
    "";
            opcionAH55.Visible = true;
            this.SGrid.Columnas = opcionAH55;
            this.SGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            opcionAH56.Imagen = ((System.Drawing.Image)(resources.GetObject("opcionAH56.Imagen")));
            opcionAH56.TextoMenu = "Exportar Tabla a Archivo";
            opcionAH56.ToolTip = "Exportacion de Informacion a Archivo, se permiten varios formatos: Texto Plano, E" +
    "xcel, HTML";
            opcionAH56.Visible = true;
            this.SGrid.Exportacion = opcionAH56;
            this.SGrid.FilterBoxPosition = GridViewExtensions.FilterPosition.Off;
            defaultGridFilterFactory4.CreateDistinctGridFilters = false;
            defaultGridFilterFactory4.DefaultGridFilterType = typeof(GridViewExtensions.GridFilters.TextGridFilter);
            defaultGridFilterFactory4.DefaultShowDateInBetweenOperator = false;
            defaultGridFilterFactory4.DefaultShowNumericInBetweenOperator = false;
            defaultGridFilterFactory4.HandleEnumerationTypes = true;
            defaultGridFilterFactory4.MaximumDistinctValues = 20;
            this.SGrid.FilterFactory = defaultGridFilterFactory4;
            this.SGrid.FilterTextVisible = false;
            opcionAH57.Imagen = ((System.Drawing.Image)(resources.GetObject("opcionAH57.Imagen")));
            opcionAH57.TextoMenu = "Filtros";
            opcionAH57.ToolTip = "Oculta o Muestra los Controles para el Filtrado de los Datos";
            opcionAH57.Visible = true;
            this.SGrid.Filtrado = opcionAH57;
            this.SGrid.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SGrid.GridLines = false;
            opcionAH58.Imagen = ((System.Drawing.Image)(resources.GetObject("opcionAH58.Imagen")));
            opcionAH58.TextoMenu = "Vista Preliminar/Impresion";
            opcionAH58.ToolTip = "Muestra la vista preliminar de la rejilla, permitiendo la impresion de esta vista" +
    "";
            opcionAH58.Visible = true;
            this.SGrid.Impresion = opcionAH58;
            this.SGrid.Location = new System.Drawing.Point(0, 0);
            opcionAH59.Imagen = ((System.Drawing.Image)(resources.GetObject("opcionAH59.Imagen")));
            opcionAH59.TextoMenu = "Formato Condicional";
            opcionAH59.ToolTip = "Permite crear un formato especifico para los registros que cumplan ciertas condic" +
    "iones";
            opcionAH59.Visible = true;
            this.SGrid.Marcado = opcionAH59;
            this.SGrid.Marcados.AddRange(new HDesarrollo.Controles.Grid.CondicionMarcado[] {
            this.MarkErrors});
            this.SGrid.Name = "SGrid";
            this.SGrid.OpenAfterXport = false;
            gridPrinter4.BottomMargin = 0;
            gridPrinter4.LeftMargin = 0;
            gridPrinter4.RightMargin = 0;
            gridPrinter4.TopMargin = 0;
            this.SGrid.Printer = gridPrinter4;
            this.SGrid.Size = new System.Drawing.Size(991, 184);
            opcionAH60.Imagen = ((System.Drawing.Image)(resources.GetObject("opcionAH60.Imagen")));
            opcionAH60.TextoMenu = "Sumarizar por Columna";
            opcionAH60.ToolTip = "Permite establecer una operacion de sumarizado de la columna seleccionada";
            opcionAH60.Visible = true;
            this.SGrid.Sumarizacion = opcionAH60;
            this.SGrid.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TxtRightExec);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.TxtTReturn);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.TxtAffected);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.TxtReaded);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.TxtLapse);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.TxtEnd);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.TxtStart);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.TxtDb);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.TxtServer);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.TxtName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.TxtGroup);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 184);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(991, 88);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos de Ejecucion";
            // 
            // TxtRightExec
            // 
            this.TxtRightExec.Location = new System.Drawing.Point(892, 65);
            this.TxtRightExec.Name = "TxtRightExec";
            this.TxtRightExec.ReadOnly = true;
            this.TxtRightExec.Size = new System.Drawing.Size(100, 21);
            this.TxtRightExec.TabIndex = 23;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(890, 50);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(88, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "Execution correct";
            // 
            // TxtTReturn
            // 
            this.TxtTReturn.Location = new System.Drawing.Point(765, 65);
            this.TxtTReturn.Name = "TxtTReturn";
            this.TxtTReturn.ReadOnly = true;
            this.TxtTReturn.Size = new System.Drawing.Size(100, 21);
            this.TxtTReturn.TabIndex = 21;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(763, 50);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Grid count";
            // 
            // TxtAffected
            // 
            this.TxtAffected.Location = new System.Drawing.Point(638, 65);
            this.TxtAffected.Name = "TxtAffected";
            this.TxtAffected.ReadOnly = true;
            this.TxtAffected.Size = new System.Drawing.Size(100, 21);
            this.TxtAffected.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(638, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Records affected";
            // 
            // TxtReaded
            // 
            this.TxtReaded.Location = new System.Drawing.Point(511, 65);
            this.TxtReaded.Name = "TxtReaded";
            this.TxtReaded.ReadOnly = true;
            this.TxtReaded.Size = new System.Drawing.Size(100, 21);
            this.TxtReaded.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(511, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Records read";
            // 
            // TxtLapse
            // 
            this.TxtLapse.Location = new System.Drawing.Point(384, 65);
            this.TxtLapse.Name = "TxtLapse";
            this.TxtLapse.ReadOnly = true;
            this.TxtLapse.Size = new System.Drawing.Size(100, 21);
            this.TxtLapse.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(381, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Lapse (ms)";
            // 
            // TxtEnd
            // 
            this.TxtEnd.Location = new System.Drawing.Point(194, 65);
            this.TxtEnd.Name = "TxtEnd";
            this.TxtEnd.ReadOnly = true;
            this.TxtEnd.Size = new System.Drawing.Size(163, 21);
            this.TxtEnd.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(190, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Execution end";
            // 
            // TxtStart
            // 
            this.TxtStart.Location = new System.Drawing.Point(4, 65);
            this.TxtStart.Name = "TxtStart";
            this.TxtStart.ReadOnly = true;
            this.TxtStart.Size = new System.Drawing.Size(163, 21);
            this.TxtStart.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Execution start";
            // 
            // TxtDb
            // 
            this.TxtDb.Location = new System.Drawing.Point(763, 28);
            this.TxtDb.Name = "TxtDb";
            this.TxtDb.ReadOnly = true;
            this.TxtDb.Size = new System.Drawing.Size(232, 21);
            this.TxtDb.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(763, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Database";
            // 
            // TxtServer
            // 
            this.TxtServer.Location = new System.Drawing.Point(510, 28);
            this.TxtServer.Name = "TxtServer";
            this.TxtServer.ReadOnly = true;
            this.TxtServer.Size = new System.Drawing.Size(232, 21);
            this.TxtServer.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(510, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Server";
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(257, 28);
            this.TxtName.Name = "TxtName";
            this.TxtName.ReadOnly = true;
            this.TxtName.Size = new System.Drawing.Size(232, 21);
            this.TxtName.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(257, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Name";
            // 
            // TxtGroup
            // 
            this.TxtGroup.Location = new System.Drawing.Point(4, 28);
            this.TxtGroup.Name = "TxtGroup";
            this.TxtGroup.ReadOnly = true;
            this.TxtGroup.Size = new System.Drawing.Size(232, 21);
            this.TxtGroup.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Group";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.ScriptText);
            this.splitContainer3.Size = new System.Drawing.Size(991, 216);
            this.splitContainer3.SplitterDistance = 384;
            this.splitContainer3.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ErrorGrid);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(384, 216);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Execution errors";
            // 
            // ErrorGrid
            // 
            this.ErrorGrid.AllowUserToAddRows = false;
            this.ErrorGrid.AllowUserToDeleteRows = false;
            this.ErrorGrid.AllowUserToResizeColumns = false;
            this.ErrorGrid.AllowUserToResizeRows = false;
            this.ErrorGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.ErrorGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ErrorGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ErrorGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorGrid.Location = new System.Drawing.Point(3, 19);
            this.ErrorGrid.Name = "ErrorGrid";
            this.ErrorGrid.ReadOnly = true;
            this.ErrorGrid.Size = new System.Drawing.Size(378, 194);
            this.ErrorGrid.TabIndex = 0;
            this.ErrorGrid.SelectionChanged += new System.EventHandler(this.ErrorGrid_SelectionChanged);
            // 
            // ScriptText
            // 
            this.ScriptText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScriptText.EnableFolding = false;
            this.ScriptText.IsIconBarVisible = true;
            this.ScriptText.IsReadOnly = false;
            this.ScriptText.Location = new System.Drawing.Point(0, 0);
            this.ScriptText.Name = "ScriptText";
            this.ScriptText.ShowVRuler = false;
            this.ScriptText.Size = new System.Drawing.Size(603, 216);
            this.ScriptText.TabIndex = 3;
            // 
            // ThisTabMenu
            // 
            this.ThisTabMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem,
            this.closeAllToolStripMenuItem,
            this.closeAllButThisToolStripMenuItem});
            this.ThisTabMenu.Name = "ThisTabMenu";
            this.ThisTabMenu.Size = new System.Drawing.Size(167, 70);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = global::Ez_SQL.Properties.Resources.Delete_24;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("closeAllToolStripMenuItem.Image")));
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.closeAllToolStripMenuItem.Text = "Close All";
            this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.closeAllToolStripMenuItem_Click);
            // 
            // closeAllButThisToolStripMenuItem
            // 
            this.closeAllButThisToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("closeAllButThisToolStripMenuItem.Image")));
            this.closeAllButThisToolStripMenuItem.Name = "closeAllButThisToolStripMenuItem";
            this.closeAllButThisToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.closeAllButThisToolStripMenuItem.Text = "Close All But This";
            this.closeAllButThisToolStripMenuItem.Click += new System.EventHandler(this.closeAllButThisToolStripMenuItem_Click);
            // 
            // HistoricForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 492);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "HistoricForm";
            this.TabPageContextMenuStrip = this.ThisTabMenu;
            this.Text = "HistoricForm";
            this.Shown += new System.EventHandler(this.HistoricForm_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorGrid)).EndInit();
            this.ThisTabMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private HDesarrollo.Controles.Grid.CondicionMarcado MarkErrors;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private HDesarrollo.Controles.Grid.SmartGrid SGrid;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TxtRightExec;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox TxtTReturn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox TxtAffected;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TxtReaded;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TxtLapse;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TxtEnd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TxtStart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxtDb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView ErrorGrid;
        private ICSharpCode.TextEditor.TextEditorControl ScriptText;
        private System.Windows.Forms.ContextMenuStrip ThisTabMenu;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllButThisToolStripMenuItem;


    }
}