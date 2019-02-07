using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ClosedXML.Excel;

namespace Ez_SQL.Common_Code
{
    /// <summary>
    /// Type of exportations supported
    /// </summary>
    public enum ExportTo
    {
        /// <summary>
        /// Closed xml export
        /// </summary>
        XLSX,
        /// <summary>
        /// Text file, separated by a char
        /// </summary>
        TXT,
        /// <summary>
        /// Text file, separated by commas
        /// </summary>
        CSV
    };
    public enum ExcelStyle { Default, Simple };
    public enum ExcelCellStyle { Good, Bad, Neutral, Calculation, Check, Alert, None };
    public enum WidthAdjust { ByHeaders, ByFirst10Rows, ByFirst100Rows, ByAllRows, None }

    public class DataExporter
    {

        public bool DelimitedByLenght { get; set; }
        public char CharFiller { get; set; }
        public List<int> Widhts;
        public delegate void ExportEvent(DateTime FiredAt, int Records, int Progress, ExportTo ExportType);
        public delegate void ExportCompletedEvent(DateTime FiredAt, int Records, ExportTo ExportType, Stream StreamResult, string PathResult);
        public event ExportEvent OnStartExportation;
        public event ExportCompletedEvent OnCompletedExportation;
        public event ExportEvent OnProgress;
        public ExportTo ExportType { get; set; }
        public List<string> IgnoredColumns { get; set; }
        public ExcelStyle ExportExcelStyle { get; set; }
        public string Separator { get; set; }
        public List<CellRemark> Remarks { get; set; }
        public WidthAdjust AutoCellAdjust;

        public bool WriteHeaders { get; set; }
        public bool ExportWithStyles { get; set; }
        public bool UseAlternateRowStyles { get; set; }

