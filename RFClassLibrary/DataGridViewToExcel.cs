namespace RFClassLibrary
{

    using System;
    using System.Windows.Forms;
    using System.IO;
    using Microsoft.Office.Interop.Excel;
    using Microsoft.Office.Core;
    using System.Runtime.InteropServices; // For COMException
    using System.Diagnostics; // to ensure EXCEL process is really killed

    //class DataGridViewToExcel
    //{
    //}
    static class Program
    {
        /// <SUMMARY>
        /// The main entry point for the application.
        /// </SUMMARY>
        [STAThread]
        static void Main()
        {
            Microsoft.Office.Interop.Excel.Application moiea = new Microsoft.Office.Interop.Excel.Application();
            //moiea.EnableVisualStyles();
            //Microsoft.Office.Interop.Excel.Application.SetCompatibleTextRenderingDefault(false);
            //moiea.SetCompatibleTextRenderingDefault(false);
            //Microsoft.Office.Interop.Excel.Application.Run(new Form1());
            //moiea.Run(new Form1());
        }
    }

    // This is where all the real office automation stuff occurs.
    namespace GridviewToExcel
    {
        #region export2Excel CLASS
        /// This class processes the DataView that it is provided and
        /// Exports this DataView to an Excel document.
        public class export2Excel
        {

            #region InstanceFields
            //Instance Fields
            /// <summary>
            /// Progress Handler delegate
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public delegate void ProgressHandler(object sender,
                                 ProgressEventArgs e);
            /// <summary>
            /// ProgressHandler event
            /// </summary>
            public event ProgressHandler prg;
            private DataGridView dv;
            private Style styleRows;
            private Style styleColumnHeadings;
            private Microsoft.Office.Interop.Excel.Application EXL;
            private Workbook workbook;
            private Sheets sheets;
            private Worksheet worksheet;
            private string[,] myTemplateValues;
            private int position;
            #endregion

            #region Constructor
            
            /// <summary>
            /// Constructs a new export2Excel object. The user must
            /// call the createExcelDocument method once a valid export2Excel
            /// object has been instantiated
            /// </summary>
            public export2Excel()
            {

            }
            #endregion

            #region EXCEL : ExportToExcel
            
            /// <summary>
            /// Exports a DataView to Excel. The following steps are carried out
            /// in order to export the DataView to Excel
            /// Create Excel Objects
            /// Create Column and Row Workbook Cell Rendering Styles
            /// Fill Worksheet With DataView
            /// Add Auto Shapes To Excel Worksheet
            /// Select All Used Cells
            /// Create Headers/Footers
            /// Set Status Finished
            /// Save workbook and Tidy up all objects
            /// @param dv : DataView to use
            /// @param path : The path to save/open the EXCEL file to/from
            /// @param sheetName : The target sheet within the EXCEL file
            /// </summary>
            /// <param name="dv"></param>
            /// <param name="path"></param>
            /// <param name="sheetName"></param>
            public void ExportToExcel(DataGridView dv, string path, string sheetName)
            {
                try
                {
                    this.dv = dv; //Assign Instance Fields

                    #region NEW EXCEL DOCUMENT : Create Excel Objects

                    //create new EXCEL application
                    EXL = new Microsoft.Office.Interop.Excel.ApplicationClass();
                    
                    //index to hold location of the requested sheetName 
                    //in the workbook sheets
                    //collection
                    int indexOfsheetName;

                    #region FILE EXISTS
                    //Does the file exist for the given path
                    if (File.Exists(path))
                    {

                        //Yes file exists, so open the file
                        workbook = EXL.Workbooks.Open(path,
                            0, false, 5, "", "", false,
                            Microsoft.Office.Interop.Excel.XlPlatform.xlWindows,
                            "", true, false, 0, true, false, false);

                        //get the workbook sheets collection
                        sheets = workbook.Sheets;

                        //set the location of the requested sheetName to -1, 
                        //need to find where
                        //it is. It may not actually exist
                        indexOfsheetName = -1;

                        //loop through the sheets collection
                        for (int i = 1; i <= sheets.Count; i++)
                        {
                            //get the current worksheet at index (i)
                            worksheet = (Worksheet)sheets.get_Item(i);

                            //is the current worksheet the sheetName 
                            //that was requested
                            if (worksheet.Name.ToString().Equals(sheetName))
                            {
                                //yes it is, so store its index
                                indexOfsheetName = i;

                                //Select all cells, and clear the contents
                                Microsoft.Office.Interop.Excel.Range myAllRange =
                                                                 worksheet.Cells;
                                myAllRange.Select();
                                myAllRange.CurrentRegion.Select();
                                myAllRange.ClearContents();
                            }
                        }

                        //At this point it is known that the sheetName 
                        //that was requested
                        //does not exist within the found file, 
                        //so create a new sheet within the
                        //sheets collection
                        if (indexOfsheetName == -1)
                        {
                            //Create a new sheet for the requested sheet
                            Worksheet sh = (Worksheet)workbook.Sheets.Add(
                                Type.Missing,
                               (Worksheet)sheets.get_Item(sheets.Count),
                                Type.Missing, Type.Missing);
                            //Change its name to that requested
                            sh.Name = sheetName;
                        }
                    }
                    #endregion

                    #region FILE DOESNT EXIST
                    //No the file DOES NOT exist, so create a new file
                    else
                    {
                        //Add a new workbook to the file
                        workbook =
                          EXL.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                        //get the workbook sheets collection
                        sheets = workbook.Sheets;
                        //get the new sheet
                        worksheet = (Worksheet)sheets.get_Item(1);
                        //Change its name to that requested
                        worksheet.Name = sheetName;
                    }
                    #endregion

                    #region get correct worksheet index for requested sheetName

                    //get the workbook sheets collection
                    sheets = workbook.Sheets;

                    //set the location of the requested sheetName 
                    //to -1, need to find where
                    //it is. It will definately exist now 
                    //as it has just been added
                    indexOfsheetName = -1;

                    //loop through the sheets collection
                    for (int i = 1; i <= sheets.Count; i++)
                    {
                        //get the current worksheet at index (i)
                        worksheet = (Worksheet)sheets.get_Item(i);

                        //is the current worksheet the sheetName 
                        //that was requested
                        if (worksheet.Name.ToString().Equals(sheetName))
                        {
                            //yes it is, so store its index
                            indexOfsheetName = i;
                        }
                    }

                    //set the worksheet that the DataView should 
                    //write to, to the known index of the
                    //requested sheet
                    worksheet = (Worksheet)sheets.get_Item(indexOfsheetName);
                    #endregion

                    #endregion

                    // Set styles 1st
                    SetUpStyles();
                    //Fill EXCEL worksheet with DataView values
                    fillWorksheet_WithDataView();
                    //Add the autoshapes to EXCEL
                    //AddAutoShapesToExcel(); wdk 20100917 removed
                    //Select all used cells within current worksheet
                    SelectAllUsedCells();
                    
                    try
                    {
                        
                        workbook.Close(true, path, Type.Missing);
                        EXL.UserControl = false;
                        EXL.Quit();
                        EXL = null;
                        //kill the EXCEL process as a safety measure
                        killExcel();
                        // Show that processing is finished
                        ProgressEventArgs pe = new ProgressEventArgs(100);
                        OnProgressChange(pe);
                        MessageBox.Show("Finished adding " +
                          "dataview to Excel", "Info",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
                    }
                    catch (COMException)
                    {
                        //02/07/2008 rgc/wdk added  
                        // When the user responds to the MessageBox() the workbook.Close() fails on 
                        // the process
                        EXL.UserControl = false;
                        EXL.Quit();
                        EXL = null;
                        //kill the EXCEL process as a safety measure
                        killExcel();
                        //02/07/2008 end 
                        MessageBox.Show("User closed Excel manually, " +
                                        "so we don't have to do that");
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex.Message);
                }
            }
            #endregion

            #region EXCEL : UseTemplate
           
            /// <summary>
            ///  Exports a DataView to Excel. The following steps are carried out
            /// in order to export the DataView to Excel
            /// Create Excel Objects And Open Template File
            /// Select All Used Cells
            /// Create Headers/Footers
            /// Set Status Finished
            /// Save workbook and Tidy up all objects
            /// @param path : The path to save/open the EXCEL file to/from
            /// </summary>
            /// <param name="path"></param>
            /// <param name="templatePath"></param>
            /// <param name="myTemplateValues"></param>
            public void UseTemplate(string path, string templatePath,
                                    string[,] myTemplateValues)
            {
                try
                {
                    this.myTemplateValues = myTemplateValues;
                    //create new EXCEL application
                    EXL = new Microsoft.Office.Interop.Excel.ApplicationClass();
                    //Yes file exists, so open the file
                    workbook = EXL.Workbooks.Open(templatePath,
                        0, false, 5, "", "", false,
                        Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "",
                        true, false, 0, true, false, false);
                    //get the workbook sheets collection
                    sheets = workbook.Sheets;
                    //get the new sheet
                    worksheet = (Worksheet)sheets.get_Item(1);
                    //Change its name to that requested
                    worksheet.Name = "ATemplate";
                    //Fills the Excel Template File Selected With A 2D Test Array
                    fillTemplate_WithTestValues();
                    //Select all used cells within current worksheet
                    SelectAllUsedCells();

                    try
                    {
                        workbook.Close(true, path, Type.Missing);
                        EXL.UserControl = false;
                        EXL.Quit();
                        EXL = null;
                        //kill the EXCEL process as a safety measure
                        killExcel();
                        // Show that processing is finished
                        ProgressEventArgs pe = new ProgressEventArgs(100);
                        OnProgressChange(pe);
                        MessageBox.Show("Finished adding test values to " +
                                        "Template", "Info",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                    }
                    catch (COMException)
                    {
                        Console.WriteLine("User closed Excel manually," +
                                          " so we don't have to do that");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex.Message);
                }
            }
            #endregion

            #region STEP 1 : Create Column & Row Workbook Cell Rendering Styles
            //Creates 2 Custom styles for the workbook These styles are
            //  styleColumnHeadings
            //  styleRows
            //These 2 styles are used when filling 
            //the individual Excel cells with the
            //DataView values. If the current cell relates 
            //to a DataView column heading
            //then the style styleColumnHeadings will be used 
            //to render the current cell.
            //If the current cell relates to a DataView row 
            //then the style styleRows will
            //be used to render the current cell.
            private void SetUpStyles()
            {
                // Style styleColumnHeadings
                try
                {
                    styleColumnHeadings = workbook.Styles["styleColumnHeadings"];
                }
                // Style doesn't exist yet.
                catch
                {
                    styleColumnHeadings =
                       workbook.Styles.Add("styleColumnHeadings", Type.Missing);
                    styleColumnHeadings.Font.Name = "Arial";
                    styleColumnHeadings.Font.Size = 14;
                    styleColumnHeadings.Font.Color =
                      (255 << 16) | (255 << 8) | 255;
                    styleColumnHeadings.Interior.Color =
                      (0 << 16) | (0 << 8) | 0;
                    styleColumnHeadings.Interior.Pattern =
                       Microsoft.Office.Interop.Excel.XlPattern.xlPatternSolid;
                }
                // Style styleRows
                try
                {

                    styleRows = workbook.Styles["styleRows"];
                }
                // Style doesn't exist yet.
                catch
                {
                    styleRows = workbook.Styles.Add("styleRows", Type.Missing);
                    styleRows.Font.Name = "Arial";
                    styleRows.Font.Size = 10;
                    styleRows.Font.Color = (0 << 16) | (0 << 8) | 0;
                    styleRows.Interior.Color =
                        (192 << 16) | (192 << 8) | 192;
                    styleRows.Interior.Pattern =
                         Microsoft.Office.Interop.Excel.XlPattern.xlPatternSolid;
                }
            }
            #endregion

            #region STEP 2 : Fill Worksheet With DataView
            //Fills an Excel worksheet with the values contained in the DataView
            //parameter
            private void fillWorksheet_WithDataView()
            {
                position = 0;
                //Add DataView Columns To Worksheet
                int row = 1;
                int col = 1;
                // Loop thought the columns
                for (int i = 0; i < dv.Columns.Count; i++)
                {

                    fillExcelCell(worksheet, row, col++,
                                  dv.Columns[i].HeaderText,
                                  styleColumnHeadings.Name);
                }

                //Add DataView Rows To Worksheet
                row = 2;
                col = 1;

                for (int i = 0; i < dv.Rows.Count; i++)
                {
                    // 12/21/2007 wdk modified to not walk to the last uninitialized row.
                    if (dv.Rows[i].IsNewRow)
                    {
                        break;
                    }
                    for (int j = 0; j < dv.Columns.Count; j++)
                    {
                        //fillExcelCell(worksheet, row, col++, dv[i][j].ToString(),
                        //              styleRows.Name);
                        fillExcelCell(worksheet, row, col++, dv[j,i].Value.ToString(),
                                     styleRows.Name);
                    }
                    col = 1;
                    row++;

                    position = (100 / dv.Rows.Count) * row + 2;
                    ProgressEventArgs pe = new ProgressEventArgs(position);
                    OnProgressChange(pe);

                }
            }
            #endregion

            #region STEP 3 : Fill Individual Cell and Render Using Predefined Style
            //Formats the current cell based on the Style setting parameter name
            //provided here
            //@param worksheet : The worksheet
            //@param row : Current row
            //@param col : Current Column
            //@param Value : The value for the cell
            //@param StyleName : The style name to use
            private void fillExcelCell(Worksheet worksheet, int row, int col,
                                       Object Value, string StyleName)
            {
                Range rng = (Range)worksheet.Cells[row, col];
                rng.Select();
                rng.Value2 = Value.ToString();
                rng.Style = StyleName;
                rng.Columns.EntireColumn.AutoFit();
                rng.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                rng.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                rng.Borders.ColorIndex = Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic;
            }
            #endregion

            #region STEP 4 : Add Auto Shapes To Excel Worksheet
            //Add some WordArt objecs to the Excel worksheet
            private void AddAutoShapesToExcel()
            {

                //Method fields
                float txtSize = 80;
                float Left = 100.0F;
                float Top = 100.0F;
                //Have 2 objects
                int[] numShapes = new int[2];
                Microsoft.Office.Interop.Excel.Shape[] myShapes =
                  new Microsoft.Office.Interop.Excel.Shape[numShapes.Length];

                try
                {
                    //loop through the object count
                    for (int i = 0; i < numShapes.Length; i++)
                    {

                        //Add the object to Excel
                        myShapes[i] =
                         worksheet.Shapes.AddTextEffect(
                         MsoPresetTextEffect.msoTextEffect1, "DRAFT",
                         "Arial Black", txtSize, MsoTriState.msoFalse,
                         MsoTriState.msoFalse, (Left * (i * 3)), Top);

                        //Manipulate the object settings
                        myShapes[i].Rotation = 45F;
                        myShapes[i].Fill.Visible =
                         Microsoft.Office.Core.MsoTriState.msoFalse;
                        myShapes[i].Fill.Transparency = 0F;
                        myShapes[i].Line.Weight = 1.75F;
                        myShapes[i].Line.DashStyle =
                          MsoLineDashStyle.msoLineSolid;
                        myShapes[i].Line.Transparency = 0F;
                        myShapes[i].Line.Visible =
                           Microsoft.Office.Core.MsoTriState.msoTrue;
                        myShapes[i].Line.ForeColor.RGB =
                          (0 << 16) | (0 << 8) | 0;
                        myShapes[i].Line.BackColor.RGB =
                          (255 << 16) | (255 << 8) | 255;
                    }
                }
                catch (Exception )
                {
                }

            }
            #endregion

            #region STEP 5 : Select All Used Cells
            //Selects all used cells for the Excel worksheet
            private void SelectAllUsedCells()
            {

                Microsoft.Office.Interop.Excel.Range myAllRange = worksheet.Cells;
                myAllRange.Select();
                myAllRange.CurrentRegion.Select();

            }
            #endregion

            #region STEP 6 : Fill Template With Test Values
            //Fills the Excel Template File Selected With 
            //A 2D Test Array parameter
            private void fillTemplate_WithTestValues()
            {
                //Initilaise the correct Start Row/Column to match the Template
                int StartRow = 3;
                int StartCol = 2;

                position = 0;

                // Display the array elements within the Output 
                // window, make sure its correct before
                for (int i = 0; i <= myTemplateValues.GetUpperBound(0); i++)
                {
                    //loop through array and put into EXCEL template
                    for (int j = 0;
                         j <= myTemplateValues.GetUpperBound(1);
                         j++)
                    {
                        //update position in progress bar
                        position = (100 / myTemplateValues.Length) * i;
                        ProgressEventArgs pe = new ProgressEventArgs(position);
                        OnProgressChange(pe);

                        //put into EXCEL template
                        Range rng = (Range)worksheet.Cells[StartRow, StartCol++];
                        rng.Select();
                        rng.Value2 = myTemplateValues[i, j].ToString();
                        rng.Rows.EntireRow.AutoFit();
                    }
                    //New row, so column needs to be reset
                    StartCol = 2;
                    StartRow++;
                }
            }

            #endregion

            #region Kill EXCEL
            //As a safety check go through all processes and make
            //doubly sure excel is shutdown. Working with COM
            //have sometimes noticed that the EXL.Quit() call
            //doesn't always do the job
            private void killExcel()
            {
                try
                {
                    Process[] ps = Process.GetProcesses();
                   
                    foreach (Process p in ps)
                    {
                        if (p.ProcessName.ToLower().Equals("excel"))
                        {
                            p.Kill();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR " + ex.Message);
                }
            }
            #endregion

            #region Events
            /// Raises the OnProgressChange event for the parent form. 
            public virtual void OnProgressChange(ProgressEventArgs e)
            {
                // Invokes the delegates. 
                prg?.Invoke(this, e);
            }
            #endregion
        }
        #endregion
        #region ProgressEventArgs CLASS
        /// Provides the ProgressEventArgs 
        public class ProgressEventArgs : EventArgs
        {
            #region Instance Fields
            //Instance fields
            private int prgValue = 0;
            #endregion
            #region Public Constructor
            /// Constructs a new ProgressEventArgs object
            /// using the parameters provided
            /// @param prgValue : new progress value
            public ProgressEventArgs(int prgValue)
            {
                this.prgValue = prgValue;
            }
            #endregion
            #region Public Methods/Properties

            /// Returns the progress value
            public int ProgressValue
            {
                get { return prgValue; }
            }
            #endregion

        }
        #endregion

        //public partial class Form1 : Form
        //{
        //    //instance fields
        //    private export2Excel export2XLS;
        //    private DataSet _dataSet;

        //    public Form1()
        //    {
        //        InitializeComponent();
        //    }

        //    //load the gridview using the local access database
        //    private void Form1_Load(object sender, EventArgs e)
        //    {

        //        Directory.SetCurrentDirectory(Microsoft.Office.Interop.Excel.Application.StartupPath +
        //                                      @"..\..\..\");
        //        String accessPath = Directory.GetCurrentDirectory() +
        //                            @"\Northwind.mdb";

        //        // Set the connection and sql strings
        //        // assumes your mdb file is in your root
        //        string connString =
        //          @"Provider=Microsoft.JET.OLEDB.4.0;data source=" + accessPath;
        //        string sqlString = "SELECT * FROM customers";

        //        OleDbDataAdapter dataAdapter = null;
        //        _dataSet = null;

        //        try
        //        {
        //            // Connection object
        //            OleDbConnection connection = new OleDbConnection(connString);

        //            // Create data adapter object
        //            dataAdapter = new OleDbDataAdapter(sqlString, connection);

        //            // Create a dataset object and fill with data
        //            // using data adapter's Fill method
        //            _dataSet = new DataSet();
        //            dataAdapter.Fill(_dataSet, "customers");
        //            connection.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Problem with DB access-\n\n   connection: "
        //                + connString + "\r\n\r\n            query: " + sqlString
        //                + "\r\n\r\n\r\n" + ex.ToString());
        //            this.Close();
        //            return;
        //        }

        //        DataView dvCust = _dataSet.Tables["customers"].DefaultView;
        //        dg1.DataSource = dvCust;

        //    }

        //    //Do a straight export to a new Excel document 
        //    //of the gridviews grid data
        //    private void btn2Excel_Click(object sender, EventArgs e)
        //    {

        //        //show a file save dialog and ensure the user selects
        //        //correct file to allow the export
        //        saveFileDialog1.Filter = "Excel (*.xls)|*.xls";
        //        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        //        {
        //            if (!saveFileDialog1.FileName.Equals(String.Empty))
        //            {
        //                FileInfo f = new FileInfo(saveFileDialog1.FileName);
        //                if (f.Extension.Equals(".xls"))
        //                {
        //                    StartExport(saveFileDialog1.FileName);
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Invalid file type");
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show("You did pick a location " +
        //                                "to save file to");
        //            }
        //        }
        //    }

        //    //starts the export to new excel document
        //    //@param filepath : the file to export to
        //    private void StartExport(String filepath)
        //    {
        //        btn2Excel.Enabled = false;
        //        btnUseTemplate.Enabled = false;
        //        //create a new background worker, to do the exporting
        //        BackgroundWorker bg = new BackgroundWorker();
        //        bg.DoWork += new DoWorkEventHandler(bg_DoWork);
        //        bg.RunWorkerCompleted +=
        //           new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
        //        bg.RunWorkerAsync(filepath);

        //        //create a new export2XLS object, providing 
        //        //DataView as a input parameter
        //        export2XLS = new export2Excel();
        //        export2XLS.prg +=
        //            new export2Excel.ProgressHandler(export2XLS_prg);
        //    }

        //    //do the new excel document work using the background worker
        //    private void bg_DoWork(object sender, DoWorkEventArgs e)
        //    {
        //        //get the Gridviews DataView
        //        DataView dv = _dataSet.Tables["customers"].DefaultView;
        //        //Pass the path and the sheet to use
        //        export2XLS.ExportToExcel(dv, (String)e.Argument, "newSheet1");
        //    }

        //    //Do a export to a new Excel document, 
        //    //with the use of a custom template
        //    private void btnUseTemplate_Click(object sender, EventArgs e)
        //    {
        //        //show a file save dialog and ensure the user selects
        //        //correct file to allow the export
        //        saveFileDialog1.Filter = "Excel (*.xls)|*.xls";
        //        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        //        {
        //            if (!saveFileDialog1.FileName.Equals(String.Empty))
        //            {
        //                FileInfo f = new FileInfo(saveFileDialog1.FileName);
        //                if (f.Extension.Equals(".xls"))
        //                {
        //                    StartExportUseTemplate(saveFileDialog1.FileName);
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Invalid file type");
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show("You did pick a location" +
        //                                " to save file to");
        //            }
        //        }
        //    }

        //    //Create a new excel document from a given template
        //    //@param filepath : the file to export to
        //    private void StartExportUseTemplate(String filepath)
        //    {
        //        btn2Excel.Enabled = false;
        //        btnUseTemplate.Enabled = false;
        //        //create a new background worker, to do the exporting
        //        BackgroundWorker bg = new BackgroundWorker();
        //        bg.DoWork += new DoWorkEventHandler(bg_DoWorkUseTemplate);
        //        bg.RunWorkerCompleted +=
        //           new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
        //        bg.RunWorkerAsync(filepath);

        //        //create a new export2XLS object, providing 
        //        //DataView as a input parameter
        //        export2XLS = new export2Excel();
        //        export2XLS.prg +=
        //         new export2Excel.ProgressHandler(export2XLS_prg);
        //    }

        //    //Do the adding to custom template to create new excel document, 
        //    //work using the background worker
        //    private void bg_DoWorkUseTemplate(object sender, DoWorkEventArgs e)
        //    {
        //        //create some data to whack in the template
        //        String[,] templateValues =  {    { "task1", 
        //              "CompletedBy1", "CompletedDate1" }, 
        //              { "task2", "CompletedBy2", "CompletedDate2" } 
        //                                };

        //        //The template path to use
        //        Directory.SetCurrentDirectory(Application.StartupPath +
        //                                      @"..\..\..\");
        //        String templatePath = Directory.GetCurrentDirectory() +
        //                              @"\TaskList.xlt";

        //        //Pass the template path and the test values and fill 
        //        //the template
        //        export2XLS.UseTemplate((String)e.Argument, templatePath,
        //                                templateValues);
        //    }

        //    //Update the progress bar with the a value
        //    private void export2XLS_prg(object sender, ProgressEventArgs e)
        //    {
        //        //need to call InvokeRequired to check 
        //        //if thread need marshalling, to get the thread onto 
        //        //the same thread as the thread who owns the controls
        //        if (this.InvokeRequired)
        //        {
        //            this.Invoke(new EventHandler(delegate
        //            {
        //                progressBar1.Value = e.ProgressValue;
        //            }));
        //        }
        //        else
        //        {
        //            progressBar1.Value = e.ProgressValue;
        //        }
        //    }

        //    //show a message to the user when the background worker has finished
        //    //and re-enable the export buttons
        //    private void bg_RunWorkerCompleted(object sender,
        //                    RunWorkerCompletedEventArgs e)
        //    {
        //        btn2Excel.Enabled = true;
        //        btnUseTemplate.Enabled = true;
        //        MessageBox.Show("Finished");
        //    }
        //}

    }
}