        public string Author { get; set; }
        public string Company { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public bool UseDefaultSheetNames { get; set; }

        private string ThFilePath;
        private DataSet ThDataSet;
        private int ThRecordCount;
        private int ThCurrentRecord;
        private int ThCurPercentage;
        private bool ThIsStream;
        private Stream ThStream;

        private BackgroundWorker AsyncExporter;

        public DataExporter()
        {
            ExportType = ExportTo.XLSX;
            IgnoredColumns = new List<string>();
            WriteHeaders = true;
            ExportWithStyles = true;
            UseAlternateRowStyles = true;
            ExportExcelStyle = ExcelStyle.Default;
            Separator = "\t";
            Remarks = new List<CellRemark>();
            DelimitedByLenght = false;
            CharFiller = ' ';
            Widhts = new List<int>();
            AutoCellAdjust = WidthAdjust.ByHeaders;
            UseDefaultSheetNames = true;
        }

        public void ExportToFile(string FilePath, DataSet Data, bool GoAsync = false)
        {
            if (GoAsync)
            {
                if (AsyncExporter != null || (AsyncExporter != null && AsyncExporter.IsBusy))
                {
                    AsyncExporter.CancelAsync();
                    AsyncExporter = null;
                }
                AsyncExporter = new BackgroundWorker();
                AsyncExporter.WorkerReportsProgress = true;
                AsyncExporter.WorkerSupportsCancellation = false;
                AsyncExporter.DoWork += new DoWorkEventHandler(AsyncExporter_DoWork);
                AsyncExporter.ProgressChanged += new ProgressChangedEventHandler(AsyncExporter_ProgressChanged);
                AsyncExporter.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AsyncExporter_RunWorkerCompleted);

                ThDataSet = Data;
                ThFilePath = FilePath;
                ThCurPercentage = 0;
                ThCurrentRecord = 0;
                ThRecordCount = 0;
                ThIsStream = false;

                foreach (DataTable table in Data.Tables)
                {
                    ThRecordCount += table.Rows.Count;
                }

                if (OnStartExportation != null)
                    OnStartExportation(DateTime.Now, ThRecordCount, 0, ExportType);

                AsyncExporter.RunWorkerAsync();

            }
            else
            {
                if (File.Exists(FilePath))
                    File.Delete(FilePath);

                switch (ExportType)
                {
                    case ExportTo.XLSX:
                        CreateXLSX(Data, FilePath);
                        break;
                    case ExportTo.TXT:
                        Stream ResTxt = CreateTXT(Data);
                        ResTxt.Seek(0, SeekOrigin.Begin);
                        using (Stream file = File.OpenWrite(FilePath))
                        {
                            Extensions.CopyStream(ResTxt, file);
                        }
                        break;
                    case ExportTo.CSV:
                        Stream ResCsv = CreateCSV(Data);
                        ResCsv.Seek(0, SeekOrigin.Begin);
                        using (Stream file = File.OpenWrite(FilePath))
                        {
                            Extensions.CopyStream(ResCsv, file);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        public Stream ExportToStream(DataSet Data, bool GoAsync = false)
        {
            if (GoAsync)
            {
                if (AsyncExporter != null || (AsyncExporter != null && AsyncExporter.IsBusy))
                {
                    AsyncExporter.CancelAsync();
                    AsyncExporter = null;
                }
                AsyncExporter = new BackgroundWorker();
                AsyncExporter.WorkerReportsProgress = true;
                AsyncExporter.WorkerSupportsCancellation = false;
                AsyncExporter.DoWork += new DoWorkEventHandler(AsyncExporter_DoWork);
                AsyncExporter.ProgressChanged += new ProgressChangedEventHandler(AsyncExporter_ProgressChanged);
                AsyncExporter.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AsyncExporter_RunWorkerCompleted);

                ThDataSet = Data;
                ThFilePath = "";
                ThCurPercentage = 0;
                ThCurrentRecord = 0;
                ThRecordCount = 0;
                ThIsStream = true;

                foreach (DataTable table in Data.Tables)
                {
                    ThRecordCount += table.Rows.Count;
                }

                if (OnStartExportation != null)
                    OnStartExportation(DateTime.Now, ThRecordCount, 0, ExportType);

                AsyncExporter.RunWorkerAsync();
                return null;
            }
            else
            {
                switch (ExportType)
                {
                    case ExportTo.XLSX:
                        Stream ResX = CreateXLSX(Data);
                        ResX.Seek(0, SeekOrigin.Begin);
                        return ResX;
                    case ExportTo.TXT:
                        Stream ResTxt = CreateTXT(Data);
                        ResTxt.Seek(0, SeekOrigin.Begin);
                        return ResTxt;
                    case ExportTo.CSV:
                        Stream ResCsv = CreateCSV(Data);
                        ResCsv.Seek(0, SeekOrigin.Begin);
                        return ResCsv;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return null;
            }
        }

        #region Methods used by the background worker
        void AsyncExporter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (OnCompletedExportation != null)
            {
                if (ThIsStream)
                {
                    OnCompletedExportation(DateTime.Now, ThRecordCount, ExportType, ThStream, ThFilePath);
                }
                else
                {
                    OnCompletedExportation(DateTime.Now, ThRecordCount, ExportType, null, ThFilePath);
                }
            }
        }
        void AsyncExporter_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (OnProgress != null)
                OnProgress(DateTime.Now, ThRecordCount, ThCurrentRecord + 1, ExportType);
        }
        void AsyncExporter_DoWork(object sender, DoWorkEventArgs e)
        {
            switch (ExportType)
            {
                case ExportTo.XLSX:
                    if (!ThIsStream)
                    {
                        CreateXLSX(ThDataSet, ThFilePath);
                    }
                    else
                    {
                        ThStream = CreateXLSX(ThDataSet);
                        ThStream.Seek(0, SeekOrigin.Begin);
                    }
                    break;
                case ExportTo.TXT:

                    if (!ThIsStream)
                    {
                        using (Stream file = File.OpenWrite(ThFilePath))
                        {
                            Stream ResTxt = CreateTXT(ThDataSet);
                            ResTxt.Seek(0, SeekOrigin.Begin);
                            Extensions.CopyStream(ResTxt, file);
                        }
                    }
                    else
                    {
                        ThStream = CreateTXT(ThDataSet);
                        ThStream.Seek(0, SeekOrigin.Begin);
                    }
                    break;
                case ExportTo.CSV:
                    if (!ThIsStream)
                    {
                        using (Stream file = File.OpenWrite(ThFilePath))
                        {
                            Stream ResCsv = CreateCSV(ThDataSet);
                            ResCsv.Seek(0, SeekOrigin.Begin);
                            Extensions.CopyStream(ResCsv, file);
                        }
                    }
                    else
                    {
                        ThStream = CreateCSV(ThDataSet);
                        ThStream.Seek(0, SeekOrigin.Begin);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Overloads for the export method
        public void ExportToFile(string FilePath, DataTable Data, bool GoAsync = false)
        {
            DataSet helper = new DataSet();
            helper.Tables.Add(Data.Copy());
            ExportToFile(FilePath, helper, GoAsync);
        }
        public void ExportToFile<T>(string FilePath, IList<T> Data, bool GoAsync = false)
        {
            ExportToFile(FilePath, Extensions.ConvertToDataTable<T>(Data), GoAsync);
        }
        public void ExportToFile<K, V>(string FilePath, IDictionary<K, V> Data, bool GoAsync = false)
        {
            List<V> listBuffer = new List<V>();
            foreach (KeyValuePair<K, V> pair in Data)
            {
                listBuffer.Add(pair.Value);
            }
            ExportToFile(FilePath, Extensions.ConvertToDataTable<V>(listBuffer), GoAsync);
        }
        public void ExportToFile<T>(string FilePath, ICollection<T> Data, bool GoAsync = false)
        {
            List<T> listBuffer = new List<T>();
            foreach (T val in Data)
            {
                listBuffer.Add(val);
            }
            ExportToFile(FilePath, Extensions.ConvertToDataTable<T>(listBuffer), GoAsync);
        }
        public Stream ExportToStream(DataTable Data, bool GoAsync = false)
        {
            DataSet helper = new DataSet();
            helper.Tables.Add(Data.Copy());
            return ExportToStream(helper, GoAsync);
        }
        public Stream ExportToStream<T>(IList<T> Data, bool GoAsync = false)
        {
            return ExportToStream(Extensions.ConvertToDataTable<T>(Data), GoAsync);
        }
        public Stream ExportToStream<K, V>(IDictionary<K, V> Data, bool GoAsync = false)
        {
            List<V> listBuffer = new List<V>();
            foreach (KeyValuePair<K, V> pair in Data)
            {
                listBuffer.Add(pair.Value);
            }
            return ExportToStream(Extensions.ConvertToDataTable<V>(listBuffer), GoAsync);
        }
        public Stream ExportToStream<T>(ICollection<T> Data, bool GoAsync = false)
        {
            List<T> listBuffer = new List<T>();
            foreach (T val in Data)
            {
                listBuffer.Add(val);
            }
            return ExportToStream(Extensions.ConvertToDataTable<T>(listBuffer), GoAsync);
        }
        #endregion

        #region Code to generate the XLSX file(Excel 2007-2010)
        private Stream CreateXLSX(DataSet dataSet, string filePath = "")
        {
            XLWorkbook book = new XLWorkbook();
            if (!String.IsNullOrEmpty(Author))
                book.Properties.Author = Author;
            if (!String.IsNullOrEmpty(Company))
                book.Properties.Company = Company;
            if (!String.IsNullOrEmpty(Subject))
                book.Properties.Subject = Subject;
            if (!String.IsNullOrEmpty(Title))
                book.Properties.Title = Title;

            foreach (DataTable table in dataSet.Tables)
            {
                CreateWorkSheetXLSX(book, table);
            }

            if (!String.IsNullOrEmpty(filePath))
            {
                book.SaveAs(filePath);
                return null;
            }
            else
            {
                MemoryStream back = new MemoryStream();
                book.SaveAs(back);
                return back;
            }
        }
        private void CreateWorkSheetXLSX(XLWorkbook closedXMLBook, DataTable data)
        {
            int sheetCount = closedXMLBook.Worksheets.Count;
            int curRow = 1, curCol = 1, autoAdjustCells;
            string sheetName = UseDefaultSheetNames ? "Data" + (sheetCount > 0 ? " " + sheetCount.ToString() : "") : data.TableName;
            var newSheet = closedXMLBook.AddWorksheet(sheetName);

            switch (AutoCellAdjust)
            {
                case WidthAdjust.ByHeaders:
                    autoAdjustCells = 0;
                    break;
                case WidthAdjust.ByFirst10Rows:
                    autoAdjustCells = 10;
                    break;
                case WidthAdjust.ByFirst100Rows:
                    autoAdjustCells = 100;
                    break;
                case WidthAdjust.ByAllRows:
                    autoAdjustCells = 10000;
                    break;
                default:
                case WidthAdjust.None:
                    autoAdjustCells = -1;
                    break;
            }


            #region Style for the headers
            newSheet.Row(1).Height = 35;
            var headerStyle = newSheet.Cell(1, 1).Style;
            #endregion

            #region Define if the writing of the headers is needed
            if (WriteHeaders)
            {
                foreach (DataColumn col in data.Columns)
                {
                    string colName = String.IsNullOrEmpty(col.Caption) ? col.ColumnName : col.Caption;
                    if (IgnoredColumns != null && IgnoredColumns.Count > 0 && IgnoredColumns.IsStringOnList(colName, false))
                        continue;

                    newSheet.Cell(curRow, curCol).Value = colName;
                    if (ExportWithStyles)
                    {
                        newSheet.Cell(curRow, curCol).Style.HeaderStyle(ExportExcelStyle);
                    }
                    else
                    {
                        newSheet.Cell(curRow, curCol).Style.NoStyle();
                    }
                    curCol++;
                }
                curRow++;
                curCol = 1;
            }
            if (autoAdjustCells == 0)
            {
                newSheet.Columns().AdjustToContents();
            }

            #endregion

            #region Finally write the details of the data
            int tabLength = data.Rows.Count;
            int rowLength = data.Columns.Count;

            for (int rowNum = 0; rowNum < tabLength; rowNum++)
            {
                for (int colNum = 0; colNum < rowLength; colNum++)
                {
                    DataColumn col = data.Columns[colNum];
                    string colName = String.IsNullOrEmpty(col.Caption) ? col.ColumnName : col.Caption;

                    if (IgnoredColumns != null && IgnoredColumns.Count > 0 && IgnoredColumns.IsStringOnList(colName, false))
                        continue;

                    if (ExportWithStyles)
                    {
                        if (UseAlternateRowStyles)
                        {
                            if (rowNum % 2 == 0)
                            {
                                newSheet.Cell(curRow, curCol).Style.AlternateStyle(ExportExcelStyle);
                            }
                            else
                            {
                                newSheet.Cell(curRow, curCol).Style.NormalStyle(ExportExcelStyle);
                            }
                        }
                        else
                        {
                            newSheet.Cell(curRow, curCol).Style.NormalStyle(ExportExcelStyle);
                        }
                    }
                    else
                    {
                        newSheet.Cell(curRow, curCol).Style.NoStyle();
                    }



                    if (data.Rows[rowNum][colNum] == DBNull.Value || String.IsNullOrEmpty(data.Rows[rowNum][colNum].ToString()))
                    {
                        newSheet.Cell(curRow, curCol).Value = "";
                    }
                    else if (data.Rows[rowNum][colNum].ToString().StartsWith("="))
                    {
                        newSheet.Cell(curRow, curCol).FormulaA1 = data.Rows[rowNum][colNum].ToString();
                    }
                    else
                    {
                        newSheet.Cell(curRow, curCol).Value = data.Rows[rowNum][colNum];
                    }

                    if (Remarks != null && Remarks.Count > 0 && Remarks.Any(x => x.Row == curRow && x.Col == curCol))
                    {
                        CellRemark remark = Remarks.FirstOrDefault(x => x.Row == curRow && x.Col == curCol);
                        newSheet.Cell(curRow, curCol).Style.SetCustomStyle(remark.Style);
                        if (String.IsNullOrEmpty(remark.Comment))
                            newSheet.Cell(curRow, curCol).Comment.AddText(remark.Comment);
                    }

                    curCol++;


                }

                if (autoAdjustCells > 0 && ((rowNum + 1) >= autoAdjustCells))
                {
                    autoAdjustCells = -1;
                    newSheet.Columns().AdjustToContents();
                }

                ThCurrentRecord++;
                if (AsyncExporter != null && AsyncExporter.IsBusy)
                {
                    if (ThRecordCount > 100)
                    {
                        if (ThCurrentRecord % (ThRecordCount / 100) == 0)
                        {
                            AsyncExporter.ReportProgress(((ThCurrentRecord) * 100) / ThRecordCount);
                            ThCurPercentage = ((ThCurrentRecord) * 100) / ThRecordCount;
                        }
                    }
                }

                curCol = 1;
                curRow++;

            }
            if (autoAdjustCells > 0 && (autoAdjustCells >= tabLength))
            {
                newSheet.Columns().AdjustToContents();
            }

            #endregion

        }
        #endregion

        #region Code to generate the text file
        private Stream CreateTXT(DataSet dataSet)
        {
            MemoryStream memoryStream = new MemoryStream();
            TextWriter tw = new StreamWriter(memoryStream);

            if (!DelimitedByLenght)
            {
                #region Code used for text files with separators
                foreach (DataTable table in dataSet.Tables)
                {
                    string curLine;
                    int colCount = table.Columns.Count;

                    if (WriteHeaders)
                    {
                        curLine = "";
                        for (int i = 0; i < colCount; i++)
                        {
                            string colName = String.IsNullOrEmpty(table.Columns[i].Caption) ? table.Columns[i].ColumnName : table.Columns[i].Caption;
                            if (IgnoredColumns != null && IgnoredColumns.Count > 0 && IgnoredColumns.IsStringOnList(colName, false))
                                continue;
                            curLine += String.Format("{0}{1}", colName, Separator);
                        }
                        tw.WriteLine(curLine.Substring(0, curLine.Length - Separator.Length));
                    }

                    for (int curRow = 0; curRow < table.Rows.Count; curRow++)
                    {
                        curLine = "";
                        for (int curCol = 0; curCol < colCount; curCol++)
                        {
                            string colName = table.Columns[curCol].ColumnName;
                            if (IgnoredColumns != null && IgnoredColumns.Count > 0 && IgnoredColumns.IsStringOnList(colName, false))
                                continue;
                            if (table.Rows[curRow][curCol] != null && table.Rows[curRow][curCol] != DBNull.Value)
                            {
                                curLine += String.Format("{0}{1}", table.Rows[curRow][curCol].ToString(), Separator);
                            }
                            else
                            {
                                curLine += String.Format("{0}", Separator);
                            }
                        }
                        tw.WriteLine(curLine.Substring(0, curLine.Length - Separator.Length));
                    }

                    ThCurrentRecord++;
                    if (AsyncExporter != null && AsyncExporter.IsBusy)
                    {
                        if (ThRecordCount > 100)
                        {
                            if (ThCurrentRecord % (ThRecordCount / 100) == 0)
                            {
                                AsyncExporter.ReportProgress(((ThCurrentRecord) * 100) / ThRecordCount);
                                ThCurPercentage = ((ThCurrentRecord) * 100) / ThRecordCount;
                            }
                        }
                    }
                }
                tw.Flush();
                #endregion
            }
            else
            {
                #region Code used for text files with delimited length
                foreach (DataTable table in dataSet.Tables)
                {
                    string curLine;
                    int colCount = table.Columns.Count;

                    if (WriteHeaders)
                    {
                        curLine = "";
                        for (int i = 0; i < colCount; i++)
                        {
                            string colName = table.Columns[i].ColumnName;
                            if (IgnoredColumns != null && IgnoredColumns.Count > 0 && IgnoredColumns.IsStringOnList(colName, false))
                                continue;
                            curLine += colName.PadRight(Widhts[i], CharFiller);
                        }
                        tw.WriteLine(curLine);
                    }
                    for (int curRow = 0; curRow < table.Rows.Count; curRow++)
                    {
                        curLine = "";
                        for (int curCol = 0; curCol < colCount; curCol++)
                        {
                            string colName = table.Columns[curCol].ColumnName;
                            if (IgnoredColumns != null && IgnoredColumns.Count > 0 && IgnoredColumns.IsStringOnList(colName, false))
                                continue;
                            curLine += table.Rows[curRow][curCol].ToString().PadRight(Widhts[curCol], CharFiller);
                        }
                        tw.WriteLine(curLine);
                    }

                    ThCurrentRecord++;
                    if (AsyncExporter != null && AsyncExporter.IsBusy)
                    {
                        if (ThRecordCount > 100)
                        {
                            if (ThCurrentRecord % (ThRecordCount / 100) == 0)
                            {
                                AsyncExporter.ReportProgress(((ThCurrentRecord) * 100) / ThRecordCount);
                                ThCurPercentage = ((ThCurrentRecord) * 100) / ThRecordCount;
                            }
                        }
                    }


                }
                tw.Flush();
                #endregion
            }

            return memoryStream;
        }
        #endregion

        #region Code to generate the csv file
        private Stream CreateCSV(DataSet dataSet)
        {
            string csvSeparator = ",";
            MemoryStream memoryStream = new MemoryStream();
            TextWriter tw = new StreamWriter(memoryStream);
            foreach (DataTable table in dataSet.Tables)
            {
                string curLine;
                int colCount = table.Columns.Count;

                if (WriteHeaders)
                {
                    curLine = "";
                    for (int i = 0; i < colCount; i++)
                    {
                        string colName = table.Columns[i].ColumnName;
                        if (IgnoredColumns != null && IgnoredColumns.Count > 0 && IgnoredColumns.IsStringOnList(colName, false))
                            continue;
                        curLine += String.Format("{0}{1}", colName.EnsureCSVField(), csvSeparator);
                    }
                    tw.WriteLine(curLine.Substring(0, curLine.Length - Separator.Length));
                }

                for (int curRow = 0; curRow < table.Rows.Count; curRow++)
                {
                    curLine = "";
                    for (int curCol = 0; curCol < colCount; curCol++)
                    {
                        string colName = table.Columns[curCol].ColumnName;
                        if (IgnoredColumns != null && IgnoredColumns.Count > 0 && IgnoredColumns.IsStringOnList(colName, false))
                            continue;
                        if (table.Rows[curRow][curCol] != null && table.Rows[curRow][curCol] != DBNull.Value)
                        {
                            curLine += String.Format("{0}{1}", table.Rows[curRow][curCol].ToString().EnsureCSVField(), csvSeparator);
                        }
                        else
                        {
                            curLine += String.Format("{0}", csvSeparator);
                        }
                    }
                    tw.WriteLine(curLine.Substring(0, curLine.Length - Separator.Length));
                }

                ThCurrentRecord++;
                if (AsyncExporter != null && AsyncExporter.IsBusy)
                {
                    if (ThCurrentRecord % (ThRecordCount / 100) == 0)
                    {
                        AsyncExporter.ReportProgress(((ThCurrentRecord) * 100) / ThRecordCount);
                        ThCurPercentage = ((ThCurrentRecord) * 100) / ThRecordCount;
                    }
                }
            }
            tw.Flush();
            return memoryStream;
        }
        #endregion

    }
    internal static class ClosedHelper
    {
        public static void HeaderStyle(this IXLStyle style, ExcelStyle ExportStyle)
        {
            style.Font.SetFontSize(12);
            style.Font.Bold = true;
            style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            style.Alignment.WrapText = true;
            switch (ExportStyle)
            {
                case ExcelStyle.Default:
                    style.Fill.BackgroundColor = XLColor.Navy;
                    style.Font.FontColor = XLColor.White;
                    style.Border.BottomBorderColor = XLColor.Black;
                    style.Border.BottomBorder = XLBorderStyleValues.Thick;
                    style.Border.TopBorderColor = XLColor.Black;
                    style.Border.TopBorder = XLBorderStyleValues.Thick;
                    style.Border.LeftBorderColor = XLColor.Black;
                    style.Border.LeftBorder = XLBorderStyleValues.Thick;
                    style.Border.RightBorderColor = XLColor.Black;
                    style.Border.RightBorder = XLBorderStyleValues.Thick;
                    break;
                case ExcelStyle.Simple:
                    style.Fill.BackgroundColor = XLColor.White;
                    style.Font.FontColor = XLColor.Black;
                    style.Border.BottomBorder = XLBorderStyleValues.Thick;
                    style.Border.BottomBorderColor = XLColor.Gray;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("ExportStyle");
            }
        }

        public static void NormalStyle(this IXLStyle style, ExcelStyle ExportStyle)
        {
            //normal style
            style.Font.Bold = false;
            style.Font.SetFontSize(10);
            style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

            switch (ExportStyle)
            {
                case ExcelStyle.Default:
                    style.Fill.BackgroundColor = XLColor.White;
                    style.Font.FontColor = XLColor.Navy;
                    style.Border.BottomBorderColor = XLColor.Black;
                    style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    style.Border.TopBorderColor = XLColor.Black;
                    style.Border.TopBorder = XLBorderStyleValues.Thin;
                    style.Border.LeftBorderColor = XLColor.Black;
                    style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    style.Border.RightBorderColor = XLColor.Black;
                    style.Border.RightBorder = XLBorderStyleValues.Thin;
                    break;
                case ExcelStyle.Simple:
                    style.Fill.BackgroundColor = XLColor.White;
                    style.Font.FontColor = XLColor.Black;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("ExportStyle");
            }
        }

        public static void AlternateStyle(this IXLStyle style, ExcelStyle ExportStyle)
        {
            //alt row style
            style.Font.Bold = false;
            style.Font.SetFontSize(10);
            style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;


            switch (ExportStyle)
            {
                case ExcelStyle.Default:
                    style.Fill.BackgroundColor = XLColor.LightSkyBlue;
                    style.Font.FontColor = XLColor.Navy;
                    style.Border.BottomBorderColor = XLColor.Black;
                    style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    style.Border.TopBorderColor = XLColor.Black;
                    style.Border.TopBorder = XLBorderStyleValues.Thin;
                    style.Border.LeftBorderColor = XLColor.Black;
                    style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    style.Border.RightBorderColor = XLColor.Black;
                    style.Border.RightBorder = XLBorderStyleValues.Thin;
                    break;
                case ExcelStyle.Simple:
                    style.Fill.BackgroundColor = XLColor.WhiteSmoke;
                    style.Font.FontColor = XLColor.Black;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("ExportStyle");
            }
        }

        public static void NoStyle(this IXLStyle style)
        {
            style.Fill.BackgroundColor = XLColor.White;
            style.Font.Bold = false;
            style.Font.SetFontSize(10);
            style.Font.FontColor = XLColor.Black;
            style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
        }

        public static void SetCustomStyle(this IXLStyle style, ExcelCellStyle cellStyle)
        {
            switch (cellStyle)
            {
                case ExcelCellStyle.Good:
                    style.Fill.BackgroundColor = XLColor.PaleGreen;
                    style.Font.FontColor = XLColor.DarkGreen;
                    break;
                case ExcelCellStyle.Bad:
                    style.Fill.BackgroundColor = XLColor.LightCoral;
                    style.Font.FontColor = XLColor.Firebrick;
                    break;
                case ExcelCellStyle.Neutral:
                    style.Fill.BackgroundColor = XLColor.Yellow;
                    style.Font.FontColor = XLColor.DarkOrange;
                    break;
                case ExcelCellStyle.Calculation:
                    style.Fill.BackgroundColor = XLColor.Silver;
                    style.Font.FontColor = XLColor.DarkOrange;
                    break;
                case ExcelCellStyle.Check:
                    style.Fill.BackgroundColor = XLColor.DimGray;
                    style.Font.FontColor = XLColor.Orange;
                    style.Font.Bold = true;
                    style.Border.BottomBorderColor = XLColor.Black;
                    style.Border.BottomBorder = XLBorderStyleValues.Thick;
                    style.Border.TopBorderColor = XLColor.Black;
                    style.Border.TopBorder = XLBorderStyleValues.Thick;
                    style.Border.LeftBorderColor = XLColor.Black;
                    style.Border.LeftBorder = XLBorderStyleValues.Thick;
                    style.Border.RightBorderColor = XLColor.Black;
                    style.Border.RightBorder = XLBorderStyleValues.Thick;
                    break;
                case ExcelCellStyle.Alert:
                    style.Fill.BackgroundColor = XLColor.Red;
                    style.Font.FontColor = XLColor.White;
                    style.Font.Bold = true;
                    style.Border.BottomBorderColor = XLColor.Black;
                    style.Border.BottomBorder = XLBorderStyleValues.Thick;
                    style.Border.TopBorderColor = XLColor.Black;
                    style.Border.TopBorder = XLBorderStyleValues.Thick;
                    style.Border.LeftBorderColor = XLColor.Black;
                    style.Border.LeftBorder = XLBorderStyleValues.Thick;
                    style.Border.RightBorderColor = XLColor.Black;
                    style.Border.RightBorder = XLBorderStyleValues.Thick;
                    break;
                case ExcelCellStyle.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("cellStyle");
            }
        }

    }
    public class CellRemark
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public string Comment { get; set; }
        public ExcelCellStyle Style { get; set; }

        public CellRemark()
        {
            Style = ExcelCellStyle.Bad;
        }
    }
}
