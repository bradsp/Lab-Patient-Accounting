using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Library;
using LabBilling.Logging;
using MCL;
using Microsoft.Data.SqlClient;
using System.Collections; // for arraylist
using System.ComponentModel;
using System.Data;
using System.Drawing.Printing; // for print document
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Transactions;
using Utilities;

namespace LabBilling.Legacy;

public partial class Posting835 : Form
{
    private DateTime _dtFilesImported;     // date the files were imported so we don't do it twice.
    private DirectoryInfo _diCurrent = null;         //new DirectoryInfo(string.Format(@"{0}",dr[0]["value"].ToString()));
    private DirectoryInfo _diInvalid = null;         //new DirectoryInfo(string.Format(@"{0}\invalid", dr[0]["value"].ToString()));
    private DirectoryInfo _diSaved = null;           // new DirectoryInfo(string.Format(@"{0}\saved", dr[0]["value"].ToString()));
    private DirectoryInfo _diFrom = null;            //new DirectoryInfo(string.Format(@"{0}", dr[0]["value"].ToString()));
    private DirectoryInfo[] _diDirectories = null;   //diFrom.GetDirectories(DateTime.Now.Year.ToString());
    private FileInfo[] _fiCurrent = null;
    private FileInfo[] _fiSaved = null;
    private FileInfo[] _fiInvalid = null;
    private FileInfo[] _fiFrom = null;
    private string _strFileType = null;
    private readonly Dictionary<string, decimal> _dicSTAmts = new();
    private DataTable _dtNotes = new();
    private string PropAppName
    { get { return string.Format("{0} {1}", Application.ProductName, Application.ProductVersion); } }

    private readonly Dictionary<string, string> m_dicPayer = new();
    // rgc/wdk 20120425 added for sql spid reduction
    private string _strFinCode = null;
    private string _strInsCode = null;
    private readonly CAcc _rAcc = null;
    private readonly CEob _rEob = null;
    private readonly CAcc _cAccStatus = null;
    private readonly CAcc _cAcc = null;

    private readonly AccountService _accountService = new(Program.AppEnvironment);
    private readonly DictionaryService _dictionaryService = new(Program.AppEnvironment);
    private readonly BatchTransactionService _batchTransactionService = new(Program.AppEnvironment);

    public event EventHandler<string> AccountLaunched;

    // end of 20120425
    static DataGridViewCell _celHidden;
    static DataGridViewCell _celMoney;
    static Dictionary<string, DataGridViewCell> _dicColGridsEOB;
    static Dictionary<string, DataGridViewCell> _dicColGrids;
    /// <summary>
    /// The ISA's last field is the subfield designated seperator for this file. 
    /// MEDICARE uses '>' and TLC uses '^' when checking the SVC*HC? this is important
    /// </summary>
    string _strComponentSeperator = "";

    /// <summary>
    /// Path to the Account Log file used for StreamReader and StreamWriter
    /// </summary>
    private string _strAccountLogPath;
    /// <summary>
    /// Path to the log file used for StreamReader and StreamWriter
    /// </summary>
//        private string m_strErrorLogPath;
    /// <summary>
    /// Steam for writing Account log
    /// </summary>
    private StreamWriter _swAccount;
    /// <summary>
    /// Stream for reading Account log
    /// </summary>
    private StreamReader _srAccount;
    /// <summary>
    /// Stream for writing the Error log
    /// </summary>
    //private StreamWriter m_swErrorLog;
    /// <summary>
    /// Stream for reading the Error log
    /// </summary>
    //private StreamReader m_srErrorLog;

    // static private List<Thread> m_ThreadList = null;// a list of thread objects (strongly typed for performance) created for the print preview dialogs.
    private readonly string _strServer = "";
    private readonly string _strDatabase = "";
    private ERR _eRR;

    /// <summary>
    /// Used for displaying the datagridview for EOBs 100 records at a time.
    /// </summary>
    private int _nCurrentDisplayed = 0;
    private decimal[] _d_dgvEOB_Totals;
    private string _strFileName;

    /// <summary>
    /// 06/05/2008 wdk
    /// This is used to post both the eob and checks. It is much faster than using the DataGridView
    /// which slows enormiouly after 100 records are loaded.
    /// </summary>
    private DataSet _dsRecords = new DataSet();

    /// <summary>
    /// Dictionarys containing Grids bands
    /// </summary>
    private Dictionary<string, DataGridViewBand> _dicBandProcessed;
    private Dictionary<string, DataGridViewBand> _dicBandNotProcessed;
    private Dictionary<string, DataGridViewBand> _dicBandDenieds;
    private Dictionary<string, DataGridViewBand> _dicBandEOB;

    /// <summary>
    /// CAS*PI*94 
    /// PI - PAYOR INITIATED REDUCTION  
    /// 94 - Processed in Excess of charges.
    /// rgc/wdk 20120517 added
    /// </summary>
    decimal _dPayorInitiatedReductions = 0.00m;

    /// <summary>
    /// CAS CO 50 with cpt4 modifier of GZ will be added to other adj amt???
    /// </summary>
    decimal _dChargesWriteOffGZ = 0.00m;
    /// <summary>
    /// CLP 04 without the CLP* (not positive needs verification) 
    /// </summary>
    decimal _dEOBChargesDenied = 0.00m;
    /// <summary>
    /// CAS C0/96's totals for the EOB header grid
    /// </summary>
    decimal _dEOBChargesNCovd = 0.00m;

    // EOB's Patient Liability
    /// <summary>
    /// CAS PR/45 totals Patient Libility Non Covered
    /// </summary>
    decimal _dEOBPatLibNCovdCharges = 0.00m;
    /// <summary>
    /// CAS PR/2 Patient Libility CoInsurance Amounts check file date 02/07/2008 to verify
    /// </summary>
    decimal _dEOBPatLibCoInsurance = 0.00m;

    // EOB's Payment Data
    /// <summary>
    /// Calculated from the processed tab's paid amount field ???
    /// </summary>
    decimal _dEOBPayDataHCPCSAmt = 0.00m;
    /// <summary>
    ///  Total of the co/45 from the CAS. This amount is posted in the contractual of the check record.
    /// </summary>
    decimal _dEOBPayDataContAdjAmt = 0.00m;
    /// <summary>
    /// Have not seen yet should be a PC in the CAS 04/16/2008
    /// </summary>
    decimal _dEOBPayDataPatRefund = 0.00m;
    /// <summary>
    /// Calculated from the OA/23 s of the details
    /// </summary>
    decimal _dEOBPayDataMSPPrimPay = 0.00m;

    // ReportGenerator m_rgReport = null;
    private PrintDocument _myPrintDocument = null;
    private readonly Dictionary<string, string> _dicProviders;
    private Dictionary<string, decimal> _dicBalances;

    /// <summary>
    /// This string arrary is for the insertion of records into datagrids dgvProcessed, dgvDenieds, dgvNotProcessed
    /// uses the enum col835Grids for initilization.
    /// </summary>
    public string[] m_strarrRecordsInsert;
    //public string[] m_strarrRecordsWriteOff;
    //DataGridViewRow[] m_dgvRecordsWriteOff;
    private DataTable _dtRecordWriteOff;

    /// <summary>
    /// This string arrary is for the insertion of records into datagrids dgvProcessed, dgvDenieds, dgvNotProcessed
    /// uses the enum col835Grids for initilization this is for the second contractual on the line if present.
    /// </summary>
    public string[] m_strarrRecordsInsertAddContractual;

    /// <summary>
    /// This string arrary is for the insertion of records into the datagrid dgvEOB
    /// uses the enum col835EOB for initilization.
    /// </summary>
    public string[] m_strarrEOBInsert;

    /// <summary>
    /// Account totals for Allowed, charges, paid, and adjustment Amt.
    /// </summary>
    public string[] m_strarrAccountTotals;


    /// <summary>
    /// This enum is for the SVC detail record. All 835 grids except 
    /// dgvEOB use this for header columns, as do the datasets for each table
    /// </summary>
    public enum Col835Grids
    {
        Account = 0, //Account does not have an e (enum) identifier as we allow RFClassLibrary to launchAcc if the column is called "Account"
        eSubscriberName,
        eSubscriberID,
        eCheckNo,
        eClaimStatus, /// <summary> Used for the determine which grid this inserted record should go on. Processed (1,2,19) , denied (4), not processed is anything else</summary>
        eCPT4Code,
        eRevCode,
        eUnits,
        eAPC, // see file for 04/08/2008 from medicare
        eStat, // see file for 04/08/2008 from medicare 
        eWeight, // not used as of 04/09/2008 
        eDateOfService,
        eAllowed,
        eCharges,
        ePaid,
        eContractualAdjAmt,
        eOtherAdjAmt, //05/27/2008 wdk added to handle PR/2 amounts 
        eWriteOffAmt,
        eWriteOffDate,
        eWriteOffCode,
        eReason,
        eClaimID
        //eClaimAdjCode
        // if any items are added after this enum change the 
        //            CreateDataGridViewProcessed(),
        //CreateDataGridViewNotProcessed(), and CreateDataGridViewDenied() function                        
    }

    /// <summary>
    /// This enum is for the EOB's header parts. Only used for dgvEOB 
    /// </summary>
    public enum Col835EOB
    {
        // Subscriber Data
        Account = 0,
        eSubscriberName,
        eDOS,
        eHIC, // subscriber id
        eICN,
        ePatStat, // not used as of 04/09/2008 
        //ePCN, //  same as Account so removed as duplicate
        eClaimSt,
        eType, // type is clp 8 and 9 together ie 141 (Medicare only)
        // Charges
        eChargesReported,
        eChargesNCovd,
        eChargesDenied,
        // Patient Liability
        ePatientLibCoinsurance, // PR/2's
        ePatientLibNCovdCharges, // PR/45's
        // Payment Data
        ePaymentDataReimbRate,
        ePaymentDataMSPPrimPay,
        ePaymentDataHCPCSAmt,
        ePaymentDataContAdjAmt, // Total of the CO/45's
        ePaymentDataPatRefund,
        ePaymentDataPerDiemRate,
        ePaymentDataNetReimbAmt, // total of the paid's in the CLP
        eOtherAdjAmt, //05/27/2008 wdk added to handl PR/2 amounts 
        // claim forwarded (Medicare only)
        eClaimForwarded,
        eClaimForwardedID // if any items are added after this enum change the CreateDataGridViewEOB() funtion

    }
    private void TsmiFileOpen_Click(object sender, EventArgs e)
    {
        openFileDialog.Filter = "835 Files (*.835)|*.835|XML Files (*.X12)|*.X12|ALL Files (*.*)|*.*";
        openFileDialog.FilterIndex = 1;
        openFileDialog.Tag = (string)"MEDICARE";
        openFileDialog.InitialDirectory = Program.AppEnvironment.ApplicationParameters.RemitProcessingDirectory;
        if(openFileDialog.ShowDialog() == DialogResult.OK)
        {
            ClearForm();

            _strFileName = openFileDialog.FileName;
            _d_dgvEOB_Totals = new decimal[Enum.GetNames(typeof(Col835EOB)).Length]; // 06/05/2008 wdk dgvEOB.Columns.Count];
            _d_dgvEOB_Totals.Initialize();
            _strFileType = "MEDICARE";
            ProcessFile(_strFileName, _strFileType);
            Application.DoEvents();
        }


    }

    /// <summary>
    /// Forms Load this is where we create the gridcontrol headers, dictionary with all the providers we are 
    /// currently processing electronic files for, and our printdocument (At this point you can assign the 
    /// printdocument event handler [Print]).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Posting835_Load(object sender, EventArgs e)
    {

        //1. copy the files from directory listed in the system table under programs = 'Posting835Remittance'
        //      key_name = 'import_directory
        //2. into the file location listed in the system table under programs = 'Posting835Remittance'
        //      key_name = 'import_directory_into
        //if (MessageBox.Show("Do you want to move the current files?", "INITIAL PROCESSING",MessageBoxButtons.YesNo) 
        //        == DialogResult.Yes)
        //{
        // added a date to the system table to manage the import so it isn't done twice by the clerks.

        // this is no longer needed now that we are getting remits from nThrive
        //MoveFilesToProcess(); 

        //}

        GetSystemParameters();

        CreateSpecialDataGridViewCells();
        CreateDictionaryColGrids();
        CreateEOBDictionaryColumnGrid();
        CreateGridControlsHeaders();
        CreatePayorDictionary();
        _myPrintDocument = new PrintDocument(); // create the blank document to print on.
        tc835.SelectedIndex = 0;
        InitializeStringArrays();
        tbDatabase.Text = string.Format("{0} / {1}", _strServer, _strDatabase); // display on screen the server and database

    }

    private void GetSystemParameters()
    {
        // our directories
        _diCurrent = new DirectoryInfo($"{Program.AppEnvironment.ApplicationParameters.RemitImportDirectory}");
        _diInvalid = new DirectoryInfo($"{Program.AppEnvironment.ApplicationParameters.RemitImportDirectory}\\invalid");
        _diSaved = new DirectoryInfo($"{Program.AppEnvironment.ApplicationParameters.RemitImportDirectory}\\saved");
    }

    private void CreatePayorDictionary()
    {

        m_dicPayer.Add("046000", "");
        m_dicPayer.Add("06J4812", "UHC");

        m_dicPayer.Add("12350", "ALCATEL-LUCENT FRR UNION");
        m_dicPayer.Add("126927", "");
        m_dicPayer.Add("12700", "BOILERMAKERS NATIONAL H&W FUND");

        m_dicPayer.Add("199947", "OPT/PPO");

        m_dicPayer.Add("304000", "");
        m_dicPayer.Add("3P1104", "");

        m_dicPayer.Add("4G3135", "");
        m_dicPayer.Add("4P0327", "");

        m_dicPayer.Add("525617", "");
        m_dicPayer.Add("", "");

        m_dicPayer.Add("6J4812", "UHC");

        m_dicPayer.Add("701669", "");
        m_dicPayer.Add("704389", "");
        m_dicPayer.Add("703981", "");
        m_dicPayer.Add("704534", "");
        m_dicPayer.Add("704630", "");
        m_dicPayer.Add("704966", "");
        m_dicPayer.Add("706717", "");
        m_dicPayer.Add("708501", "");
        m_dicPayer.Add("708547", "");
        m_dicPayer.Add("709779", "");
        m_dicPayer.Add("710755", "");
        m_dicPayer.Add("714946", "");
        m_dicPayer.Add("730781", "");
        m_dicPayer.Add("742681", "");
        m_dicPayer.Add("742904", "");
        m_dicPayer.Add("743213", "");

        m_dicPayer.Add("80003", "AARP MEDICARECOMPLETE PLUS");
        m_dicPayer.Add("8K5167", "");

    }

    private void DGV_RowHeaderClicked(object sender, DataGridViewCellMouseEventArgs e)
    {
        DataGridView dgv = sender as DataGridView;

        var (account, error) = FormExtensions.GetDGVAccount(dgv, e.RowIndex);
        if (!string.IsNullOrEmpty(account))
            AccountLaunched?.Invoke(this, account);
        else
            MessageBox.Show(error, "Account not Launched");
    }

    public Posting835(string[] args)
    {
        InitializeComponent();
        if (args.Length != 2)
        {
            MessageBox.Show("Incorrect number of arguments. Needs server and database");
            Environment.Exit(13);
        }

        if (args[0].StartsWith("/"))
        {
            _strServer = args[0].Remove(0, 1); // 08/08/2008 wdk changed to accomidate the ERR class
                                                // m_strServer = args[0].Remove(0, 1);
        }
        else
        {
            _strServer = args[0];
        }

        if (args[1].StartsWith("/"))
        {
            _strDatabase = args[1].Remove(0, 1); // 08/08/2008 wdk changed to accomidate the ERR class
        }
        else
        {
            _strDatabase = args[1];
        }


        dgvEOB.RowHeaderMouseDoubleClick += DGV_RowHeaderClicked;
        dgvProcessed.RowHeaderMouseDoubleClick += DGV_RowHeaderClicked;
        dgvNotProcessed.RowHeaderMouseDoubleClick += DGV_RowHeaderClicked;
        dgvDenieds.RowHeaderMouseDoubleClick += DGV_RowHeaderClicked;

        _eRR = new ERR(new string[] { Program.AppEnvironment.ApplicationParameters.DatabaseEnvironment != "Production" ? "/TEST" : "/LIVE", _strServer, _strDatabase }); // ERR class needs /LIVE or /TEST to be the first argument in the command line.
        // rgc/wdk 20120425 moved to remove the spid overload in sql.
        _cAcc = new CAcc(_strServer, _strDatabase, ref _eRR);
        _cAccStatus = new CAcc(_strServer, _strDatabase, ref _eRR);
        _rAcc = new CAcc(_strServer, _strDatabase, ref _eRR);
        _rEob = new CEob(_strServer, _strDatabase, ref _eRR);

    }

    /// <summary>
    /// this is on the toolstrip above the menu strip. It only imports files from the WTH server.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void tsmi835_Click(object sender, EventArgs e)
    {
        openFileDialog.Filter = "All Files (*.*)|*.*";
        openFileDialog.InitialDirectory = string.Format(@"{0}", _diCurrent);

        openFileDialog.Tag = (string)"MEDICARE";
        openFileDialog.ShowDialog();
        _strFileType = "MEDICARE";

    }

    /// <summary>
    /// Create all the grid control headers here. Make sure to all all columns and set the ones you are 
    /// not using to visible = false. This way the DataGridViewRow insert will be in sync.
    /// </summary>
    private void CreateGridControlsHeaders()
    {
        CreateDataGridViewProcessed();
        CreateDataGridViewNotProcessed();
        CreateDataGridViewDenieds();
        CreateDataGridViewEOB();
        CreateDataTableNotes();
    }

    private void CreateDataTableNotes()
    {
        _dtNotes = new DataTable();
        _dtNotes.Columns.Add("Account");
        _dtNotes.Columns.Add("cpt4");
        _dtNotes.Columns.Add("status");
        _dtNotes.Columns.Add("reason");
    }

    private void CreateDataGridViewDenieds()
    {
        _dsRecords.Tables.Add("dgvDenieds");
        _dicBandDenieds = new Dictionary<string, DataGridViewBand>();
        foreach (string strCol in Enum.GetNames(typeof(Col835Grids)))
        {
            Application.DoEvents();
            try
            {
                _dsRecords.Tables["dgvDenieds"].Columns.Add(strCol);
                _dsRecords.Tables["dgvDenieds"].Columns[strCol].ExtendedProperties.Add("VISIBLE", true);
                _dsRecords.Tables["dgvDenieds"].Columns[strCol].ExtendedProperties.Add("ALIGNMENT", "Left");
                _dsRecords.Tables["dgvDenieds"].Columns[strCol].ExtendedProperties.Add("CURRENCY", false);

                dgvDenieds.Columns.Add(strCol, strCol);
                DataGridViewCell cellType;
                _dicColGrids.TryGetValue(strCol, out cellType);
                if (cellType != null)
                {
                    dgvDenieds.Columns[strCol].CellTemplate = cellType;
                    if (cellType == _celHidden)
                    {
                        _dicBandDenieds.Add(strCol, dgvDenieds.Columns[strCol]);
                        dgvDenieds.Columns[strCol].Visible = false;
                        _dsRecords.Tables["dgvDenieds"].Columns[strCol].ExtendedProperties["VISIBLE"] = false;
                    }
                    if (cellType == _celMoney)
                    {
                        _dsRecords.Tables["dgvDenieds"].Columns[strCol].ExtendedProperties["ALIGNMENT"] = "Right";
                        _dsRecords.Tables["dgvDenieds"].Columns[strCol].ExtendedProperties["CURRENCY"] = true;
                    }
                }
            }
            catch
            {
                // do nothing this is to prevent crash when checking to see if the cellTye.Equals(null)
                // which it will be for non hidden, non money fields like RecCode, Cpt4 w/Modifier, dates, etc.
            }
        }

        dgvDenieds.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        dgvDenieds.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
        // Handle all the cell formatting for currency 
        dgvDenieds.CellFormatting += new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);
    }

    /// <summary>
    /// Maintains a dictionary that can contains special fields and their names (key). 
    /// Used mainly for hidding non relevant data from the user. Hidden fields can be 
    /// displayed when the grid is selected and the F12 Key is pressed.
    /// </summary>
    private static void CreateDictionaryColGrids()
    {
        _dicColGrids = new Dictionary<string, DataGridViewCell>
        {
            // MONEY FIELDS
            { Col835Grids.ePaid.ToString(), _celMoney },
            { Col835Grids.eContractualAdjAmt.ToString(), _celMoney },
            { Col835Grids.eCharges.ToString(), _celMoney },
            { Col835Grids.eAllowed.ToString(), _celMoney },
            { Col835Grids.eOtherAdjAmt.ToString(), _celMoney },
            { Col835Grids.eWriteOffAmt.ToString(), _celMoney }, // wdk 20130731 added

            // HIDDEN FIELDS (initially not visible) can toggle them to be visible with F12 
            { Col835Grids.eCPT4Code.ToString(), _celHidden },
            { Col835Grids.eRevCode.ToString(), _celHidden },
            { Col835Grids.eUnits.ToString(), _celHidden },
            { Col835Grids.eStat.ToString(), _celHidden },
            { Col835Grids.eWeight.ToString(), _celHidden },
            { Col835Grids.eAPC.ToString(), _celHidden }
        };

    }

    /// <summary>
    /// To configure the Grids:
    ///     A. Create two special cells.
    ///         1. Money cell with a gold background and the format style of currency.
    ///         2. Hidden cell with a light coral background that can be displayed by selecting a
    ///             grid and pressing the F12 key to unhide/hide.
    ///             Additional cells can be hidden/unhidden by right clicking on the column header and toggling the property.
    /// </summary>
    private static void CreateSpecialDataGridViewCells()
    {
        _celHidden = new DataGridViewTextBoxCell();
        _celHidden.Style.BackColor = Color.LightCoral;

        _celMoney = new DataGridViewTextBoxCell();
        _celMoney.Style.Format = "C";
        _celMoney.Style.BackColor = Color.Gold;
        _celMoney.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    private void CreateDataGridViewNotProcessed()
    {
        _dsRecords.Tables.Add("dgvNotProcessed");
        _dicBandNotProcessed = new Dictionary<string, DataGridViewBand>();
        foreach (string strCol in Enum.GetNames(typeof(Col835Grids)))
        {
            Application.DoEvents();
            try
            {
                _dsRecords.Tables["dgvNotProcessed"].Columns.Add(strCol);
                _dsRecords.Tables["dgvNotProcessed"].Columns[strCol].ExtendedProperties.Add("VISIBLE", true);
                _dsRecords.Tables["dgvNotProcessed"].Columns[strCol].ExtendedProperties.Add("ALIGNMENT", "Left");
                _dsRecords.Tables["dgvNotProcessed"].Columns[strCol].ExtendedProperties.Add("CURRENCY", false);

                dgvNotProcessed.Columns.Add(strCol, strCol);
                _dicColGrids.TryGetValue(strCol, out DataGridViewCell cellType);
                if (cellType != null)
                {
                    dgvNotProcessed.Columns[strCol].CellTemplate = cellType;
                    if (cellType == _celHidden)
                    {
                        _dicBandNotProcessed.Add(strCol, dgvNotProcessed.Columns[strCol]);
                        dgvNotProcessed.Columns[strCol].Visible = false;
                        _dsRecords.Tables["dgvNotProcessed"].Columns[strCol].ExtendedProperties["VISIBLE"] = false;
                    }
                    if (cellType == _celMoney)
                    {
                        _dsRecords.Tables["dgvNotProcessed"].Columns[strCol].ExtendedProperties["ALIGNMENT"] = "Right";
                        _dsRecords.Tables["dgvNotProcessed"].Columns[strCol].ExtendedProperties["CURRENCY"] = true;
                    }
                }
            }
            catch
            {
                // do nothing this is to prevent crash when checking to see if the cellTye.Equals(null)
                // which it will be for non hidden, non money fields like RecCode, Cpt4 w/Modifier, dates, etc.
            }
        }

        dgvNotProcessed.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        dgvNotProcessed.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
        // Handle all the cell formatting for currency 
        dgvNotProcessed.CellFormatting += new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);

    }

    private void CreateDataGridViewEOB()
    {
        _dsRecords.Tables.Add("dgvEOB");
        _dicBandEOB = new Dictionary<string, DataGridViewBand>();

        foreach (string strCol in Enum.GetNames(typeof(Col835EOB)))
        {
            Application.DoEvents();
            try
            {
                _dsRecords.Tables["dgvEOB"].Columns.Add(strCol);
                _dsRecords.Tables["dgvEOB"].Columns[strCol].ExtendedProperties.Add("VISIBLE", true);
                _dsRecords.Tables["dgvEOB"].Columns[strCol].ExtendedProperties.Add("ALIGNMENT", "Left");
                _dsRecords.Tables["dgvEOB"].Columns[strCol].ExtendedProperties.Add("CURRENCY", false);

                dgvEOB.Columns.Add(strCol, strCol);
                _dicColGridsEOB.TryGetValue(strCol, out DataGridViewCell cellType);
                if (cellType != null)
                {
                    dgvEOB.Columns[strCol].CellTemplate = cellType;
                    if (cellType == _celHidden)
                    {
                        _dicBandEOB.Add(strCol, dgvEOB.Columns[strCol]);
                        dgvEOB.Columns[strCol].Visible = false;
                        _dsRecords.Tables["dgvEOB"].Columns[strCol].ExtendedProperties["VISIBLE"] = false;
                    }
                    if (cellType == _celMoney)
                    {
                        _dsRecords.Tables["dgvEOB"].Columns[strCol].ExtendedProperties["ALIGNMENT"] = "Right";
                        _dsRecords.Tables["dgvEOB"].Columns[strCol].ExtendedProperties["CURRENCY"] = true;
                    }

                }
            }
            catch
            {
                // do nothing this is to prevent crash when checking to see if the cellTye.Equals(null)
                // which it will be for non hidden, non money fields like RecCode, Cpt4 w/Modifier, dates, etc.
            }
        }
        dgvEOB.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        dgvEOB.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
        // Handle all the cell formatting for currency 
        dgvEOB.CellFormatting += new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);

    }

    /// <summary>
    /// Sets the cell properties for printing via the extended properties in the create.
    /// Could use those properties here to set the entended properties to print or not print
    /// based on the cells m_celHidden value.
    /// </summary>
    private static void CreateEOBDictionaryColumnGrid()
    {
        _dicColGridsEOB = new Dictionary<string, DataGridViewCell>
        {
            // money fields
            { Col835EOB.ePaymentDataMSPPrimPay.ToString(), _celMoney },
            { Col835EOB.ePaymentDataHCPCSAmt.ToString(), _celMoney },
            { Col835EOB.ePaymentDataContAdjAmt.ToString(), _celMoney },
            { Col835EOB.ePaymentDataPatRefund.ToString(), _celMoney },
            { Col835EOB.eChargesReported.ToString(), _celMoney },
            { Col835EOB.eChargesNCovd.ToString(), _celMoney },
            { Col835EOB.eChargesDenied.ToString(), _celMoney },
            { Col835EOB.ePatientLibCoinsurance.ToString(), _celMoney },
            { Col835EOB.ePatientLibNCovdCharges.ToString(), _celMoney },
            { Col835EOB.ePaymentDataNetReimbAmt.ToString(), _celMoney },
            { Col835EOB.eOtherAdjAmt.ToString(), _celMoney },

            // initially hidden fields can toggle them to be visible with f12 
            { Col835EOB.ePaymentDataPerDiemRate.ToString(), _celHidden },
            { Col835EOB.ePaymentDataReimbRate.ToString(), _celHidden },
            { Col835EOB.eICN.ToString(), _celHidden },
            { Col835EOB.ePatStat.ToString(), _celHidden },
            { Col835EOB.eClaimSt.ToString(), _celHidden },
            { Col835EOB.eType.ToString(), _celHidden },
            { Col835EOB.eClaimForwarded.ToString(), _celHidden },
            { Col835EOB.eClaimForwardedID.ToString(), _celHidden }
        };
    }

    /// <summary>
    /// Creates the Datagridview for the Processed tab's records. Then sets their properties
    /// </summary>
    private void CreateDataGridViewProcessed()
    {
        _dsRecords.Tables.Add("dgvProcessed");

        // Set the band for the columns to the type in the cell
        _dicBandProcessed = new Dictionary<string, DataGridViewBand>();
        foreach (string strCol in Enum.GetNames(typeof(Col835Grids)))
        {
            Application.DoEvents();
            _dsRecords.Tables["dgvProcessed"].Columns.Add(strCol);
            _dsRecords.Tables["dgvProcessed"].Columns[strCol].ExtendedProperties.Add("VISIBLE", true);
            _dsRecords.Tables["dgvProcessed"].Columns[strCol].ExtendedProperties.Add("ALIGNMENT", "Left");
            _dsRecords.Tables["dgvProcessed"].Columns[strCol].ExtendedProperties.Add("CURRENCY", false);


            dgvProcessed.Columns.Add(strCol, strCol);
            DataGridViewCell cellType;
            _dicColGrids.TryGetValue(strCol, out cellType);
            try
            {
                if (cellType != null)
                {
                    dgvProcessed.Columns[strCol].CellTemplate = cellType;
                    if (cellType == _celHidden)
                    {
                        _dicBandProcessed.Add(strCol, dgvProcessed.Columns[strCol]);
                        dgvProcessed.Columns[strCol].Visible = false;
                        _dsRecords.Tables["dgvProcessed"].Columns[strCol].ExtendedProperties["VISIBLE"] = false;
                    }
                    if (cellType == _celMoney)
                    {
                        _dsRecords.Tables["dgvProcessed"].Columns[strCol].ExtendedProperties["ALIGNMENT"] = "Right";
                        _dsRecords.Tables["dgvProcessed"].Columns[strCol].ExtendedProperties["CURRENCY"] = true;
                    }
                }
            }
            catch
            {
                // do nothing this is to prevent crash when checking to see if the cellTye.Equals(null)
                // which it will be for non hidden, non money fields like RecCode, Cpt4 w/Modifier, dates, etc.
            }
        }
        dgvProcessed.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        dgvProcessed.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
        dgvProcessed.Columns[(int)Col835Grids.eWriteOffAmt].DefaultCellStyle.DataSourceNullValue = DBNull.Value;
        // Handle all the cell formatting for currency 
        dgvProcessed.CellFormatting += new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);

    }

    /// <summary>
    /// Reset the string arrays to empty them.
    /// </summary>
    private void InitializeStringArrays()
    {
        // create the string arrays for inserting rows
        m_strarrEOBInsert = new string[Enum.GetNames(typeof(Col835EOB)).Length]; // 06/05/2008 wdk
        m_strarrRecordsInsert = new string[Enum.GetNames(typeof(Col835Grids)).Length];// 06/05/2008 wdk dgvProcessed.Columns.Count];         
        // WDK 20130725 added 
        m_strarrRecordsInsertAddContractual = new string[Enum.GetNames(typeof(Col835Grids)).Length];// wdk 20130725         
        // wdk 20130816 added
        //m_strarrRecordsWriteOff = new string[Enum.GetNames(typeof(col835Grids)).Length];// wdk 20130725         
        //m_dgvRecordsWriteOff = new DataGridViewRow[99];
        _dtRecordWriteOff = new DataTable("WRITE OFF");

        foreach (string str in Enum.GetNames(typeof(Col835Grids)))
        {
            Application.DoEvents();
            _dtRecordWriteOff.Columns.Add(str, System.Type.GetType("System.String"));
        }
    }

    /// <summary>
    /// Uses 
    /// 1. Format the contents of the columns that are currency values.
    /// 2. Tooltips for the individual cells. 
    /// 3. Row header numbers creation    
    /// 
    /// Note: Contains documentation for datetime cell formatting, and tool tips
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        ((DataGridView)sender).Rows[e.RowIndex].HeaderCell.Value = $"{(e.RowIndex + 1)}";

        if (e.CellStyle.Format == "C")
        {
            if (e.Value != null)
            {
                decimal dRes = 0.00m;
                if (decimal.TryParse(e.Value.ToString(), out dRes))
                {
                    e.Value = dRes.ToString("c"); //
                }
                else
                {
                    e.Value = string.Format("{0:C}", "0.00");
                }
            }
            else
            {
                e.Value = "0.00";
            }
        }

    }

    //private void openFileDialog_FileOk(object sender, CancelEventArgs e)
    //{
    //    if (e.Cancel)
    //    {
    //        return;
    //    }

    //    ClearForm();
    //    if (!_eRR.m_Logfile.WriteLogFile(openFileDialog.FileName))
    //    {
    //        //  MessageBox.Show("DIDN'T WRITE");
    //    }

    //    _strFileName = openFileDialog.FileName;
    //    _d_dgvEOB_Totals = new decimal[Enum.GetNames(typeof(Col835EOB)).Length]; // 06/05/2008 wdk dgvEOB.Columns.Count];
    //    _d_dgvEOB_Totals.Initialize();
    //    _strFileType = openFileDialog.Tag.ToString();
    //    ProcessFile(_strFileName, _strFileType);
    //    Application.DoEvents();

    //}

    /// <summary>
    /// This begins the process of reading the file selected in the open file dialog.
    /// </summary>
    /// <param name="strFileName"></param>
    private void ProcessFile(string strFileName, string strType)
    {
        _strFinCode = "";
        _strInsCode = "";
        #region MEDICARE
        if (strType == "MEDICARE")
        {
            lbChecks.Visible = false;
            tbCheckAmt.Visible = true;
            tbCheckNo.Visible = true;
            if (!ProcessMedicare835File(strFileName)) // bulk of the process is started here
            {
                // if we don't find any MCL RECORDS move the file to SAVED\INVALID directory.
                // wdk 20160526 we are now purging the files before we get here for invalid files so don't ask again
                try
                {
                    File.Move(strFileName, $"{_diInvalid}\\{strFileName.Substring(strFileName.LastIndexOf('\\'))}");
                }
                catch (IOException)
                {
                    string strMoveFileName = strFileName.Replace(".835", string.Format(@"{0}.835", DateTime.Now.ToFileTimeUtc().ToString()));
                    File.Move(strFileName, string.Format(@"{0}\{1}", _diInvalid, strMoveFileName));
                }
                _eRR.m_Logfile.WriteLogFile("Returned from parsing with a false value after parsing completed.");
            }
            else
            {
                // Fills out the totals in the header of the form.
                SetTableLayOutTotals();
                tsslDenieds.Text = string.Format("Denieds: {0}", _dsRecords.Tables[dgvDenieds.Name].Rows.Count);
                tsslNotProcessed.Text = string.Format("Not Processed: {0}", _dsRecords.Tables[dgvNotProcessed.Name].Rows.Count);
                tsslProcessed.Text = string.Format("Processed: {0}", _dsRecords.Tables[dgvProcessed.Name].Rows.Count);
                tsslEOB.Text = string.Format("EOBs: {0}", _dsRecords.Tables[dgvEOB.Name].Rows.Count);
                tsmiFirst20_Click(dgvProcessed, null);
                tsmiFirst20_Click(dgvDenieds, null);
                tsmiFirst20_Click(dgvNotProcessed, null);
                tsmiFirst20_Click(dgvEOB, null);

            }
            return;
        }
        #endregion MEDICARE

        #region BLUECROSS
        if (strType == "BLUECROSS")
        {
            lbChecks.Visible = true;
            tbCheckAmt.Visible = false;
            tbCheckNo.Visible = false;
            if (!ProcessBluecross835File(strFileName)) // bulk of the process is started here
            {
                // if we don't find any MCL RECORDS move the file to SAVED\INVALID directory.
                if (MessageBox.Show("This file does not contain any MCL records.\r\nDo you want to copy this file to the INVALID directory", "MEDICARE FILE", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    try
                    {
                        File.Move(strFileName, $"{_diInvalid}\\{strFileName.Substring(strFileName.LastIndexOf('\\'))}");
                    }
                    catch (IOException)
                    {
                        string strMoveFileName = strFileName.Replace(".835", "_dk.835");
                        File.Move(strFileName, $"{_diInvalid}\\{strMoveFileName.Substring(strMoveFileName.LastIndexOf('\\'))}");
                    }
                }
            }
            else
            {
                // Fills out the totals in the header of the form.
                SetTableLayOutTotals();
                tsslDenieds.Text = string.Format("Denieds: {0}", _dsRecords.Tables[dgvDenieds.Name].Rows.Count);
                tsslNotProcessed.Text = string.Format("Not Processed: {0}", _dsRecords.Tables[dgvNotProcessed.Name].Rows.Count);
                tsslProcessed.Text = string.Format("Processed: {0}", _dsRecords.Tables[dgvProcessed.Name].Rows.Count);
                tsslEOB.Text = string.Format("EOBs: {0}", _dsRecords.Tables[dgvEOB.Name].Rows.Count);
                tsmiFirst20_Click(dgvProcessed, null);
                tsmiFirst20_Click(dgvDenieds, null);
                tsmiFirst20_Click(dgvNotProcessed, null);
                tsmiFirst20_Click(dgvEOB, null);

            }
            return;
        }
        #endregion BLUECORSS

        #region UHC
        if (strType == "UHC")
        {
            if (!ProcessUHC835File(strFileName)) // bulk of the process is started here
            {
                // if we don't find any MCL RECORDS move the file to SAVED\INVALID directory.
                if (MessageBox.Show("This file does not contain any MCL records.\r\nDo you want to copy this file to the INVALID directory", "MEDICARE FILE", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    try
                    {
                        File.Move(strFileName, $"{_diInvalid}\\{strFileName.Substring(strFileName.LastIndexOf('\\'))}");
                    }
                    catch (IOException)
                    {
                        string strMoveFileName = strFileName.Replace(".835", "_dk.835");
                        File.Move(strFileName, $"{_diInvalid}\\{strMoveFileName.Substring(strMoveFileName.LastIndexOf('\\'))}");

                    }
                }
            }
            else
            {
                // Fills out the totals in the header of the form.
                SetTableLayOutTotals();
                tsslDenieds.Text = string.Format("Denieds: {0}", _dsRecords.Tables[dgvDenieds.Name].Rows.Count);
                tsslNotProcessed.Text = string.Format("Not Processed: {0}", _dsRecords.Tables[dgvNotProcessed.Name].Rows.Count);
                tsslProcessed.Text = string.Format("Processed: {0}", _dsRecords.Tables[dgvProcessed.Name].Rows.Count);
                tsslEOB.Text = string.Format("EOBs: {0}", _dsRecords.Tables[dgvEOB.Name].Rows.Count);
                tsmiFirst20_Click(dgvProcessed, null);
                tsmiFirst20_Click(dgvDenieds, null);
                tsmiFirst20_Click(dgvNotProcessed, null);
                tsmiFirst20_Click(dgvEOB, null);

            }
            return;
        }
        #endregion UHC
        //foreach (DataRow dr in m_dtNotes.Rows)
        //{
        //    Application.DoEvents();
        //}
    }


    /// <summary>
    /// Checks to file to see if it
    /// 1. Get File date, bill cycyle and file number from the ISA??
    /// 1.check for CLP's with an account number beginning with a 'C' or 'D'
    /// 2. Loads the appropriate grids with the claim status if our account's are found.
    /// </summary>
    /// <param name="strFileName">file name with path</param>
    /// <returns></returns>
    private bool ProcessMedicare835File(string strFileName)
    {

        bool bRetVal = false;
        tbFileName.Text = string.Format("File Name: {0}", strFileName);
        tbFileName.Tag = strFileName;
        // read the file into a string.
        string strLine = RFCObject.GetFileContents(strFileName).Replace(Environment.NewLine, "");
        strLine = strLine.Replace("\n", "");
        strLine = strLine.Replace("\r", "");
        // wdk 20090901 added for processing new file from CAHABA that has multiple ISA's
        ArrayList test = new(strLine.Split(new string[] { "CLP*L", "CLP*C", "CLP*D" }, StringSplitOptions.RemoveEmptyEntries));
        ArrayList al = new(strLine.Split(new string[] { "~ISA" }, StringSplitOptions.RemoveEmptyEntries));
        foreach (string sAl in al)
        {
            Application.DoEvents();
            if (sAl.Contains("*440002"))
            {
                strLine = sAl;
                break;
            }
        }
        // split the file on the GS's the zero element should be the ISA
        string[] strGS = strLine.Split(new string[] { "~GS*" }, StringSplitOptions.RemoveEmptyEntries);
        string[] strISAElements = strGS[0].Split(new char[] { '*' });

        tbBillCycle.Text = string.Format("Bill Cycle: {0}", strISAElements[9]);
        tbBillCycle.Tag = strISAElements[9];
        _strComponentSeperator = strISAElements[16];

        // our GS[1] and above should be real GS segments without the GS*
        for (int i = 1; i <= strGS.GetUpperBound(0); i++)
        {
            if (ParseMedicareGS(strGS[i]))
            {
                bRetVal = true;
            }
        }

        return bRetVal;
    }

    /// <summary>
    /// Checks to file to see if it
    /// 1. Get File date, bill cycyle and file number from the ISA??
    /// 1.check for CLP's with an account number beginning with a 'C' or 'D'
    /// 2. Loads the appropriate grids with the claim status if our account's are found.
    /// </summary>
    /// <param name="strFileName">file name with path</param>
    /// <returns></returns>
    private bool ProcessUHC835File(string strFileName)
    {
        bool bRetVal = true;
        tbFileName.Text = string.Format("File Name: {0}", strFileName);
        tbFileName.Tag = strFileName;
        // read the file into a string.
        string strLine = RFCObject.GetFileContents(strFileName).Replace(Environment.NewLine, "");
        strLine = strLine.Replace("\n", "");
        strLine = strLine.Replace("\r", "");


        // split the file using the "~" to check for multiple packets in the file. if we ever get 
        // a UHC like this we will have to handle.
        ArrayList alFile = new(strLine.Split(new char[] { '~' }));

        // splitting on ~CLP means 1. the header info is in alClaims[0] 
        ArrayList alClaims = new(strLine.Split(new string[] { "~CLP*", "~SE*" }, StringSplitOptions.RemoveEmptyEntries));
        alClaims.RemoveAt(alClaims.Count - 1); // removing the split out SE,GE, and IEA so it doesn't have to be handled later.

        // split the header info into an an arrray list.
        ArraySegment<string> aSegHeader = new(alClaims[0].ToString().Split(new char[] { '~' }));
        #region Process the header info.
        foreach (string strHeaderSegment in aSegHeader.Array)
        {
            Application.DoEvents();
            if (strHeaderSegment.StartsWith("ISA*"))
            {
                string[] strISAElements = strHeaderSegment.Split(new char[] { '*' });
                tbBillCycle.Text = string.Format("Bill Cycle: {0}", strISAElements[9]);
                tbBillCycle.Tag = strISAElements[9];
                _strComponentSeperator = strISAElements[16];
                continue;
            }
            if (strHeaderSegment.StartsWith("GS*"))
            {
                // get the GS data necessary
                string[] strGSHeaderElements = strHeaderSegment.Split(new char[] { '*' });
                // Eft Date
                Time.StringToHL7Time(strGSHeaderElements[4], out DateTime dtEftDate);
                tbFileDate.Text = $"EFT Date: {dtEftDate.ToString("d")}";
                tbFileDate.Tag = dtEftDate.ToString("d");
                // File Number
                tbFileNumber.Text = $"File # {strGSHeaderElements[6]}";
                tbFileNumber.Tag = strGSHeaderElements[6];
                continue;
            }

            if (strHeaderSegment.StartsWith("TRN*"))
            {

                // use the check number/eft Number to check for previously posted checks so as not to duplicate
                string[] strTRNElements = strHeaderSegment.Split(new char[] { '*' });

                int nRec = ThereArePreviouslyPostedChecks(strTRNElements[2]);
                tbCheckNo.Text = $"Chk No: {strTRNElements[2]}";
                tbCheckNo.Tag = strTRNElements[2];
                if (nRec > 0)
                {
                    if (MessageBox.Show($"{tbFileName.Text} already posted {nRec} chk records.\r\r\n Select YES to move to the saved directory.", "FILE POSTING ERROR", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        MoveFileToSavedDirectory(); // handles the exceptions thrown
                        bRetVal = false;
                    }
                    else
                    {
                        tsmiPostCheckRecords.Enabled = false;
                        tsmiPostCheckRecords.ToolTipText = "This file has been previously posted to the chk table.";
                    }
                }
                m_strarrRecordsInsert.SetValue(strTRNElements[2], (int)Col835Grids.eCheckNo);
                continue;
                // WDK 20130725 added 
                //  m_strarrRecordsInsertAddContractual.SetValue(strTRNElements[2], (int)col835Grids.eCheckNo);
            }

            // get the check date from the DTM*405 which should be in strFile[6]
            if (strHeaderSegment.StartsWith("DTM*405*"))
            {
                string[] strDTMElements = strHeaderSegment.Split(new char[] { '*' });
                DateTime dtCheckDate;
                Utilities.Time.StringToHL7Time(strDTMElements[2], out dtCheckDate);
                tbCheckDate.Text = $"Chk Date: {dtCheckDate.ToString("d")}";
                tbCheckDate.Tag = dtCheckDate.ToString("d");
                continue;
            }
            if (strHeaderSegment.StartsWith("N1*PR*")) // be sure to keep this one before the skip region below.
            {
                rtbCheckSource.Text = strHeaderSegment.Replace("N1*PR*", "");
                string strPayer = rtbCheckSource.Text;
                if (strPayer == "UNITEDHEALTHCARE SERVICES INC AND ITS AFFILIATES" ||
                    strPayer == "UNITEDHEALTHCARE INSURANCE COMPANY AND ITS AFFILIATES" ||
                    strPayer.StartsWith("UNITEDHEALTHCARE SERVICES COMPANY O"))
                {
                    _strFinCode = "L";
                    _strInsCode = "UHC";
                    rtbCheckSource.Tag = (string)"UHC REMITTANCE";
                }
                else
                {
                    MessageBox.Show($"[{"UHC"}] Payer is not recognized.");
                    bRetVal = false;
                }
                continue;
            }

            #region Header segments not used so SKIP
            if (strHeaderSegment.StartsWith("ST*"))
            {
                continue;
            }
            if (strHeaderSegment.StartsWith("BPR*"))
            {
                continue;
            }
            if (strHeaderSegment.StartsWith("REF*"))
            {
                continue;
            }
            if (strHeaderSegment.StartsWith("N1*") ||
                strHeaderSegment.StartsWith("N3*") ||
                strHeaderSegment.StartsWith("N4*"))
            {
                continue;
            }
            if (strHeaderSegment.StartsWith("PER*"))
            {
                continue;
            }
            if (strHeaderSegment.StartsWith("LX*"))
            {
                continue;
            }
            if (strHeaderSegment.StartsWith("TS3*"))
            {
                continue;
            }
            #endregion Header segments not used

        }
        #endregion Process the header info

        #region Process the claims

        foreach (string strClaim in alClaims.GetRange(1, alClaims.Count - 1))
        {
            Application.DoEvents();
            // ProcessUHCSVC(strClaim); put this back when fixing uhc

            // strClaimParts[0] will be the patient info --strClaimParts[0+X] will contain the individual charges
            ArrayList alClaimParts = new(strClaim.Split(new string[] { "~SVC*" }, StringSplitOptions.RemoveEmptyEntries));
            string[] strClaimHeaderParts = alClaimParts[0].ToString().Split(new char[] { '~' });
            double dClaimTotalCharge = 0.00f;
            double dClaimTotalPaid = 0.00f;
            double dClaimTotalPatResp = 0.00f;
            double dClaimTotalContractual = 0.00f;

            if (strClaimHeaderParts[0].StartsWith("L") || strClaimHeaderParts[0].StartsWith("C") || strClaimHeaderParts[0].StartsWith("D"))
            {
                ArrayList strClaimHeaderInfo = new(strClaimHeaderParts[0].Split(new char[] { '~' }));
                // we only care about strClaimHeaderInfo[0]
                string[] strInfo = strClaimHeaderInfo[0].ToString().Split(new char[] { '*' });

                dClaimTotalCharge = double.Parse(double.Parse(strInfo[2]).ToString("F2").ToString());
                dClaimTotalPaid = double.Parse(double.Parse(strInfo[3]).ToString("F2").ToString());
                dClaimTotalPatResp = 0;
                if (!string.IsNullOrEmpty(strInfo[4].ToString()))
                {
                    dClaimTotalPatResp = double.Parse(double.Parse(strInfo[4]).ToString("F2").ToString());
                }
                dClaimTotalContractual =
                    double.Parse(double.Parse((dClaimTotalCharge - (dClaimTotalPaid + dClaimTotalPatResp)).ToString()).ToString("F2"));
            }
            else
            {
                continue;
            }

            foreach (string strServiceLine in alClaimParts.GetRange(1, alClaimParts.Count - 1))
            {
                Application.DoEvents();
                ArrayList strServiceLineParts = new ArrayList(strServiceLine.Split(new char[] { '~' }));
                // strServiceLineParts[0] contains the cpt4 and the amount totals for the charge
                string[] strCPT4 = null;
                double dCharge = 0.00f;
                double dPaid = 0.00f;
                double dPatResp = 0.00f;
                // double dContractual = 0.00f;
                int nQty = 0;
                foreach (string strLinePart in strServiceLineParts)
                {
                    Application.DoEvents();
                    if (strLinePart.StartsWith("DTM") ||
                        strLinePart.StartsWith("AMT"))
                    {
                        continue;
                    }

                    if (strLinePart.StartsWith("HC"))
                    {
                        strCPT4 = strServiceLineParts[0].ToString().Split(new char[] { '*' });
                        dCharge = double.Parse(double.Parse(strCPT4[1]).ToString("F2"));
                        dPaid = double.Parse(double.Parse(strCPT4[2]).ToString("F2"));
                        dPatResp = double.Parse(double.Parse(
                            string.IsNullOrEmpty(strCPT4[3]) ? "0.00" : strCPT4[3]).ToString("F2"));
                        nQty = int.Parse(strCPT4[4]);
                        continue;
                    }
                    if (strLinePart.StartsWith("CAS*PR*"))
                    {
                        dPatResp = double.Parse(double.Parse(strLinePart.Split(new char[] { '*' })[3]).ToString("F2").ToString());
                        continue;
                    }
                    if (strLinePart.StartsWith("CAS*PI*"))
                    {
                        dPatResp = double.Parse(double.Parse(strLinePart.Split(new char[] { '*' })[3]).ToString("F2").ToString());
                        continue;
                    }
                }
            }
        }

        #endregion Process the claims

        return bRetVal;
    }

    /// <summary>
    /// Checks to file to see if it
    /// 1. Get File date, bill cycyle and file number from the ISA??
    /// 1.check for CLP's with an account number beginning with a 'C' or 'D'
    /// 2. Loads the appropriate grids with the claim status if our account's are found.
    /// </summary>
    /// <param name="strFileName">file name with path</param>
    /// <returns></returns>
    private bool ProcessBluecross835File(string strFileName)
    {
        bool bRetVal = false;
        tbFileName.Text = string.Format("File Name: {0}", strFileName);
        tbFileName.Tag = strFileName;
        // read the file into a string.
        string strLine = RFCObject.GetFileContents(strFileName).Replace(Environment.NewLine, "");
        strLine = strLine.Replace("\n", "");
        strLine = strLine.Replace("\r", "");
        // wdk 20140904 to see actual check totals for Carol
        ArrayList alST = new ArrayList(strLine.Split(new string[] { "ST*835*" }, StringSplitOptions.RemoveEmptyEntries));
        foreach (string str in alST)
        {
            Application.DoEvents();
            // if the ST element has none of our accounts continue
            //GetLabAccounts(str);
            if (GetLabAccounts(str) == 0)
            {
                Application.DoEvents();
                continue;
            }

            string lbString = "CHECK ";
            ArrayList alTRN = new ArrayList(str.Split(new string[] { "TRN*1*" }, StringSplitOptions.RemoveEmptyEntries));
            lbString += alTRN[1].ToString().Substring(0, alTRN[1].ToString().IndexOf('*'));
            ArrayList alBPR = new ArrayList(str.Split(new string[] { "BPR*I*", "BPR*H*" }, StringSplitOptions.RemoveEmptyEntries));
            lbString += string.Format(" amount {0}", alBPR[1].ToString().Substring(0, alBPR[1].ToString().IndexOf('*')));
            ArrayList alParts = new(alBPR[1].ToString().Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries));
            var queryPLB = from string strPLBParts in alParts
                           where strPLBParts.Substring(0, 3) == "PLB"
                           select strPLBParts;

            if (queryPLB.Any())
            {
                int nDex = alParts.IndexOf(queryPLB.ToArray()[0]);
                string strPLB = alParts[nDex].ToString();

                string[] strReCoups = strPLB.Split(new char[] { '*' });
                decimal dReCoups = 0.00M;
                for (int i = 3; i <= strReCoups.GetUpperBound(0); i++)
                {
                    decimal result = 0.00M;
                    if (decimal.TryParse(strReCoups[i], out result))
                    {
                        dReCoups += result;
                    }
                }
                lbString += string.Format(" - PLB {0}", dReCoups);
            }
            lbChecks.Items.Add(lbString);
        }

        // split the file on the GS's the zero element should be the ISA
        string[] strGS = strLine.Split(new string[] { "~GS*" }, StringSplitOptions.RemoveEmptyEntries);
        string[] strISAElements = strGS[0].Split(new char[] { '*' });

        tbBillCycle.Text = $"Bill Cycle: {strISAElements[9]}";
        tbBillCycle.Tag = strISAElements[9];
        _strComponentSeperator = strISAElements[16];

        // our GS[1] and above should be real GS segments without the GS*
        for (int i = 1; i <= strGS.GetUpperBound(0); i++)
        {
            Application.DoEvents();
            if (ParseBlueCrossGS(strGS[i]))
            {
                bRetVal = true;
            }
        }

        return bRetVal;
    }

    private static int GetLabAccounts(string str)
    {
        System.Text.RegularExpressions.MatchCollection accCollection;
        accCollection = System.Text.RegularExpressions.Regex.Matches(str, @"[C-D,L]+\d{6,7}");
        return accCollection.Count;
    }


    /// <summary>
    /// The GS segment only contains the bill cycle (03) date and file number (05)for our needs
    /// </summary>
    /// <param name="strGS"></param>
    /// <returns></returns>
    private bool ParseMedicareGS(string strGS)
    {
        bool bRetVal = false;
        // GS segment without the GS* 
        //string[] strST = strGS.Split(new string[] { "~ST*835" }, StringSplitOptions.RemoveEmptyEntries);// zero element has ST, TRN, BPR, DTM*405 elements we are looking for
        //09/22/2008 files being sent with newline between '~' and 'ST*835'
        string[] strST = strGS.Split(new string[] { "ST*835" }, StringSplitOptions.RemoveEmptyEntries);// zero element has ST, TRN, BPR, DTM*405 elements we are looking for
        string[] strGSHeaderElements = strST[0].Split(new char[] { '*' });
        // Eft Date
        DateTime dtEftDate;
        Utilities.Time.StringToHL7Time(strGSHeaderElements[3], out dtEftDate);
        tbFileDate.Text = string.Format("EFT Date: {0}", dtEftDate.ToString("d"));
        tbFileDate.Tag = dtEftDate.ToString("d");
        // File Number
        tbFileNumber.Text = string.Format("File # {0}", strGSHeaderElements[5]);
        tbFileNumber.Tag = strGSHeaderElements[5];

        foreach (string strSTElement in strST)
        {
            Application.DoEvents();
            if (strSTElement.StartsWith("HP*")) //Blue cross Hospital Payment
            {
                continue;
            }

            if (ParseST(strSTElement))
            {
                bRetVal = true;
                if (rtbCheckSource.Text == "TLC")
                {

                    return (bRetVal);
                }
            }
        }

        return bRetVal;
    }


    /// <summary>
    /// The GS segment only contains the bill cycle (03) date and file number (05)for our needs
    /// </summary>
    /// <param name="strGS"></param>
    /// <returns></returns>
    private bool ParseBlueCrossGS(string strGS)
    {
        bool bRetVal = false;
        // GS segment without the GS* 
        //string[] strST = strGS.Split(new string[] { "~ST*835" }, StringSplitOptions.RemoveEmptyEntries);// zero element has ST, TRN, BPR, DTM*405 elements we are looking for
        //09/22/2008 files being sent with newline between '~' and 'ST*835'
        string[] strST = strGS.Split(new string[] { "ST*835" }, StringSplitOptions.RemoveEmptyEntries);// zero element has ST, TRN, BPR, DTM*405 elements we are looking for
        string[] strGSHeaderElements = strST[0].Split(new char[] { '*' });
        // Eft Date
        DateTime dtEftDate;
        Time.StringToHL7Time(strGSHeaderElements[3], out dtEftDate);
        tbFileDate.Text = $"EFT Date: {dtEftDate.ToString("d")}";
        tbFileDate.Tag = dtEftDate.ToString("d");
        // File Number
        tbFileNumber.Text = $"File # {strGSHeaderElements[5]}";
        tbFileNumber.Tag = strGSHeaderElements[5];

        foreach (string strSTElement in strST)
        {
            Application.DoEvents();
            if (strSTElement.StartsWith("HP*")) //Blue cross Hospital Payment
            {
                continue;
            }
            if (strSTElement.StartsWith("HI*")) //Blue cross Institutional Paymeny
            {
                continue;
            }
            if (ParseBlueCrossST(strSTElement))
            {
                bRetVal = true;
                if (rtbCheckSource.Text == "TLC")
                {
                    return (bRetVal);
                }
            }
        }

        return bRetVal;
    }

    /// <summary>
    /// Loads the ST data for the file for our info. 
    /// </summary>
    /// <param name="strST"></param>
    private bool ParseST(string strST)
    {
        InitializeStringArrays(); // clears all the string arrays the first time through
        bool bRetVal = false;

        tbProviderID.Text = "NO PROVIDER ID";
        tbProviderID.Tag = (string)"NO PROVIDER ID";
        //string[] strCLP = strST.Split(new string[] { "~CLP*" }, StringSplitOptions.RemoveEmptyEntries);
        //09/22/2008 wdk file format now has a new line between '~' and 'CLP*'
        string[] strCLP = strST.Split(new string[] { "CLP*" }, StringSplitOptions.RemoveEmptyEntries);

        // ST for TLC contains a TRN. TRN02 is the check number. BRP02 is check amount
        // BPR if TLC has a useful check amount,
        // TRN02 (Check number),
        // DTM*405*03 (bill cycle)
        string[] strSTHeader = strCLP[0].Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string str in strCLP)
        {
            Application.DoEvents();
            string strPR;
            if ((str.StartsWith("L") || str.StartsWith("C") || str.StartsWith("D")) && str.Contains("CAS*PR*"))
            {
                strPR = str;
            }
        }
        for (int i = 0; i <= strSTHeader.GetUpperBound(0); i++)
        {
            try
            {
                if (strSTHeader[i].IndexOf("N3", 0, 3) > -1 || // address
                        strSTHeader[i].IndexOf("N4", 0, 3) > -1 || //csz
                            strSTHeader[i].IndexOf("PER", 0, 3) > -1 // contact info
                    )
                {
                    // insurance address lines can be skipped.
                    continue;
                }
                if (strSTHeader[i].IndexOf("BPR", 0, 3) > -1)
                {
                    string[] strBPRElements = strSTHeader[i].ToString().Split(new char[] { '*' });
                    tbCheckAmt.Text = string.Format("Chk Amt: {0}", strBPRElements[2]);
                    tbCheckAmt.Tag = strBPRElements[2];
                    continue;
                }
                if (strSTHeader[i].IndexOf("TRN", 0, 3) > -1)
                {
                    string[] strTRNElements = strSTHeader[i].Split(new char[] { '*' });
                    // 05/07/2008 wdk/rgc moved from posting check records to loading  the file.
                    // wdk 20130822 retrofit to remove the "OEPRA0" from the check no.
                    int nRec = ThereArePreviouslyPostedChecks(strTRNElements[2]);
                    if (nRec == 0)
                    {
                        nRec = ThereArePreviouslyPostedChecks(strTRNElements[2].Replace("0EPRA0", "").Replace("0EZPS0", ""));
                    }
                    tbCheckNo.Text = string.Format("Chk No: {0}", strTRNElements[2]);
                    tbCheckNo.Tag = strTRNElements[2];
                    if (nRec > 0)
                    {
                        if (MessageBox.Show($"{tbFileName.Text} already posted {nRec} chk records.\r\r\n Select YES to move to the saved directory.", "FILE POSTING ERROR", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            MoveFileToSavedDirectory();
                            return false;
                        }
                        else
                        {
                            // tsmiPostCheckRecords.Enabled = false;
                            tsmiPostCheckRecords.ToolTipText = "This file has been previously posted to the chk table.";
                        }
                    }
                    m_strarrRecordsInsert.SetValue(strTRNElements[2], (int)Col835Grids.eCheckNo);
                    // WDK 20130725 added 
                    //  m_strarrRecordsInsertAddContractual.SetValue(strTRNElements[2], (int)col835Grids.eCheckNo);
                    continue;
                }

                if (strSTHeader[i].IndexOf("DTM*405*", 0, 8) > -1)
                {
                    string[] strDTMElements = strSTHeader[i].Split(new char[] { '*' });
                    DateTime dtCheckDate;
                    Utilities.Time.StringToHL7Time(strDTMElements[2], out dtCheckDate);
                    tbCheckDate.Text = $"Chk Date: {dtCheckDate.ToString("d")}";
                    tbCheckDate.Tag = dtCheckDate.ToString("d");
                    continue;
                }
                if (strSTHeader[i].IndexOf("N1*PR", 0, 5) > -1)
                {
                    rtbCheckSource.Text = strSTHeader[i].Replace("N1*PR*", "");
                    string strPayer = rtbCheckSource.Text;
                    if (strPayer == "VSHP TENNCARE SELECT" ||
                        strPayer == "VSHP BLUECARE RISK EAST/WEST")
                    {
                        _strFinCode = "D";
                        _strInsCode = "TNBC";
                        rtbCheckSource.Tag = (string)"TNBC REMITTANCE";
                        continue;

                    }

                    if (strPayer == "BLUE CORSS" ||
                        strPayer == "BLUECROSS BLUESHIELD OF TENNESSEE" ||
                        strPayer == "BCBST BLUEADVANTAGE" ||
                        strPayer == "BLUE CROSS AND BLUE SHIELD OF TENNESSEE")
                    {
                        _strFinCode = "B";
                        _strInsCode = "BC";
                        rtbCheckSource.Tag = (string)"BC REMITTANCE";
                        continue;

                    }
                    if (strPayer == "MEDICARE" ||
                        strPayer == "CAHABA GBA, LLC (TN)" ||
                        strPayer == "JJ MAC TN - PALMETTO GBA #10311")
                    {
                        _strFinCode = "A";
                        _strInsCode = "MC";
                        rtbCheckSource.Tag = (string)"SSI REMITTANCE";
                        continue;
                    }
                    if (strPayer == "UNITEDHEALTHCARE SERVICES INC AND ITS AFFILIATES")
                    {
                        _strFinCode = "L";
                        _strInsCode = "UHC";
                        rtbCheckSource.Tag = (string)"UHC REMITTANCE";
                    }
                    if (strPayer == "AETNA")
                    {
                        _strFinCode = "L";
                        _strInsCode = "AETNA";
                        rtbCheckSource.Tag = (string)"AETNA REMITTANCE";
                    }

                }
                // 04/25/2008 wdk had to rewrite this because the TLC files contain hospital id (626010402) used for everyone MED CTR MED PROD etc.
                // TLC          N1*PE*MED CTR/LAB*FI*626010402~
                // MEDICARE     N1*PE*JACKSON-MADISON CO GEN HOSP*XX*1720160708~
                // MEDICARE     N1*PE*JACKSON-MADISON COUNTY GENERAL*XX*1093705428~ // 07/10/2008 wdk added 
                //if (strSTHeader[i].StartsWith("N1*PE*")) //04/25/2008 wdk does not work for TLC hospital id (626010402) used for everyone MED CTR MED PROD etc.
                if (strSTHeader[i].StartsWith("N1*PE*MED CTR/LAB*"))
                {
                    string[] strN1Elements = strSTHeader[i].Split(new char[] { '*' });
                    tbProviderID.Text = $"Provider ID: {strN1Elements[4]}";
                    tbProviderID.Tag = (string)strN1Elements[4];
                    rtbCheckSource.Text = "TLC";
                    continue;
                }
                //07/10/2008 wdk added || strSTHeader[i].StartsWith("N1*PE*JACKSON-MADISON COUNTY GENERAL*") to below because the new files only contain this string for the payer 
                if (strSTHeader[i].StartsWith("N1*PE*JACKSON-MADISON CO GEN HOSP*") || strSTHeader[i].StartsWith("N1*PE*JACKSON-MADISON COUNTY GENERAL*"))
                {
                    string[] strN1Elements = strSTHeader[i].Split(new char[] { '*' });
                    tbProviderID.Text = $"Provider ID: {strN1Elements[4]}";
                    tbProviderID.Tag = (string)strN1Elements[4];
                    rtbCheckSource.Text = "MC";
                    continue;
                }
                // wdk 20090901 added for CAHABA
                if (strSTHeader[i].StartsWith("N1*PE*JACKSON-MADISON COUNTY GENERAL*XX*1093705428")) //rgc/wdk 20090904 the hospital also has provider id 1225029390 we think our checks are in 1093705428 provider id
                {
                    string[] strN1Elements = strSTHeader[i].Split(new char[] { '*' });
                    tbProviderID.Text = $"Provider ID: {strN1Elements[4]}";
                    tbProviderID.Tag = (string)strN1Elements[4];
                    rtbCheckSource.Text = "MC";
                    continue;
                }
                // wdk 20110411 added for CAHABA version 2 see version one above
                if (strSTHeader[i].StartsWith("N1*PE*JACKSON MADISON COUNTY GENERAL*XX*1093705428")) //rgc/wdk 20090904 the hospital also has provider id 1225029390 we think our checks are in 1093705428 provider id
                {
                    string[] strN1Elements = strSTHeader[i].Split(new char[] { '*' });
                    tbProviderID.Text = $"Provider ID: {strN1Elements[4]}";
                    tbProviderID.Tag = (string)strN1Elements[4];
                    rtbCheckSource.Text = "MC";
                    continue;
                }
                if (strSTHeader[i].StartsWith("N1*PR*BLUE"))
                {
                    string[] strN1Elements = strSTHeader[i].Split(new char[] { '*' });
                    tbProviderID.Text = $"Provider ID: {"1000427"}";
                    tbProviderID.Tag = "1000427";
                    rtbCheckSource.Text = "BLUE CROSS";
                    continue;
                }
            }
            catch (IndexOutOfRangeException)
            {
                // could have an ST or LX just continue
                continue;
            }
            catch (ArgumentOutOfRangeException)
            {
                //line too short continue we don't expect anything from this line.
                continue;
            }
        }

        foreach (string strCLPElement in strCLP)
        {
            Application.DoEvents();

            string[] strCLPElements = strCLPElement.Split(new char[] { '*' });
            if (strCLPElements[0].ToString().Length == 0) // no account number in CLP in source file.
            {
                if (strCLPElement.IndexOf("Medical Center Laboratory") > -1)
                {
                    _eRR.m_Logfile.WriteLogFile(string.Format("No account in source file. \r\n{0}", strCLPElement));
                }
                continue;
            }
            // 09/05/2008 wdk
            //626010402_20080831_1054.835 has a clp  with an extra char at the end 
            //CLP*c2607227`*
            string strTestAcc = strCLPElements[0].ToString();

            int nAccLength = 9;
            try
            {
                if (strCLPElements[0][1] == 'A')
                {
                    nAccLength = 10;
                }
            }
            catch (IndexOutOfRangeException)
            {
                _eRR.m_Logfile.WriteLogFile(strCLPElement);
                continue;
            }
            if (strCLPElements[0].Length > nAccLength) // no account number in CLP in source file.
            {
                if (strCLPElement.IndexOf("Medical Center Laboratory") > -1)
                {
                    _eRR.m_Logfile.WriteLogFile($"Account [{strCLPElements[0]} is to long.. \r\n{strCLPElement}");
                }
                continue;
            }
            // end of 09/05/2008 wdk
            if (strCLPElement[0].ToString().ToUpper() == "L" || strCLPElement[0].ToString().ToUpper() == "C" || strCLPElement[0].ToString().ToUpper() == "D")
            {
                if (_cAcc.AccountIsValid(strCLPElements[0].Replace("A", "")))
                {
                    ParseSVC(strCLPElement);
                    m_strarrEOBInsert.SetValue("", (int)Col835EOB.ePatStat); // 06/03/2008 not used so set blank
                    _dsRecords.Tables[dgvEOB.Name].Rows.Add(m_strarrEOBInsert);
                    AddTotalsToArray(m_strarrEOBInsert);
                    bRetVal = true;
                }
            }
            else
            {
                continue;
            }
        }

        return bRetVal;
    }

    /// <summary>
    /// Loads the ST data for the file for our info. 
    /// </summary>
    /// <param name="strST"></param>
    private bool ParseBlueCrossST(string strST)
    {
        InitializeStringArrays(); // clears all the string arrays the first time through
        bool bRetVal = false;

        tbProviderID.Text = "NO PROVIDER ID";
        tbProviderID.Tag = (string)"NO PROVIDER ID";
        //string[] strCLP = strST.Split(new string[] { "~CLP*" }, StringSplitOptions.RemoveEmptyEntries);
        //09/22/2008 wdk file format now has a new line between '~' and 'CLP*'
        string[] strCLP = strST.Split(new string[] { "CLP*" }, StringSplitOptions.RemoveEmptyEntries);


        // ST for TLC contains a TRN. TRN02 is the check number. BRP02 is check amount
        // BPR if TLC has a useful check amount,
        // TRN02 (Check number),
        // DTM*405*03 (bill cycle)
        string[] strSTHeader = strCLP[0].Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);
        ArrayList alSTHeader = new(strCLP[0].Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries));

        alSTHeader.RemoveAt(0);// we know the zero element is the ST NUMBER only so skip
        string strValue = QueryArrayList(alSTHeader, "LX*");
        if (!string.IsNullOrEmpty(strValue))
        {
            alSTHeader.RemoveAt(alSTHeader.IndexOf(strValue));
        }
        strValue = QueryArrayList(alSTHeader, "TRN");
        if (!string.IsNullOrEmpty(strValue))
        {
            string[] strTRNElements = strValue.Split(new char[] { '*' });
            //strSTHeader[i].Split(new char[] { '*' });
            // 05/07/2008 wdk/rgc moved from posting check records to loading  the file.
            // wdk 20130822 retrofit to remove the "OEPRA0" from the check no.
            int nRec = ThereArePreviouslyPostedChecks(strTRNElements[2]);
            if (nRec == 0)
            {
                nRec = ThereArePreviouslyPostedChecks(strTRNElements[2].Replace("0EPRA0", "").Replace("0EZPS0", ""));
            }
            tbCheckNo.Text = string.Format("Chk No: {0}", strTRNElements[2]);
            tbCheckNo.Tag = strTRNElements[2];
            if (nRec > 0)
            {
                if (MessageBox.Show(string.Format("{0} already posted {1} chk records.\r\r\n Select YES to move to the saved directory.", tbFileName.Text, nRec), "FILE POSTING ERROR", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MoveFileToSavedDirectory();
                    return false;
                }
                else
                {
                    // tsmiPostCheckRecords.Enabled = false;
                    tsmiPostCheckRecords.ToolTipText = "This file has been previously posted to the chk table.";
                }
            }
            m_strarrRecordsInsert.SetValue(strTRNElements[2], (int)Col835Grids.eCheckNo);
        }
        strValue = QueryArrayList(alSTHeader, "BPR");
        if (!string.IsNullOrEmpty(strValue))
        {
            string[] strBPRElements = strValue.Split(new char[] { '*' });
            //strSTHeader[i].ToString().Split(new char[] { '*' });
            tbCheckAmt.Text = string.Format("Chk Amt: {0}", strBPRElements[2]);
            tbCheckAmt.Tag = strBPRElements[2];
            //continue;
        }
        strValue = QueryArrayList(alSTHeader, "DTM*405*");
        if (!string.IsNullOrEmpty(strValue))
        {
            string[] strDTMElements = strValue.Split(new char[] { '*' });
            DateTime dtCheckDate;
            Time.StringToHL7Time(strDTMElements[2], out dtCheckDate);
            tbCheckDate.Text = string.Format("Chk Date: {0}", dtCheckDate.ToString("d"));
            tbCheckDate.Tag = dtCheckDate.ToString("d");
        }
        strValue = QueryArrayList(alSTHeader, "N1*PR");
        if (!string.IsNullOrEmpty(strValue))
        {
            rtbCheckSource.Text = strValue.Replace("N1*PR*", "");
            string strPayer = rtbCheckSource.Text;
            _strFinCode = "B";
            _strInsCode = "BC";
            rtbCheckSource.Tag = (string)"BC REMITTANCE";
        }

        //skip the first item as it is the ST item count

        foreach (string strCLPElement in strCLP)
        {
            Application.DoEvents();
            if (GetLabAccounts(strCLPElement) == 0)
            {
                continue;
            }

            ParseBlueCrossSVC(strCLPElement);
            m_strarrEOBInsert.SetValue("", (int)Col835EOB.ePatStat); // 06/03/2008 not used so set blank
            _dsRecords.Tables[dgvEOB.Name].Rows.Add(m_strarrEOBInsert);
            AddTotalsToArray(m_strarrEOBInsert);
            bRetVal = true;
        }

        return bRetVal;
    }

    /// <summary>
    /// This is only good for items that are known to be 1 in the array list
    /// 
    /// </summary>
    /// <param name="al"></param>
    /// <param name="strVar"></param>
    /// <returns></returns>
    private static string QueryArrayList(ArrayList al, string strVar)
    {

        string strRetVal = null;
        int nVarLen = strVar.Length;
        try
        {
            var queryRVAL = from string strParts in al
                            where strParts.Substring(0, nVarLen) == strVar
                            select strParts;

            if (queryRVAL.Count() > 0)
            {
                int nDex = al.IndexOf(queryRVAL.ToArray()[0]);
                strRetVal = al[nDex].ToString();
            }
        }
        catch (IndexOutOfRangeException)
        {
        }
        finally
        {
        }
        return strRetVal;

    }

    private void AddTotalsToArray(string[] strarrEOBInsert)
    {
        decimal dTemp = 0.00m;
        _d_dgvEOB_Totals[(int)Col835EOB.eChargesReported] +=
            decimal.TryParse(strarrEOBInsert[(int)Col835EOB.eChargesReported], out dTemp) ? dTemp : 0.00m;
        _d_dgvEOB_Totals[(int)Col835EOB.ePaymentDataNetReimbAmt] +=
            decimal.TryParse(strarrEOBInsert[(int)Col835EOB.ePaymentDataNetReimbAmt], out dTemp) ? dTemp : 0.00m;
        _d_dgvEOB_Totals[(int)Col835EOB.ePaymentDataContAdjAmt] +=
            decimal.TryParse(strarrEOBInsert[(int)Col835EOB.ePaymentDataContAdjAmt], out dTemp) ? dTemp : 0.00m;
        _d_dgvEOB_Totals[(int)Col835EOB.eChargesDenied] +=
            decimal.TryParse(strarrEOBInsert[(int)Col835EOB.eChargesDenied], out dTemp) ? dTemp : 0.00m;
        _d_dgvEOB_Totals[(int)Col835EOB.eOtherAdjAmt] +=
            decimal.TryParse(strarrEOBInsert[(int)Col835EOB.eOtherAdjAmt], out dTemp) ? dTemp : 0.00m;
    }

    /// <summary>
    /// This is our CL(ient) P(ayment) so process it into its component parts by splitting it on the
    /// SVCs. The zero element will be the patient/payment totals demographics and the additional elements will
    /// be the EOB data.
    /// </summary>
    /// <param name="strCLPElement"></param>
    public void ParseSVC(string strCLP)
    {
        // create and intialize local variables
        DateTime dtService = DateTime.Today;

        InitializeEOBVariables(); // 06/20/2008 wdk method extracted to clear our eob variables
        string[] strSVC = strCLP.Split(new string[] { "~SVC*" }, StringSplitOptions.RemoveEmptyEntries);
        // Set the patient data with the CLP data
        SetPatientData(strSVC[0]);

        foreach (string str in strSVC)
        {
            Application.DoEvents();
            if (DateTime.Today < new DateTime(2013, 07, 19, 16, 00, 00))
            {
                continue;
            }
            ProcessMedicareSVC(str); // 06/20/2008 wdk to fix several problems TESTING
        }

        #region SET EOB TOTALS to m_strarrEOBInsert
        // set the eob totals before adding to the eob grid
        // charges
        // charges reported is added to the grid via the SVC parsing.
        m_strarrEOBInsert.SetValue($"{_dEOBChargesDenied:F2}", (int)Col835EOB.eChargesDenied);
        m_strarrEOBInsert.SetValue($"{_dEOBChargesNCovd:F2}", (int)Col835EOB.eChargesNCovd);

        // Patient Libality
        m_strarrEOBInsert.SetValue($"{_dEOBPatLibNCovdCharges:F2}", (int)Col835EOB.ePatientLibNCovdCharges);
        m_strarrEOBInsert.SetValue($"{_dEOBPatLibCoInsurance:F2}", (int)Col835EOB.ePatientLibCoinsurance);

        // Payment data
        m_strarrEOBInsert.SetValue($"{_dEOBPayDataMSPPrimPay:F2}", (int)Col835EOB.ePaymentDataMSPPrimPay);
        m_strarrEOBInsert.SetValue($"{_dEOBPayDataHCPCSAmt:F2}", (int)Col835EOB.ePaymentDataHCPCSAmt);
        m_strarrEOBInsert.SetValue($"{_dEOBPayDataContAdjAmt:F2}", (int)Col835EOB.ePaymentDataContAdjAmt);
        m_strarrEOBInsert.SetValue($"{_dEOBPayDataPatRefund:F2}", (int)Col835EOB.ePaymentDataPatRefund);

        #endregion SET EOB TOTALS to m_strarrEOBInsert    

    }

    /// <summary>
    /// This is our CL(ient) P(ayment) so process it into its component parts by splitting it on the
    /// SVCs. The zero element will be the patient/payment totals demographics and the additional elements will
    /// be the EOB data.
    /// </summary>
    /// <param name="strCLPElement"></param>
    public void ParseBlueCrossSVC(string strCLP)
    {
        // create and intialize local variables
        DateTime dtService = DateTime.Today;

        InitializeEOBVariables(); // 06/20/2008 wdk method extracted to clear our eob variables
        string[] strSVC = strCLP.Split(new string[] { "~SVC*" }, StringSplitOptions.RemoveEmptyEntries);
        // Set the patient data with the CLP data
        SetPatientData(strSVC[0]);

        foreach (string str in strSVC)
        {
            Application.DoEvents();
            if (DateTime.Today < new DateTime(2013, 07, 19, 16, 00, 00))
            {
                continue;
            }
            if (!str.Contains("HC>"))
            {
                continue;
            }
            ProcessBlueCrossSVC(str); // 06/20/2008 wdk to fix several problems TESTING
        }

        #region SET EOB TOTALS to m_strarrEOBInsert
        // set the eob totals before adding to the eob grid
        // charges
        // charges reported is added to the grid via the SVC parsing.
        m_strarrEOBInsert.SetValue($"{_dEOBChargesDenied:F2}", (int)Col835EOB.eChargesDenied);
        m_strarrEOBInsert.SetValue($"{_dEOBChargesNCovd:F2}", (int)Col835EOB.eChargesNCovd);

        // Patient Libality
        m_strarrEOBInsert.SetValue($"{_dEOBPatLibNCovdCharges:F2}", (int)Col835EOB.ePatientLibNCovdCharges);
        m_strarrEOBInsert.SetValue($"{_dEOBPatLibCoInsurance:F2}", (int)Col835EOB.ePatientLibCoinsurance);

        // Payment data
        m_strarrEOBInsert.SetValue($"{_dEOBPayDataMSPPrimPay:F2}", (int)Col835EOB.ePaymentDataMSPPrimPay);
        m_strarrEOBInsert.SetValue($"{_dEOBPayDataHCPCSAmt:F2}", (int)Col835EOB.ePaymentDataHCPCSAmt);
        m_strarrEOBInsert.SetValue($"{_dEOBPayDataContAdjAmt:F2}", (int)Col835EOB.ePaymentDataContAdjAmt);
        m_strarrEOBInsert.SetValue($"{_dEOBPayDataPatRefund:F2}", (int)Col835EOB.ePaymentDataPatRefund);

        #endregion SET EOB TOTALS to m_strarrEOBInsert

    }
    private void ProcessMedicareSVC(string strSVC)
    {

        #region CLEAR m_strarrRecordsInsert


        // Clear all the values for the records insert so each SVC contains unique data except the 
        // account and subscribers name, and status. 
        for (int j = ((int)Col835Grids.eClaimStatus + 1); j <= m_strarrRecordsInsert.GetUpperBound(0); j++)
        {
            m_strarrRecordsInsert.SetValue((string)"", j);

            if ((int)Col835Grids.eAllowed == j ||
                    (int)Col835Grids.eCharges == j ||
                        (int)Col835Grids.eContractualAdjAmt == j ||
                            (int)Col835Grids.ePaid == j ||
                                (int)Col835Grids.eOtherAdjAmt == j ||
                                    (int)Col835Grids.eWriteOffAmt == j // wdk 20130731 added
                )
            {
                m_strarrRecordsInsert.SetValue((string)"0.00", j);
            }
        }
        #endregion CLEAR m_strarrRecordsInsert

        string[] strPayLine = strSVC.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);
        bool bContainsCAS = strSVC.Contains("~CAS*"); // 06/23/2008 wdk If no CAS element they paid full amount of charge

        DateTime dtService = DateTime.Today;

        Decimal dTempAmtPaid = 0.00m;
        Decimal dTempAmtContractual = 0.00m;
        Decimal dTempSVCCharges = 0.00m;
        foreach (string strSVCLine in strPayLine)
        {
            Application.DoEvents();
            string[] strSVCElements = strSVCLine.Split(new char[] { '*' });

            switch (strSVCElements[0].Substring(0, strSVCElements[0].Length <= 2 ? strSVCElements[0].Length : 3))
            {
                #region HC
                case "HC:": // 07/01/2008 rgc/wdk expect Claimsnet to use the ':' seperator
                case "HC^": // TLC //ISA[19] is the component seperator used for this file. 
                case "HC>": // Medicare // this means we have the SVC line without SVC*
                {
                    #region CLEAR m_strarrRecordsInsert


                    // Clear all the values for the records insert so each SVC contains unique data except the 
                    // account and subscribers name, and status. 
                    for (int j = ((int)Col835Grids.eClaimStatus + 1); j <= m_strarrRecordsInsert.GetUpperBound(0); j++)
                    {
                        m_strarrRecordsInsert.SetValue((string)"", j);

                        if ((int)Col835Grids.eAllowed == j ||
                                (int)Col835Grids.eCharges == j ||
                                    (int)Col835Grids.eContractualAdjAmt == j ||
                                        (int)Col835Grids.ePaid == j ||
                                            (int)Col835Grids.eOtherAdjAmt == j)
                        {
                            m_strarrRecordsInsert.SetValue((string)"0.00", j);
                        }
                    }
                    #endregion CLEAR m_strarrRecordsInsert


                    // Processing a new SVC line so fill out what we know
                    // Service Code
                    m_strarrRecordsInsert.SetValue(strSVCElements[0], (int)Col835Grids.eCPT4Code);
                    // Charges Reported
                    dTempSVCCharges = decimal.Parse(strSVCElements[1]);
                    m_strarrRecordsInsert.SetValue(strSVCElements[1], (int)Col835Grids.eCharges);
                    // Allowed and paid are the same
                    m_strarrRecordsInsert.SetValue(strSVCElements[2], (int)Col835Grids.eAllowed);
                    m_strarrRecordsInsert.SetValue(strSVCElements[2], (int)Col835Grids.ePaid);
                    dTempAmtPaid = decimal.Parse(strSVCElements[2]); // set this variable into dgvEOB(colEOB.ePayDataHCPCSAmt) after all the svc's  are processed
                    _dEOBPayDataHCPCSAmt += dTempAmtPaid;
                    // Rev Code
                    m_strarrRecordsInsert.SetValue(strSVCElements[3], (int)Col835Grids.eRevCode);
                    // Units
                    try
                    {
                        m_strarrRecordsInsert.SetValue(strSVCElements[4], (int)Col835Grids.eUnits);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        // rgc/wdk 20090813 new CAHABA file does not contain the units. Old file had
                        // zero listed
                        m_strarrRecordsInsert.SetValue("0", (int)Col835Grids.eUnits);
                    }
                    if (!bContainsCAS) // 06/23/2008 wdk full price paid no CAS line in the split so continue
                    {
                        m_strarrRecordsInsert.SetValue("PIF", (int)Col835Grids.eReason); // P(aid) I(n) F(ull)
                        dtService = new DateTime();
                        string[] strDate = strPayLine[1].Split(new char[] { '*' });
                        Utilities.Time.StringToHL7Time(strDate[2], out dtService);
                        m_strarrRecordsInsert.SetValue(dtService.ToString("d"), (int)Col835Grids.eDateOfService);
                        AddRecordToDataGrid();
                        continue;
                    }
                    break;
                }
                #endregion HC

                #region DTM
                case "DTM":
                {
                    if (strSVCElements[1].Contains("472"))
                    {
                        dtService = new DateTime();
                        Utilities.Time.StringToHL7Time(strSVCElements[2], out dtService);
                        m_strarrRecordsInsert.SetValue(dtService.ToString("d"), (int)Col835Grids.eDateOfService);
                    }

                    break;
                }
                #endregion DTM

                #region CAS
                case "CAS":
                {
                    dTempAmtContractual = decimal.Parse(strSVCElements[3]);
                    //Claim adjustment group / Adjustment Reason Code
                    m_strarrRecordsInsert.SetValue(strSVCElements[1] + "/" + strSVCElements[2], (int)Col835Grids.eReason);
                    // SET SVC elements into the record to be inserted into the recordset and grid. May have been cleared if an OA has been processed before this CO
                    m_strarrRecordsInsert.SetValue(dTempSVCCharges.ToString(), (int)Col835Grids.eCharges);
                    m_strarrRecordsInsert.SetValue(dTempAmtPaid.ToString(), (int)Col835Grids.ePaid);
                    m_strarrRecordsInsert.SetValue(dTempAmtPaid.ToString(), (int)Col835Grids.eAllowed);
                    m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.eOtherAdjAmt);
                    string[] strNoFail = new string[10];
                    strSVCElements.CopyTo(strNoFail, 0);
                    switch (strSVCElements[1])
                    {
                        #region CO Contractual processing
                        case "CO":
                        {
                            switch (strSVCElements[2])
                            {
                                /* wdk 20130724 added  223 Adjustment code for mandated federal, state or local law/regulation that is not already covered by another code and is mandated before a new code can be created.*/
                                case "253": // wdk 20140121 Sequestration - reduction in federal spending 
                                {
                                    _dEOBPayDataContAdjAmt += dTempAmtContractual;
                                    m_strarrRecordsInsert.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eContractualAdjAmt);
                                    break;

                                }
                                case "223":
                                {
                                    dTempAmtContractual = Decimal.Negate(dTempAmtContractual);
                                    _dEOBPayDataContAdjAmt += dTempAmtContractual;
                                    m_strarrRecordsInsert.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eContractualAdjAmt);
                                    break;

                                }
                                case "26": // 04/25/2008 wdk TLC Expenses incurred prior to coverage.??
                                {
                                    dTempAmtContractual = Decimal.Negate(dTempAmtContractual);
                                    _dEOBPayDataContAdjAmt += dTempAmtContractual;
                                    m_strarrRecordsInsert.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eContractualAdjAmt);
                                    break;
                                }
                                case "45": // contractual obligation amount
                                {
                                    _dEOBPayDataContAdjAmt += dTempAmtContractual;
                                    m_strarrRecordsInsert.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eContractualAdjAmt);
                                    break;
                                }
                                case "18": // wdk 20130731 moved here to handle the GZ modifiers
                                {
                                    if (m_strarrRecordsInsert.GetValue((int)Col835Grids.eCPT4Code).ToString().Contains("GZ"))
                                    {
                                        _dChargesWriteOffGZ += dTempAmtContractual;
                                        m_strarrRecordsInsert.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eWriteOffAmt);
                                        m_strarrRecordsInsert.SetValue("200", (int)Col835Grids.eWriteOffCode);
                                        m_strarrRecordsInsert.SetValue(DateTime.Now.ToShortDateString(), (int)Col835Grids.eWriteOffDate);
                                    }
                                    else
                                    {
                                        _dEOBPayDataContAdjAmt += dTempAmtContractual;
                                        m_strarrRecordsInsert.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eContractualAdjAmt);
                                    }
                                    break;
                                }
                                case "50":
                                {
                                    if (m_strarrRecordsInsert.GetValue((int)Col835Grids.eCPT4Code).ToString().Contains("GZ"))
                                    {
                                        _dChargesWriteOffGZ += dTempAmtContractual;
                                        m_strarrRecordsInsert.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eWriteOffAmt);
                                        m_strarrRecordsInsert.SetValue("200", (int)Col835Grids.eWriteOffCode);
                                        m_strarrRecordsInsert.SetValue(DateTime.Now.ToShortDateString(), (int)Col835Grids.eWriteOffDate);
                                        //   dgvProcessed.Columns[(int)col835Grids.eWriteOffDate].Visible = true;
                                        //   dgvProcessed.Columns[(int)col835Grids.eWriteOffCode].Visible = true;

                                        //m_dicBandProcessed.Add(dgvProcessed.Name, dgvProcessed.Columns[(int)col835Grids.eWriteOffDate]);
                                        //m_dicBandProcessed.Add(dgvProcessed.Name, dgvProcessed.Columns[(int)col835Grids.eWriteOffCode]);

                                    }
                                    else
                                    {
                                        m_strarrRecordsInsert.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eWriteOffAmt);
                                    }
                                    break;
                                }
                                case "60": //Charges for outpatient services with this proximity to inpatient services are not covered.                                               //{ removed 07/10/2008 wdk                                                 //    m_dEOBChargesNCovd += dTempAmtContractual;                                                //    break;                                                //}
                                case "96":  //96 can be on both the CO and the OA in the CAS which means NONCOVERED in both places.
                                {
                                    // eob's charges : non covered charges // don't add to contractual add to non covered
                                    _dEOBChargesNCovd += dTempAmtContractual;
                                    m_strarrRecordsInsert.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eContractualAdjAmt);
                                    break;
                                }
                                case "147": // 04/25/2008 wdk TLC Provider contracted/negotiated rate expired or not on file.??
                                {
                                    dTempAmtContractual = Decimal.Negate(dTempAmtContractual);
                                    _dEOBPayDataContAdjAmt += dTempAmtContractual;
                                    m_strarrRecordsInsert.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eContractualAdjAmt);
                                    break;
                                }
                                case "A2": // for TLC CAS*CO*A2's seem to be negative amounts ie -17.49 All we saw with a partial search of file 626010402_20080324_09235.835
                                {
                                    dTempAmtContractual = Decimal.Negate(dTempAmtContractual);
                                    _dEOBPayDataContAdjAmt += dTempAmtContractual;
                                    m_strarrRecordsInsert.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eContractualAdjAmt);
                                    break;
                                }

                                #region Denied Codes
                                case "13": // 06/30/2008 wdk added here per Darlene
                                           //   case "18": // 06/30/2008 wdk added here per Darlene // 07/29/2008 wdk per darlene this is the same as a 45
                                case "24": // 06/30/2008 wdk added here per Darlene
                                case "125": // 07/10/2008 wdk added here per Darlene
                                case "B9": //Services not covered because the patient is enrolled in a Hospice. This change to be effective 4/1/2008: Patient is enrolled in a Hospice.
                                case "B15": // 06/30/2008 wdk added here per Darlene
                                {
                                    _dEOBChargesDenied += dTempAmtContractual;
                                    m_strarrRecordsInsert.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eContractualAdjAmt); // 07/29/2008 wdk should this be here???
                                    break;
                                }

                                #endregion Denied Codes
                            }
                            if (string.IsNullOrEmpty(strNoFail[5]))
                            {
                                break;
                            }
                            m_strarrRecordsInsert.CopyTo(m_strarrRecordsInsertAddContractual, 0);
                            m_strarrRecordsInsertAddContractual.SetValue("0.00", (int)Col835Grids.eCharges);
                            m_strarrRecordsInsertAddContractual.SetValue("0.00", (int)Col835Grids.ePaid);
                            m_strarrRecordsInsertAddContractual.SetValue("0.00", (int)Col835Grids.eAllowed);
                            m_strarrRecordsInsertAddContractual.SetValue("0.00", (int)Col835Grids.eOtherAdjAmt);
                            m_strarrRecordsInsertAddContractual.SetValue(strNoFail[1] + "/" + strNoFail[5], (int)Col835Grids.eReason);
                            switch (strNoFail[5])
                            {
                                /* wdk 20130724 added  223 Adjustment code for mandated federal, state or local law/regulation that is not already covered by another code and is mandated before a new code can be created.*/
                                case "253": //wdk 20140121 added 
                                case "223":
                                {
                                    dTempAmtContractual = decimal.Parse(strNoFail[6]);
                                    _dEOBPayDataContAdjAmt += dTempAmtContractual;
                                    m_strarrRecordsInsertAddContractual.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eContractualAdjAmt);
                                    break;

                                }
                                case "26": // 04/25/2008 wdk TLC Expenses incurred prior to coverage.??
                                {
                                    dTempAmtContractual = decimal.Parse(strNoFail[6]);
                                    _dEOBPayDataContAdjAmt += dTempAmtContractual;
                                    m_strarrRecordsInsertAddContractual.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eContractualAdjAmt);
                                    break;
                                }
                                case "18": // 07/29/2008 wdk added per Carol per Darlene 
                                case "45": // contractual obligation amount
                                {
                                    dTempAmtContractual = decimal.Parse(strNoFail[6]);
                                    _dEOBPayDataContAdjAmt += dTempAmtContractual;
                                    m_strarrRecordsInsertAddContractual.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eContractualAdjAmt);
                                    break;
                                }
                                case "60": //Charges for outpatient services with this proximity to inpatient services are not covered.                                               //{ removed 07/10/2008 wdk                                                 //    m_dEOBChargesNCovd += dTempAmtContractual;                                                //    break;                                                //}
                                case "96":  //96 can be on both the CO and the OA in the CAS which means NONCOVERED in both places.
                                {
                                    // eob's charges : non covered charges // don't add to contractual add to non covered
                                    dTempAmtContractual = decimal.Parse(strNoFail[6]);
                                    _dEOBChargesNCovd += dTempAmtContractual;
                                    m_strarrRecordsInsertAddContractual.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eContractualAdjAmt);
                                    break;
                                }
                                case "147": // 04/25/2008 wdk TLC Provider contracted/negotiated rate expired or not on file.??
                                {
                                    dTempAmtContractual = decimal.Parse(strNoFail[6]);
                                    _dEOBPayDataContAdjAmt += dTempAmtContractual;
                                    m_strarrRecordsInsertAddContractual.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eContractualAdjAmt);
                                    break;
                                }
                                case "A2": // for TLC CAS*CO*A2's seem to be negative amounts ie -17.49 All we saw with a partial search of file 626010402_20080324_09235.835
                                {
                                    dTempAmtContractual = decimal.Parse(strNoFail[6]);
                                    _dEOBPayDataContAdjAmt += dTempAmtContractual;
                                    m_strarrRecordsInsertAddContractual.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eContractualAdjAmt);
                                    break;
                                }

                                #region Denied Codes
                                case "13": // 06/30/2008 wdk added here per Darlene
                                           //   case "18": // 06/30/2008 wdk added here per Darlene // 07/29/2008 wdk per darlene this is the same as a 45
                                case "24": // 06/30/2008 wdk added here per Darlene
                                case "125": // 07/10/2008 wdk added here per Darlene
                                case "B9": //Services not covered because the patient is enrolled in a Hospice. This change to be effective 4/1/2008: Patient is enrolled in a Hospice.
                                case "B15": // 06/30/2008 wdk added here per Darlene
                                {
                                    dTempAmtContractual = decimal.Parse(strNoFail[6]);
                                    _dEOBChargesDenied += dTempAmtContractual;
                                    m_strarrRecordsInsertAddContractual.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eContractualAdjAmt); // 07/29/2008 wdk should this be here???
                                    break;
                                }

                                #endregion Denied Codes
                            }
                            break;
                        }
                        #endregion CO

                        #region CR Credit Processing

                        // CAS*CR*2*-5.56**45*-48.09~
                        //      CAS01 = CR
                        //      CAS02 = 2 (Coinsurance Amount) taken from Reason code table
                        //      CAS03 = -5.56 (for this account we wrote this off)
                        //      CAS04 = "" blank
                        //      CAS05 = 45 (percent)
                        //      CAS06 is the contractual

                        case "CR": // 04/21/2008 wdk for medicare takebacks status = 22
                        {
                            m_strarrRecordsInsert.SetValue(dTempAmtPaid.ToString(), (int)Col835Grids.eContractualAdjAmt);
                            //Payment adjusted because this care may be covered by another payer per coordination of benefits. 
                            //This change to be effective 4/1/2008: This care may be covered by another payer per coordination of benefits.
                            switch (strSVCElements[2])
                            {
                                case "22":
                                {
                                    _dEOBChargesDenied += dTempAmtPaid;
                                    break;
                                }
                                case "45": //file 080409MF.835 has negative contractuals for C2463470
                                {
                                    m_strarrRecordsInsert.SetValue(dTempAmtPaid.ToString(), (int)Col835Grids.eContractualAdjAmt);
                                    _dEOBPayDataContAdjAmt += dTempAmtPaid; // 06/09/2008 wdk added see "CA2491257" in file #1717 on bill cycle 03/31/20089
                                    break;
                                }
                                case "119": // 04/21/2008 wdk for take back from Medicare Previously denied refiled then paid plus this Credit
                                {
                                    _dEOBChargesDenied += dTempAmtPaid;
                                    break;
                                }
                                case "2": // 04/21/2008 wdk for refunds from Medicare
                                {
                                    decimal dTempEOBAmt = decimal.Parse(strSVCElements[6]);
                                    _dEOBPayDataContAdjAmt += dTempEOBAmt;
                                    break;
                                }
                                default:
                                {
                                    _eRR.m_Logfile.WriteLogFile(string.Format("CR Default handler for {0} not completed.\r\nAccount:{1}\r\nFile: {2}",
                                           strSVCElements[2], m_strarrEOBInsert.GetValue((int)Col835EOB.Account), tbFileName.Tag.ToString()));
                                    break;
                                }
                            }

                            if (rtbCheckSource.Text.IndexOf("TLC") > -1) // 04/08/2008 rgc/wdk needs to be tested.
                            {
                                _dEOBPayDataContAdjAmt = Decimal.Negate(_dEOBPayDataContAdjAmt);
                            }
                            break;
                        }
                        #endregion CR Credit Processing

                        #region OA Other Adjustments
                        case "OA": // don't add to contractual
                        {
                            //dTempAmt = decimal.Parse(strSVCElements[2]);
                            // 06/10/2008 the below lines moved from case 23: below.
                            m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.eAllowed);
                            m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.eCharges);
                            m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.ePaid); // don't post OA's because the SVC line dictates the amount paid
                            m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.eContractualAdjAmt);
                            m_strarrRecordsInsert.SetValue(dTempAmtContractual.ToString(), (int)Col835Grids.eOtherAdjAmt);
                            // 06/10/2008 rgc/wdk converted if's to switch for scalibility.
                            switch (strSVCElements[2])
                            {
                                case "23":  // TLC other insurance 04/16/2008 wdk //04/21/2008 wdk Medicare has it also. // 05/27/2008 rgc/wdk MSP Prim Pay() from file #1727 account ca2480693
                                {
                                    _dEOBPayDataMSPPrimPay += dTempAmtContractual;
                                    break;
                                }
                                case "96":
                                {
                                    // put on eob non covered 
                                    _dEOBChargesNCovd += dTempAmtContractual;
                                    break;
                                }
                            }
                            break;
                        }
                        #endregion 0A

                        #region PR Patient Responsiblity

                        // rgc/wdk 20090528 PR 2's may have an additional PR 1 (Deductible) or PR 3(CoPayment) in the same CAS
                        // see account CA2740441 in ...\\wthmclbill\shared\Billing\LIVE\Posting835Remit\MedicareRemit\Saved\090506my.835 

                        case "PR":  // don't add to contractual
                        {
                            decimal dTempPRAmt = 0.00M;// decimal.Parse(strSVCElements[3]);
                            m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.ePaid); // don't post 07/01/2008 rgc/wdk Have not seen a PR yet 
                            m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.eCharges);
                            m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.eContractualAdjAmt);
                            m_strarrRecordsInsert.SetValue(dTempPRAmt.ToString(), (int)Col835Grids.eOtherAdjAmt); //05/27/2008 eOtherAdjAmt is new 
                            if (strSVCElements.GetUpperBound(0) == 3)
                            {
                                // rgc/wdk 20090528 PR 2's may have an additional PR 1 (Deductible) or PR 3(CoPayment) in the same CAS
                                // see account CA2740441 in ...\\wthmclbill\shared\Billing\LIVE\Posting835Remit\MedicareRemit\Saved\090506my.835 
                                m_strarrRecordsInsert.SetValue(strSVCElements[1] + "/" + strSVCElements[2], (int)Col835Grids.eReason);
                                dTempPRAmt = decimal.Parse(strSVCElements[3]);
                                m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.ePaid); // don't post 07/01/2008 rgc/wdk Have not seen a PR yet 
                                m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.eCharges);
                                m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.eContractualAdjAmt);
                                m_strarrRecordsInsert.SetValue(dTempPRAmt.ToString(), (int)Col835Grids.eOtherAdjAmt); //05/27/2008 eOtherAdjAmt is new 


                                switch (strSVCElements[2])
                                {
                                    case "1": // patient liability coinsurance added 05/01/2008 wdk
                                    {
                                        _dEOBPatLibCoInsurance += dTempPRAmt;
                                        break;
                                    }
                                    case "2":  // patient liability coinsurance 05/21/2008 rgc/wdk
                                    {
                                        // Because the pat's responsibility is both in the allowed added to the CO's amount
                                        // and the adjust amount add it to both for PR/2's. If required to be added together
                                        // put that code in R_Eob printPageEvent Handler in MCL to add the two together.
                                        _dEOBPatLibCoInsurance += dTempPRAmt;
                                        _dtRecordWriteOff.Rows.Add(m_strarrRecordsInsert);
                                        break;
                                    }
                                    case "26": // 07/10/2008 wdk does not have medicare
                                    {
                                        _dEOBPatLibNCovdCharges += dTempPRAmt;
                                        break;
                                    }
                                    case "31":    // Denied Not identified as the payor's client
                                    {
                                        _dEOBChargesDenied += dTempPRAmt;
                                        break;
                                    }
                                    case "45": // patient libiality noncovered charges put on eob non covered patient responsibility 
                                    {
                                        _dEOBPatLibNCovdCharges += dTempPRAmt;
                                        break;
                                    }

                                    case "96": // 06/09/2008 wdk Patient non covered per Darlene see file 1749 05/23/2008 account D608890
                                    {
                                        // wdk 20130129 add to m_dEOBPatLibNCovdCharges also per Carol
                                        _dEOBPatLibNCovdCharges += dTempPRAmt;
                                        _dEOBChargesNCovd += dTempPRAmt;
                                        break;
                                    }
                                    case "119": // 07/10/2008 wdk Patient non covered per Darlene see account D609295 in 080627MF.835
                                    {
                                        _dEOBPatLibNCovdCharges += dTempPRAmt;
                                        break;
                                    }

                                    case "B9": // 06/09/2008 wdk Denied per Darlene see file #1717 account CA2466273 
                                    {
                                        _dEOBChargesDenied += dTempPRAmt; // Hospise???
                                        break;
                                    }
                                    case "A7":
                                    {
                                        _dEOBPatLibNCovdCharges += dTempPRAmt;
                                        break;
                                    }
                                    default:
                                    {
                                        _dEOBPatLibNCovdCharges += dTempPRAmt;
                                        break;
                                    }
                                }
                                //  AddRecordToDataGrid();
                            }
                            if (strSVCElements.GetUpperBound(0) == 6)
                            {
                                // rgc/wdk 20090528 PR 2's may have an additional PR 1 (Deductible) or PR 3(CoPayment) in the same CAS
                                // see account CA2740441 in ..\\wthmclbill\shared\Billing\LIVE\Posting835Remit\MedicareRemit\Saved\090506my.835 
                                m_strarrRecordsInsert.SetValue(strSVCElements[1] + "/" + strSVCElements[5], (int)Col835Grids.eReason);
                                dTempPRAmt = decimal.Parse(strSVCElements[6]);
                                m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.ePaid); // don't post 07/01/2008 rgc/wdk Have not seen a PR yet 
                                m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.eCharges);
                                m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.eContractualAdjAmt);
                                m_strarrRecordsInsert.SetValue(dTempPRAmt.ToString(), (int)Col835Grids.eOtherAdjAmt); //05/27/2008 eOtherAdjAmt is new 


                                switch (strSVCElements[5])
                                {
                                    case "1": // patient liability coinsurance added 05/01/2008 wdk
                                    {
                                        _dEOBPatLibCoInsurance += dTempPRAmt;
                                        break;
                                    }
                                    case "2":  // patient liability coinsurance 05/21/2008 rgc/wdk
                                    {
                                        // Because the pat's responsibility is both in the allowed added to the CO's amount
                                        // and the adjust amount add it to both for PR/2's. If required to be added together
                                        // put that code in R_Eob printPageEvent Handler in MCL to add the two together.
                                        _dEOBPatLibCoInsurance += dTempPRAmt;
                                        _dtRecordWriteOff.Rows.Add(m_strarrRecordsInsert);
                                        break;
                                    }
                                    case "26": // 07/10/2008 wdk does not have medicare
                                    {
                                        _dEOBPatLibNCovdCharges += dTempPRAmt;
                                        break;
                                    }
                                    case "31":    // Denied Not identified as the payor's client
                                    {
                                        _dEOBChargesDenied += dTempPRAmt;
                                        break;
                                    }
                                    case "45": // patient libiality noncovered charges put on eob non covered patient responsibility 
                                    {
                                        _dEOBPatLibNCovdCharges += dTempPRAmt;
                                        break;
                                    }

                                    case "96": // 06/09/2008 wdk Patient non covered per Darlene see file 1749 05/23/2008 account D608890
                                    {
                                        // wdk 20130129 add to m_dEOBPatLibNCovdCharges also per Carol
                                        _dEOBPatLibNCovdCharges += dTempPRAmt;
                                        _dEOBChargesNCovd += dTempPRAmt;
                                        break;
                                    }
                                    case "119": // 07/10/2008 wdk Patient non covered per Darlene see account D609295 in 080627MF.835
                                    {
                                        _dEOBPatLibNCovdCharges += dTempPRAmt;
                                        break;
                                    }

                                    case "B9": // 06/09/2008 wdk Denied per Darlene see file #1717 account CA2466273 
                                    {
                                        _dEOBChargesDenied += dTempPRAmt; // Hospise???
                                        break;
                                    }
                                    default:
                                    {
                                        _dEOBPatLibNCovdCharges += dTempPRAmt;
                                        break;
                                    }
                                }
                                AddRecordToDataGrid();
                            }

                            m_strarrRecordsInsert.SetValue(strSVCElements[1] + "/" + strSVCElements[2], (int)Col835Grids.eReason);
                            dTempPRAmt = decimal.Parse(strSVCElements[3]);
                            m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.ePaid); // don't post 07/01/2008 rgc/wdk Have not seen a PR yet 
                            m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.eCharges);
                            m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.eContractualAdjAmt);
                            m_strarrRecordsInsert.SetValue(dTempPRAmt.ToString(), (int)Col835Grids.eOtherAdjAmt); //05/27/2008 eOtherAdjAmt is new 

                            switch (strSVCElements[2])
                            {
                                case "0": // wdk 20130129 patient libiality noncovered charges put on eob non covered patient responsibility 
                                {
                                    _dEOBPatLibNCovdCharges += dTempPRAmt;
                                    break;
                                }
                                case "1": // patient liability coinsurance added 05/01/2008 wdk
                                {
                                    _dEOBPatLibCoInsurance += dTempPRAmt;
                                    break;
                                }
                                case "2":  // patient liability coinsurance 05/21/2008 rgc/wdk
                                {
                                    // Because the pat's responsibility is both in the allowed added to the CO's amount
                                    // and the adjust amount add it to both for PR/2's. If required to be added together
                                    // put that code in R_Eob printPageEvent Handler in MCL to add the two together.
                                    //   m_dEOBPatLibCoInsurance += dTempPRAmt; wdk 20131114 removed as it is doubling the EOB Amount
                                    break;
                                }
                                case "26": // 07/10/2008 wdk does not have medicare
                                {
                                    _dEOBPatLibNCovdCharges += dTempPRAmt;
                                    break;
                                }
                                case "31":    // Denied Not identified as the payor's client
                                {
                                    _dEOBChargesDenied += dTempPRAmt;
                                    break;
                                }
                                case "45": // patient libiality noncovered charges put on eob non covered patient responsibility 
                                {
                                    _dEOBPatLibNCovdCharges += dTempPRAmt;
                                    break;
                                }
                                case "96": // 06/09/2008 wdk Patient non covered per Darlene see file 1749 05/23/2008 account D608890
                                {
                                    //m_dEOBChargesNCovd += dTempPRAmt; // wdk 20140528 removed per Carols request
                                    _dEOBPatLibNCovdCharges += dTempPRAmt; // wdk 20140528 added per Carols request
                                    break;
                                }
                                case "119": // 07/10/2008 wdk Patient non covered per Darlene see account D609295 in 080627MF.835
                                {
                                    _dEOBPatLibNCovdCharges += dTempPRAmt;
                                    break;
                                }

                                case "B9": // 06/09/2008 wdk Denied per Darlene see file #1717 account CA2466273 
                                {
                                    _dEOBChargesDenied += dTempPRAmt; // Hospise???
                                    break;
                                }
                                default:
                                {
                                    _dEOBPatLibNCovdCharges += dTempPRAmt;
                                    break;
                                }
                            }

                            break;
                        }
                        #endregion PR

                        #region PC Patient Credit
                        case "PC": // may be patient credit
                        {
                            // payment data: pat Refund
                            _dEOBPayDataPatRefund += dTempAmtPaid;
                            break;
                        }
                        #endregion PC

                        #region PI Payor Initiated Reductions
                        case "PI":
                        {
                            _dPayorInitiatedReductions += decimal.Parse(strSVCElements[3]);
                            /////

                            decimal dTempPIAmt = decimal.Parse(strSVCElements[3]);
                            dTempAmtPaid += dTempPIAmt;
                            m_strarrRecordsInsert.SetValue(dTempAmtPaid.ToString(), (int)Col835Grids.ePaid); // don't post 07/01/2008 rgc/wdk Have not seen a PR yet 
                                                                                                             //m_strarrRecordsInsert.SetValue("0.00", (int)col835Grids.eCharges);
                                                                                                             //m_strarrRecordsInsert.SetValue("0.00", (int)col835Grids.eContractualAdjAmt);
                            m_strarrRecordsInsert.SetValue(dTempPIAmt.ToString(), (int)Col835Grids.eOtherAdjAmt); //05/27/2008 eOtherAdjAmt is new 

                            break;
                        }
                        #endregion PI

                    }

                    // add the CAS to the DataSet/DataGrid
                    AddRecordToDataGrid();
                    break;
                }
                #endregion CAS


                #region AMT
                case "AMT":
                {
                    if (strSVCElements[1] == "NE") // from a TLC file
                    {
                        _dEOBChargesDenied += Decimal.Parse(strSVCElements[2]);
                    }
                    break;
                }
                #endregion AMT

            } // end of switch (strSVCElements[0])

        } // end of foreach (string strSVCLine in strPayLine)


    }

    private void ProcessBlueCrossSVC(string strSVC)
    {
        DateTime dtService = DateTime.Today;

        Decimal dTempAmtPaid = 0.00m;
        Decimal dTempAmtContractual = 0.00m;
        Decimal dTempSVCCharges = 0.00m;
        #region CLEAR m_strarrRecordsInsert
        ClearRecordInsertServiceLine();


        #endregion CLEAR m_strarrRecordsInsert

        string[] strPayLine = strSVC.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);
        ArrayList alPayLine = new ArrayList(strSVC.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries));
        string strValue = QueryArrayList(alPayLine, "HC>");
        if (!string.IsNullOrEmpty(strValue))
        {
            ClearRecordInsertServiceLine();

            string[] strSVCElements = strValue.Split(new char[] { '*' });
            // Processing a new SVC line so fill out what we know
            // Service Code
            m_strarrRecordsInsert.SetValue(strSVCElements[0], (int)Col835Grids.eCPT4Code);
            // Charges Reported
            dTempSVCCharges = decimal.Parse(strSVCElements[1]);
            m_strarrRecordsInsert.SetValue(strSVCElements[1], (int)Col835Grids.eCharges);
            // Allowed and paid are the same
            m_strarrRecordsInsert.SetValue(strSVCElements[2], (int)Col835Grids.eAllowed);
            m_strarrRecordsInsert.SetValue(strSVCElements[2], (int)Col835Grids.ePaid);
            dTempAmtPaid = decimal.Parse(strSVCElements[2]); // set this variable into dgvEOB(colEOB.ePayDataHCPCSAmt) after all the svc's  are processed
            _dEOBPayDataHCPCSAmt += dTempAmtPaid;
            // Rev Code
            m_strarrRecordsInsert.SetValue(strSVCElements[3], (int)Col835Grids.eRevCode);
            // Units
            try
            {
                m_strarrRecordsInsert.SetValue(strSVCElements[4], (int)Col835Grids.eUnits);
            }
            catch (IndexOutOfRangeException)
            {
                // zero listed
                m_strarrRecordsInsert.SetValue("0", (int)Col835Grids.eUnits);
            }

        }
        strValue = QueryArrayList(alPayLine, "DTM");
        if (!string.IsNullOrEmpty(strValue))
        {
            #region DTM
            string[] strSVCElements = strValue.Split(new char[] { '*' });
            if (strSVCElements[1].Contains("472"))
            {
                dtService = new DateTime();
                Utilities.Time.StringToHL7Time(strSVCElements[2], out dtService);
                m_strarrRecordsInsert.SetValue(dtService.ToString("d"), (int)Col835Grids.eDateOfService);
            }
            #endregion DTM
        }

        var queryCAS = from string strCAS in alPayLine
                       where strCAS.Substring(0, 7) == "CAS*CO*"
                       select strCAS;

        foreach (string strCAS in queryCAS)
        {
            ArrayList alSVCE = new ArrayList(strCAS.Split(new char[] { '*' }));
            string[] strSVCElements = strCAS.Split(new char[] { '*' });


            #region CAS*CO

            dTempAmtContractual = decimal.Parse(strSVCElements[3]);
            if (strSVCElements.GetUpperBound(0) == 6)
            {
                dTempAmtContractual += decimal.Parse(strSVCElements[6]);
            }
            if (strSVCElements.GetUpperBound(0) == 9)
            {
                dTempAmtContractual += decimal.Parse(strSVCElements[9]);
            }
            if (strSVCElements.GetUpperBound(0) == 12)
            {
                dTempAmtContractual += decimal.Parse(strSVCElements[12]);
            }
            //Claim adjustment group / Adjustment Reason Code
            m_strarrRecordsInsert.SetValue(strSVCElements[1] + "/" + strSVCElements[2], (int)Col835Grids.eReason);

            // SET SVC elements into the record to be inserted into the recordset and grid. May have been cleared if an OA has been processed before this CO
            m_strarrRecordsInsert.SetValue(dTempSVCCharges.ToString(), (int)Col835Grids.eCharges);
            m_strarrRecordsInsert.SetValue(dTempAmtPaid.ToString(), (int)Col835Grids.ePaid);
            m_strarrRecordsInsert.SetValue(dTempAmtPaid.ToString(), (int)Col835Grids.eAllowed);
            m_strarrRecordsInsert.SetValue("0.00", (int)Col835Grids.eOtherAdjAmt);


            // add the CAS to the DataSet/DataGrid
            AddRecordToDataGrid();
            //    break;

            #endregion CAS
        }

        foreach (string strSVCLine in strPayLine)
        {
            Application.DoEvents();
            string[] strSVCElements = strSVCLine.Split(new char[] { '*' });

            switch (strSVCElements[0].Substring(0, strSVCElements[0].Length <= 2 ? strSVCElements[0].Length : 3))
            {
                #region AMT
                case "AMT":
                {
                    if (strSVCElements[1] == "NE") // from a TLC file
                    {
                        _dEOBChargesDenied += Decimal.Parse(strSVCElements[2]);
                    }
                    break;
                }
                #endregion AMT
            } // end of switch (strSVCElements[0])

        } // end of foreach (string strSVCLine in strPayLine)
    }

    private void ClearRecordInsertServiceLine()
    {
        // Clear all the values for the records insert so each SVC contains unique data except the 
        // account and subscribers name, and status. 
        for (int j = ((int)Col835Grids.eClaimStatus + 1); j <= m_strarrRecordsInsert.GetUpperBound(0); j++)
        {
            m_strarrRecordsInsert.SetValue((string)"", j);

            if ((int)Col835Grids.eAllowed == j ||
                    (int)Col835Grids.eCharges == j ||
                        (int)Col835Grids.eContractualAdjAmt == j ||
                            (int)Col835Grids.ePaid == j ||
                                (int)Col835Grids.eOtherAdjAmt == j ||
                                    (int)Col835Grids.eWriteOffAmt == j)
            {
                m_strarrRecordsInsert.SetValue((string)"0.00", j);
            }
        }
    }

    private void InitializeEOBVariables()
    {
        // EOB's Charges
        _dEOBChargesNCovd = 0.00m; // CAS C0/96's totals for the EOB header grid
        _dEOBChargesDenied = 0.00m; // CLP 04 without the CLP* (not positive needs verification)

        // EOB's Patient Liability
        _dEOBPatLibNCovdCharges = 0.00m; // CAS PR/45 totals 
        _dEOBPatLibCoInsurance = 0.00m; // have not seen yet

        // EOB's Payment Data
        _dEOBPayDataMSPPrimPay = 0.00m; // other insurance 
        _dEOBPayDataHCPCSAmt = 0.00m; // try to calculate from the processed tab's paid amount field
        _dEOBPayDataContAdjAmt = 0.00m; // total of the co/45 from the CAS
        _dEOBPayDataPatRefund = 0.00m; // Have not seen yet should be a PC in the CAS
    }

    /// <summary>
    /// Adds records to the Processed, Denied, and Not Processed Grids based on the inserted records status in 
    /// m_strarrRecordsInsert's column (int)col835Grids.eClaimStatus, and the amount paid value in
    /// m_strarrRecordsInsert's column (int)col835Grids.ePaid
    /// </summary>
    private void AddRecordToDataGrid()
    {
        double dWriteOff = 0.00f;
        foreach (DataRow dr in _dtRecordWriteOff.Rows)
        {
            Application.DoEvents();
            double dParse = 0.00f;
            if (double.TryParse(dr[(int)Col835Grids.eWriteOffAmt].ToString(), out dParse))
            {
                dWriteOff += double.Parse(dParse.ToString("F2"));
            }
        }

        int nStatus = -1;
        int.TryParse(m_strarrRecordsInsert.GetValue((int)Col835Grids.eClaimStatus).ToString(), out nStatus);
        // nStatus 
        // 1 = Process for check posting
        // 2 = Not Processed unless the amount paid is not 0.00 then processed
        // 3 = Non Processed
        // 4 = Denied 
        // 22 = if amount paid is not = 0 put on Processed otherwise Not Processed
        // 19 = Process for check posting
        // 23 = if amount paid is not = 0 put on Processed otherwise Not Processed 04/23/2008 we think
        // default goes to Not Processed
        switch (nStatus)
        {
            case 1: // Deduct amt
            case 20: // 05/14/2008 rgc/wdk Accepted for processing
            case 19: // Denied work related (not really denied ???)
            case 2:// 04/23/2008 wdk/rgc Coinsurance Amount from claim status codes.               
            case 22: // Adjusted may be covered by another payer
            {
                if (m_strarrRecordsInsert.GetValue((int)Col835Grids.eReason).ToString().StartsWith("PR"))
                {
                    _dsRecords.Tables[dgvNotProcessed.Name].Rows.Add(m_strarrRecordsInsert);
                    tc835.SelectedIndex = 2;
                    break;
                }
                try  // if the amount paid is not zero add it to the processed otherwise to not processed
                {
                    decimal dTryParse = 0.00m;
                    //decimal dTotalPaid = 0.00m;

                    if (decimal.TryParse(m_strarrRecordsInsert.GetValue((int)Col835Grids.ePaid).ToString(), out dTryParse))
                    {
                        if (dTryParse == 0.00m)
                        {
                            if (m_strarrRecordsInsert.GetValue((int)Col835Grids.eReason).ToString().Contains("CO/96") ||
                                //m_strarrRecordsInsert.GetValue((int)col835Grids.eReason).ToString().Contains("PR/2") ||
                                m_strarrRecordsInsert.GetValue((int)Col835Grids.eReason).ToString().Contains("CO/50"))
                            {
                                _dsRecords.Tables[dgvProcessed.Name].Rows.Add(m_strarrRecordsInsert);
                                tc835.SelectedIndex = 0;
                            }
                            else
                            {
                                _dsRecords.Tables[dgvNotProcessed.Name].Rows.Add(m_strarrRecordsInsert);
                                tc835.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            _dsRecords.Tables[dgvProcessed.Name].Rows.Add(m_strarrRecordsInsert);
                            tc835.SelectedIndex = 0;

                        }
                    }
                    else
                    {
                        _dsRecords.Tables[dgvNotProcessed.Name].Rows.Add(m_strarrRecordsInsert);
                        tc835.SelectedIndex = 2;
                    }
                    // WDK 20130725 added 
                    if (m_strarrRecordsInsertAddContractual != null)
                    {
                        if (m_strarrRecordsInsertAddContractual.GetValue((int)Col835Grids.Account) != null)
                        {
                            if (!string.IsNullOrEmpty(m_strarrRecordsInsertAddContractual.GetValue((int)Col835Grids.Account).ToString()))
                            {
                                _dsRecords.Tables[dgvProcessed.Name].Rows.Add(m_strarrRecordsInsertAddContractual);
                                m_strarrRecordsInsertAddContractual = new string[Enum.GetNames(typeof(Col835Grids)).Length];// wdk 20130725         
                                m_strarrRecordsInsertAddContractual.Initialize();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //MessageBox.Show(
                    //    string.Format("Exception while trying to parse the amount paid from the m_strarrInsertRecord\r\r\n{0}",ex.Message), propAppName);
                }

                break;
            }
            case 4: // denieds (Modifier)
            {
                _dsRecords.Tables[dgvDenieds.Name].Rows.Add(m_strarrRecordsInsert);
                _dtNotes.Rows.Add(new object[] { m_strarrRecordsInsert[(int)Col835Grids.Account],
                        m_strarrRecordsInsert[(int)Col835Grids.eCPT4Code],
                        m_strarrRecordsInsert[(int)Col835Grids.eClaimStatus],
                        m_strarrRecordsInsert[(int)Col835Grids.eReason] });
                tc835.SelectedIndex = 1;

                break;
            }
            default: // 23 payment adj impact of a prior payer
            {
                _dsRecords.Tables[dgvNotProcessed.Name].Rows.Add(m_strarrRecordsInsert);
                _dtNotes.Rows.Add(new object[] { m_strarrRecordsInsert[(int)Col835Grids.Account],
                        m_strarrRecordsInsert[(int)Col835Grids.eCPT4Code],
                        m_strarrRecordsInsert[(int)Col835Grids.eClaimStatus],
                        m_strarrRecordsInsert[(int)Col835Grids.eReason] });

                tc835.SelectedIndex = 2;
                break;
            }
        } // end of switch    
    }

    /// <summary>
    /// The strSVC is all the CLP data except the SVCs. This is the header information for the claim.
    /// Line 1 contains the Account, status, reported charges, paid amount, ICN and type
    /// Line 2 contains an NM1*QC with the patients Name, and subscriber ID.
    /// Line 3 contains an NM1*82 with the Providers name and ID (wth tax id for medicare)
    /// Line 4 contains an NM1*TT with the forwarded Insurances name and provider id (Medicare)
    /// Line 5 contains an MOA with the Reimb rate and the 
    /// </summary>
    /// <param name="strSVC"></param>
    private void SetPatientData(string strSVC)
    {
        // the zero element is the pat demo 
        string[] strSVCHeader = strSVC.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries); // gives us CLP line without the CLP*, NM1's, REF, DTM's 
        // Split the CLP for account, Charge Amt, Amt Paid
        string[] strSVCHeaderElements = strSVCHeader[0].Split(new char[] { '*' });
        // Account number or PCN (Medicare) don't remove "A" from Account for Medicare EOB but do remove for check posting.
        m_strarrEOBInsert.SetValue(strSVCHeaderElements[0], (int)Col835EOB.Account); // account number
        m_strarrRecordsInsert.SetValue(strSVCHeaderElements[0].Replace("A", "").ToString(), (int)Col835Grids.Account); // account number
                                                                                                                       //WDK 20130816 added clear this for each new patient.
                                                                                                                       //m_strarrRecordsWriteOff =  new string[Enum.GetNames(typeof(col835Grids)).Length];// wdk 20130725         
                                                                                                                       //m_dgvRecordsWriteOff = new DataGridView();
        _dtRecordWriteOff.Clear();
        //m_dgvRecordsWriteOff[m_dgvRecordsWriteOff.GetUpperBound(0)].CreateCells( = ((DataGridViewRow)m_strarrRecordsInsert);
        // Claim status
        m_strarrEOBInsert.SetValue(strSVCHeaderElements[1].ToString(), (int)Col835EOB.eClaimSt);
        m_strarrRecordsInsert.SetValue(strSVCHeaderElements[1].ToString(), (int)Col835Grids.eClaimStatus); // 04/15/2008 rgc/wdk set this to know which tab to put the inserted record on.
                                                                                                           // WDK 20130725 added 
                                                                                                           // m_strarrRecordsInsertAddContractual.SetValue(strSVCHeaderElements[1].ToString(), (int)col835Grids.eClaimStatus);
                                                                                                           // Charges reported
        m_strarrEOBInsert.SetValue(string.Format("{0:C}",
                    strSVCHeaderElements[2]), (int)Col835EOB.eChargesReported);

        // Amount reimbursed (Paid Amt)
        m_strarrEOBInsert.SetValue(
                string.Format("{0:C}", strSVCHeaderElements[3]), (int)Col835EOB.ePaymentDataNetReimbAmt);
        // Charges Denied
        m_strarrEOBInsert.SetValue(strSVCHeaderElements[4].ToString(), (int)Col835EOB.eChargesDenied);
        // ICN 
        m_strarrEOBInsert.SetValue(strSVCHeaderElements[6].ToString(), (int)Col835EOB.eICN);

        try
        {
            // type 
            m_strarrEOBInsert.SetValue((strSVCHeaderElements[7] + strSVCHeaderElements[8]), (int)Col835EOB.eType);
        }
        catch (IndexOutOfRangeException)
        {
            // tlc does not have the 141 in the 7th and 8th positions.
            m_strarrEOBInsert.SetValue("", (int)Col835EOB.eType);
        }

        // start on i = 1 because we split the strSVCHeader[0] above this loop
        for (int i = 1; i <= strSVCHeader.GetUpperBound(0); i++)
        {
            strSVCHeaderElements = strSVCHeader[i].Split(new char[] { '*' });
            // Check the NM1's
            if (strSVCHeaderElements[0].ToString().IndexOf("NM1", 0, 3) > -1)
            {
                // find the "NM1*QC" for patient name and eob's HIC (HealthCare Insurance Code???)
                if (strSVCHeaderElements[1].ToString() == "QC") // patient name
                {
                    if (strSVCHeaderElements.GetUpperBound(0) > 4)
                    {
                        m_strarrEOBInsert.SetValue(strSVCHeaderElements[3] + "," + strSVCHeaderElements[4] + " " + strSVCHeaderElements[5], (int)Col835EOB.eSubscriberName);
                        m_strarrRecordsInsert.SetValue(strSVCHeaderElements[3] + "," + strSVCHeaderElements[4] + " " + strSVCHeaderElements[5], (int)Col835Grids.eSubscriberName);
                        // WDK 20130725 added 
                        //   m_strarrRecordsInsertAddContractual.SetValue(strSVCHeaderElements[3] + "," + strSVCHeaderElements[4] + " " + strSVCHeaderElements[5], (int)col835Grids.eSubscriberName);
                        try
                        {
                            m_strarrEOBInsert.SetValue(strSVCHeaderElements[9], (int)Col835EOB.eHIC);
                            m_strarrRecordsInsert.SetValue(strSVCHeaderElements[9], (int)Col835Grids.eSubscriberID);
                            // WDK 20130725 added 
                            //     m_strarrRecordsInsertAddContractual.SetValue(strSVCHeaderElements[9], (int)col835Grids.eSubscriberID);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            m_strarrEOBInsert.SetValue("MISSING", (int)Col835EOB.eHIC);
                            m_strarrRecordsInsert.SetValue("MISSING", (int)Col835Grids.eSubscriberID);
                            // WDK 20130725 added 
                            //     m_strarrRecordsInsertAddContractual.SetValue("MISSING", (int)col835Grids.eSubscriberID);
                        }

                    }
                    continue;
                }
                // find NM1*IL Other subscriber name. will be pay info from other insurance ie bluecross
                if (strSVCHeaderElements[1].ToString() == "IL")
                {
                    m_strarrEOBInsert.SetValue(strSVCHeaderElements[3], (int)Col835EOB.eClaimForwarded);
                    m_strarrEOBInsert.SetValue(strSVCHeaderElements[9], (int)Col835EOB.eClaimForwardedID);
                    continue;
                }

                // find the "NM1*TT" for the  Claim Forwared text and ID for EOB
                if (strSVCHeaderElements[1].ToString() == "TT")
                {
                    m_strarrEOBInsert.SetValue(strSVCHeaderElements[3], (int)Col835EOB.eClaimForwarded);
                    m_strarrEOBInsert.SetValue(strSVCHeaderElements[9], (int)Col835EOB.eClaimForwardedID);
                    continue;
                }
            }

            if (strSVCHeaderElements[0].ToString().IndexOf("MOA", 0, 3) > -1)
            {
                m_strarrEOBInsert.SetValue(strSVCHeaderElements[1], (int)Col835EOB.ePaymentDataReimbRate);
                continue;
            }
            if (strSVCHeaderElements[0].ToString() == "AMT" && strSVCHeaderElements[1].ToString() == "DY")
            {
                m_strarrEOBInsert.SetValue(strSVCHeaderElements[2], (int)Col835EOB.ePaymentDataPerDiemRate);
                continue;
            }
            // For both TLC and Medicare (as of 04/02/2008) we should find a DTM with a 232 segment.
            // continue to look until we find a DTM segment with a 232 (beginning service date) code. 
            #region Medicare note
            /*Note: Medicare sends both a 232 and a 233 (which is the ending service date) */
            #endregion Medicare note
            if (strSVCHeaderElements[0].ToString() == "DTM" && strSVCHeaderElements[1] == "232")
            {
                DateTime dtService = new DateTime();
                // Split the DTM for the date of service only
                Utilities.Time.StringToHL7Time(strSVCHeaderElements[2], out dtService);
                m_strarrEOBInsert.SetValue(dtService.ToString("d"), (int)Col835EOB.eDOS);
                continue;
            }
        }
    }

    /// <summary>
    /// Clears the totals in the table lay out panel, the datagridviews, and the dataset
    /// </summary>
    private void ClearForm()
    {
        _dsRecords.Clear(); // removes all rows in all tables.

        // clear the grids
        dgvProcessed.Rows.Clear();
        dgvDenieds.Rows.Clear();
        dgvNotProcessed.Rows.Clear();
        dgvEOB.Rows.Clear();

        // clear the label for the file being parsed
        tbFileName.Text = "File: ";
        tbFileName.Tag = "";
        tbProviderID.Text = "Provider ID: ";
        tbProviderID.Tag = "";
        tbCheckDate.Text = "Chk Date: ";
        tbCheckDate.Tag = "";
        tbCheckNo.Text = "Chk No: ";
        tbCheckNo.Tag = "";
        tbCheckAmt.Text = "Chk Amt: ";
        tbCheckAmt.Tag = "";
        tbBatchNo.Text = "Batch No:";
        tbBatchNo.Tag = "";
        lbChecks.Items.Clear();
        tsmiPostCheckRecords.Enabled = true;
        _dicSTAmts.Clear();
    }


    /// <summary>
    /// Sets the amounts into the table layout panel formatted into currency.
    /// </summary>
    private void SetTableLayOutTotals()
    {
        tbEOBChargeAmt.Text = _d_dgvEOB_Totals[(int)Col835EOB.eChargesReported].ToString("c");
        tbEOBPaidAmt.Text = _d_dgvEOB_Totals[(int)Col835EOB.ePaymentDataNetReimbAmt].ToString("c");
        tbEOBContractualAmt.Text = _d_dgvEOB_Totals[(int)Col835EOB.ePaymentDataContAdjAmt].ToString("c");
        tbEOBDeniedAmt.Text = _d_dgvEOB_Totals[(int)Col835EOB.eChargesDenied].ToString("c");
        tbEOBOtherAmt.Text = _d_dgvEOB_Totals[(int)Col835EOB.eOtherAdjAmt].ToString("c");
    }


    private void tsmiPrint_Click(object sender, EventArgs e)
    {
        DataGridView l_dgvCurrent = GetSelectedTabsDataGrid(tc835.SelectedIndex);
        _myPrintDocument.DefaultPageSettings.Landscape = true;
        _myPrintDocument.DocumentName = tc835.SelectedTab.Text;
        _myPrintDocument.PrintController = null;
        DataSetPrinter dp = new DataSetPrinter(_dsRecords, l_dgvCurrent.Name, _myPrintDocument, tc835.SelectedTab.Text.ToUpper(), ref _eRR);
        dp.propFooterText = string.Format("Check No: {0}, Dated: {1}, Total: {2}, from File: {3}",
                                    tbCheckNo.Tag.ToString(), tbCheckDate.Tag.ToString(), tbCheckAmt.Tag.ToString(), tbFileName.Text);
        this._myPrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(dp.MyPrintDocument_PrintPage);
        _myPrintDocument.Print();
        this._myPrintDocument.PrintPage -= new System.Drawing.Printing.PrintPageEventHandler(dp.MyPrintDocument_PrintPage);

    }


    /// <summary>
    /// Modifications:
    /// 06/13/2008 wdk (Friday the 13th) added write off processing if Payor is TLC and CPT4 code is 36415 
    ///     write off amount will always be $3.07 with a code of 1000 and the contractual will be $5.93. 
    ///     (Requested by Carol on 06/12/2008)
    /// 
    /// Posts the check records from the processed datagridview (dgvProcessed only)
    /// Checks the accounts balance before the insert to determine if there are charges. If there are charges
    /// a dictionary entry is placed in m_dicBalances. 
    /// 
    /// 12/28/2007 wdk
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void tsmiPostCheckRecords_Click(object sender, EventArgs e)
    {
        //MoveFileToSavedDirectory();
        //return;
        tsmiPostCheckRecords.Enabled = false;
        // when posting medicare checks make sure the source is "MEDICARE" and the comment is "SSI REMITTANCE"
        int nRecordsAdded = 0;
        int nErrorRecords = 0;
        // holder for accounts to check the balances from 
        _dicBalances = new Dictionary<string, decimal>();
        try
        {
            // if this had of used the app.config values.
            _strAccountLogPath = $"{Program.AppEnvironment.ApplicationParameters.DefaultSpoolFilePath}AccBalances{tbFileName.Text.Substring(tbFileName.Text.LastIndexOf('\\') + 1)}.txt";
            _swAccount = new StreamWriter(_strAccountLogPath);
        }
        catch (IOException ioe)
        {
            _eRR.m_Email.Send("PostCheckRecords@Viewer835.ioe", "bradley.powers@wth.org", "FILE ERROR", ioe.Message);
        }
        catch (NullReferenceException nre)
        {
            //Checks have been posted once and the stream has not been reopened. 
            _eRR.m_Email.Send("Viewer835.tsmiPostCheckRecords_click", "bradley.powers@wth.org", "FILE ERROR", nre.Message);
        }
        catch (Exception exc)
        {
            MessageBox.Show(exc.Message);
        }

        // set the check received and record modification dates, rowguid, 
        //and batch number variables to be inserted into this record.
        DateTime sdtReceived = DateTime.Now;
        DateTime sdtMod = DateTime.Parse(DateTime.Now.ToString());
        string strBatchNo = _batchTransactionService.GetNextBatchNumber().ToString();

        tbBatchNo.Text = $"Batch No: {strBatchNo}";
        this.Invalidate();

        // create the loop through the records to add
        tspbRecords.Value = 0;
        tspbRecords.Minimum = 0;
        tspbRecords.Maximum = dgvProcessed.Rows.Count;
        //tspbRecords.Step = 1;

        // put the batch number, check number and date at the top of the log
        _eRR.m_Logfile.WriteLogFile($"Check number {tbCheckNo.Tag} was posted on batch number {strBatchNo} on {DateTime.Now:G}");

        // now process the checks.
        List<Chk> chks = new();

        foreach (DataRow dr in _dsRecords.Tables[dgvProcessed.Name].Rows)
        {
            Application.DoEvents();
            tspbRecords.PerformStep();
            Application.DoEvents();


            #region RecordsToSkip  Handle new and/or totals rows. Don't post zero balance check records

            //provide a list of account balances for those accounts that have checks to be posted.
            // don't get balances because the checks are not committed and this pulls the check posting to a standstill
            // just put the account in the dictionary and get the balance after the commit.
            // the try catch will give distinct account numbers for later querying
            try
            {
                _dicBalances.Add(dr[(int)Col835Grids.Account].ToString().ToUpper().Replace("A", ""), decimal.Parse("0.00"));
            }
            catch (ArgumentException) { } // key already exists so keep on trucking

            // IF the total of the writeoff, amt_paid and contractual 
            //is equal to zero don't add the check to the table 
            //(Equal ONLY, less than or greater than go ahead and post) 
            decimal dTotal = 0m;
            decimal dAmt = 0m;
            bool bHasAmt = false;

            dTotal +=
            dAmt = decimal.Parse(dr[Col835Grids.ePaid.ToString()].ToString(), System.Globalization.NumberStyles.Currency);

            if (dAmt != 0m)
            {
                bHasAmt = true;
            }
            // contractual
            dTotal +=
            dAmt = decimal.Parse(dr[Col835Grids.eContractualAdjAmt.ToString()].ToString(), System.Globalization.NumberStyles.Currency);
            if (dAmt != 0m)
            {
                bHasAmt = true;
            }
            if (!string.IsNullOrEmpty(dr[(int)Col835Grids.eWriteOffAmt].ToString()))
            {
                dTotal +=
                dAmt = decimal.Parse(dr[Col835Grids.eWriteOffAmt.ToString()].ToString(), System.Globalization.NumberStyles.Currency);
                if (dAmt != 0m)
                {
                    bHasAmt = true;
                }
            }
            if (dTotal == 0 && !bHasAmt)
            {
                _eRR.m_Logfile.WriteLogFile(string.Format("---CHECK PAYMENT BALANCE EQUALS ZERO (0) - ACCOUNT {0}",
                        dr[(int)Col835Grids.Account].ToString().ToUpper()));
                nErrorRecords++;
                continue;
            }

            #endregion RecordsToSkip

            #region InsertRecord
            try
            {
                Chk chk = new()
                {
                    IsDeleted = false,
                    AccountNo = dr[(int)Col835Grids.Account].ToString().ToUpper(),
                    ChkDate = DateTime.Parse(tbCheckDate.Tag.ToString()),
                    DateReceived = sdtReceived,
                    CheckNo = dr[(int)Col835Grids.eCheckNo].ToString(),
                    PaidAmount = double.Parse(dr[(int)Col835Grids.ePaid].ToString(), System.Globalization.NumberStyles.Currency),
                    WriteOffAmount = string.IsNullOrEmpty(dr[(int)Col835Grids.eWriteOffAmt].ToString()) ? 0.00
                        : double.Parse(dr[(int)Col835Grids.eWriteOffAmt].ToString()),
                    ContractualAmount = double.Parse(dr[(int)Col835Grids.eContractualAdjAmt].ToString(), System.Globalization.NumberStyles.Currency),
                    Status = "NEW",
                    Source = rtbCheckSource.Text.Substring(0, (rtbCheckSource.Text.Length > 20 ? 20 : rtbCheckSource.Text.Length)),
                    Batch = int.Parse(strBatchNo),
                    Comment = rtbCheckSource.Text,
                    IsCollectionPmt = false,
                    UpdatedDate = sdtMod,
                    UpdatedUser = Environment.UserName,
                    UpdatedApp = $"{ProductName} {ProductVersion}",
                    UpdatedHost = Environment.MachineName,
                    mod_date_audit = sdtMod,
                    Cpt4Code = dr[(int)Col835Grids.eCPT4Code].ToString(),
                    PostingFile = tbFileName.Tag.ToString(),
                    WriteOffCode = string.IsNullOrEmpty(dr[(int)Col835Grids.eWriteOffCode].ToString()) ? string.Empty
                        : dr[(int)Col835Grids.eWriteOffCode].ToString(),
                    EftDate = DateTime.Parse(tbFileDate.Tag.ToString()),
                    EftNumber = tbFileNumber.Tag.ToString(),
                    FinCode = _strFinCode,
                    InsCode = _strInsCode,
                    ClaimAdjCode = dr[(int)Col835Grids.eReason].ToString()
                };

                if (DateTime.TryParse(dr[(int)Col835Grids.eWriteOffDate].ToString(), out DateTime woffDateTime))
                    chk.WriteOffDate = woffDateTime;

                chks.Add(chk);

                nRecordsAdded++;
            }
            catch (SqlException sqlError)
            {
                string errorMsg = $"{sqlError.Message} - {nRecordsAdded} transactions rolled back. Account {dr[(int)Col835Grids.Account].ToString().ToUpper()}";
                Log.Instance.Error(errorMsg, sqlError);
                MessageBox.Show(errorMsg, "CHECK POSTING TERMINATED", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                break;
            }
            catch (InvalidOperationException ioe)
            {
                string errorMsg = $"{ioe.Message} - {nRecordsAdded} transactions rolled back. Account {dr[(int)Col835Grids.Account].ToString().ToUpper()}";
                Log.Instance.Error(errorMsg, ioe);
                MessageBox.Show(errorMsg, "CHECK POSTING TERMINATED", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                break;
            }
            catch (NullReferenceException nre)
            {
                string errorMsg = $"{nre.Message} - {nRecordsAdded} transactions rolled back. Account {dr[(int)Col835Grids.Account].ToString().ToUpper()}";
                Log.Instance.Error(errorMsg, nre);
                MessageBox.Show(errorMsg, "CHECK POSTING TERMINATED", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                nErrorRecords++;
                continue;
            }
            #endregion InsertRecord
        }
        try
        {
            _batchTransactionService.PostBatchPayments(chks);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        MessageBox.Show(string.Format("{0} records added.", nRecordsAdded), "DATABASE UPDATE FINISHED");

        #region Get Account Balances and write them to the account balance file

        //wdk 20150507 if m_swAccount is null make the file in c:\Temp
        _swAccount.WriteLine("          ACCOUNT BALANCES for checks posted on {0}\r\n", DateTime.Now.ToString("D"));
        _swAccount.WriteLine($"{"ACCOUNT",-15}{"BALANCE",-15}{"STATUS\r\n"}\r\n");
        tspbRecords.Minimum = 0;
        tspbRecords.Maximum = _dicBalances.Count;
        foreach (string strAccount in _dicBalances.Keys)
        {

            Application.DoEvents();
            tspbRecords.PerformStep();

            _cAccStatus.GetBalance(strAccount, out string strBal);
            _cAccStatus.GetStatus(strAccount, out string strStatus);
            if (string.IsNullOrEmpty(strBal))
            {
                continue;
            }
            if (strBal == "0" || strBal == "ERR")
            {
                continue;
            }
            if (strBal.Contains('.'))
            {
                int nEnd = strBal.Length - strBal.IndexOf(".");
                if (nEnd > 3)
                {
                    nEnd = 3;
                }
                strBal = strBal.Substring(0, strBal.IndexOf('.') + nEnd);
            }
            double dBal = double.Parse(strBal);
            if (dBal != 0.00 && strStatus == AccountStatus.PaidOut)
            {
                _accountService.UpdateStatus(strAccount, AccountStatus.New);
            }
            _swAccount.WriteLine($"{strAccount.PadRight(16)}{dBal.ToString("C2", CultureInfo.CurrentCulture),-15}{strStatus.PadLeft(15 - strBal.Length)}");
        }

        _swAccount.Flush();


        #endregion Get Account Balances and write them to the account balance file

        #region Print Account Balance file
        try
        {
            if (!string.IsNullOrEmpty(_strAccountLogPath))
            {
                _swAccount.Close();
                _srAccount = new StreamReader(_strAccountLogPath);

                ThreadProcRFCPrintPreview(_srAccount, "ACCOUNT BALANCES");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "CREATE PRINT THREAD (ACCOUNT BALANCES) ERROR");
        }
        #endregion Print Account Balance file

        tspbRecords.Value = 0; // reset the progress bar
        tsmiPostCheckRecords.Enabled = false;
        MoveFileToSavedDirectory();
    }


    /// <summary>
    /// Moves the file listed in the File name label
    /// </summary>
    private void MoveFileToSavedDirectory()
    {
        string strFileName = tbFileName.Tag.ToString();
        string strMoveFile = $@"{_diSaved}{strFileName.Substring(strFileName.LastIndexOf('\\'))}";
        string strMoveName = $@"{_diSaved}\Saved_DUP\{strFileName.Substring(strFileName.LastIndexOf('\\'))}";
        try
        {
            if (File.Exists(string.Format(@"{0}", strMoveFile)))
            {

                File.Move(tbFileName.Tag.ToString(), $@"{_diSaved}\Saved_DUP\{strFileName.Substring(strFileName.LastIndexOf('\\'))}");
                return;
            }
            File.Move(tbFileName.Tag.ToString(), $@"{_diSaved}\{strFileName.Substring(strFileName.LastIndexOf('\\'))}");
        }
        catch (IOException ioe)
        {
            //  MessageBox.Show(ioe.Message, ioe.Source);
            _eRR.m_Logfile.WriteLogFile(ioe.Message);
        }
        catch (Exception ex)
        {
            //  MessageBox.Show("Could not move the File.", "MOVE FILE ERROR");
            _eRR.m_Logfile.WriteLogFile(ex.Message);
        }
    }


    /// <summary>
    /// Takes a printable Stream as an object and passes it to the propStreamToPrint in the 
    /// Viewer which converts the Object into the correct type and sets the string to print.
    /// 09/30/2008 wdk
    /// </summary>
    /// <param name="objToPrint">MemoryStream, FileStream, StreamReader, or string</param>
    /// <param name="strDocName">The title you want on the RFCPrintPreview Dialog</param>
    public void ThreadProcRFCPrintPreview(object objToPrint, string strDocName)
    {
        if (objToPrint == null)
        {
            return;
        }
        RFCPrintPreview ppd = new RFCPrintPreview();
        ppd.m_DocumentName = strDocName;
        ppd.propTopMost = true;
        ppd.propLandscape = false;
        ppd.Dock = DockStyle.Bottom;
        try
        {
            ppd.propStreamToPrint = objToPrint;
            ppd.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

    }

    /// <summary>
    /// Returns True if there are previously posted checks or there are errors.
    /// Returns false on successful query that returns no check records.
    /// </summary>
    public int ThereArePreviouslyPostedChecks(string strCheckNo)
    {
        string strFileName = tbFileName.Tag.ToString();
        int nRec = 0;

        using SqlConnection conn = new(Program.AppEnvironment.ConnectionString);

        SqlDataAdapter sda = new();
        string strSelect =
            string.Format("select account from chk where " +
                            " eft_date = '{0}' AND eft_number = '{1}' AND chk_no = '{2}'",
                            tbFileDate.Tag.ToString(),
                            tbFileNumber.Tag.ToString(),
                            strCheckNo);
        sda.SelectCommand = new SqlCommand(strSelect, conn);
        DataTable dtCheck = new();

        try
        {
            nRec = sda.Fill(dtCheck);
        }
        catch (SqlException se)
        {
            MessageBox.Show(se.Message, "SQL EXCEPTION DURING CHECK");
        }

        return nRec;
    }

    /// <summary>
    /// Return the datagridview on the tab page by it's tab page index
    /// </summary>
    /// <param name="nDexTab"></param>
    /// <returns></returns>
    public DataGridView GetSelectedTabsDataGrid(int nDexTab)
    {
        return ((DataGridView)tc835.TabPages[nDexTab].Controls[0]);
    }

    /// <summary>
    /// Prints the applications current view 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void tsbPrintView_Click(object sender, EventArgs e)
    {
        Bitmap[] bmps = dkPrint.Capture(dkPrint.CaptureType.Form);
        try
        {
            bmps[0].Save(@"C:\Temp\Posting835Remittance.bmp");
        }
        catch (System.Runtime.InteropServices.ExternalException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }
        try
        {
            dkPrint.propStreamToPrint = new StreamReader(@"C:\Temp\Posting835Remittance.bmp");
        }
        catch (Exception exc)
        {
            MessageBox.Show(exc.Message);
            Utilities.dkPrint.propStreamToPrint.Close();
        }
        PrintDocument printDoc = new PrintDocument();

        printDoc.DefaultPageSettings.Landscape = true;
        printDoc.PrintPage += new PrintPageEventHandler
            (Utilities.dkPrint.PrintGraphic_PrintPage);

        // printDoc.PrinterSettings.PrinterName = @"\\MCL01\MCLP5";
        printDoc.Print();

        printDoc.PrintPage -= new PrintPageEventHandler
            (Utilities.dkPrint.PrintGraphic_PrintPage);

        Utilities.dkPrint.propStreamToPrint.Close();

    }

    /// <summary>
    ///  Sets the record count on the status bar to the number in the selected grid control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void tcControl835_SelectedIndexChanged(object sender, EventArgs e)
    {
        int nTab = ((TabControl)sender).SelectedIndex;
        DataGridView dgv = GetSelectedTabsDataGrid(nTab);
        tstbFindSubscriber.AutoCompleteCustomSource.Clear();
        switch (nTab)
        {
            case 0:
            case 1:
            case 2:
            {
                foreach (DataRow dr in _dsRecords.Tables[dgv.Name].Rows)
                {
                    Application.DoEvents();
                    tstbFindSubscriber.AutoCompleteCustomSource.Add(dr[(int)Col835Grids.eSubscriberName].ToString());
                    tstbFindAccount.AutoCompleteCustomSource.Add(dr[(int)Col835Grids.Account].ToString());
                }
                break;
            }
            case 3:
            {
                foreach (DataRow dr in _dsRecords.Tables[dgv.Name].Rows)
                {
                    Application.DoEvents();
                    tstbFindSubscriber.AutoCompleteCustomSource.Add(dr[(int)Col835EOB.eSubscriberName].ToString());
                    tstbFindAccount.AutoCompleteCustomSource.Add(dr[(int)Col835EOB.Account].ToString());
                }
                break;
            }
        }
    }

    /// <summary>
    /// Create the eob and eob_details records using the EOB (master) and Processed, not processed and denied (details)tabs.
    /// Wrapped in a comittable transaction to ensure that all or none of the records get into the tables.
    /// If the records have already been posted no additional posting will occur.
    /// 05/19/2008 wdk
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void tsmiEOB_Click(object sender, EventArgs e)
    {
        _eRR.m_Logfile.WriteLogFile("PostEobFromDataSet");

        tspbRecords.Value = 0;
        tspbRecords.Minimum = 0;
        tspbRecords.Maximum = _dsRecords.Tables[dgvEOB.Name].Rows.Count;// .tdgvEOB.Rows.Count;
        tspbRecords.Step = 1;
        tspbRecords.ToolTipText = "Posting EOB's";

        this.Cursor = Cursors.WaitCursor;

        string strWhere = "";
        // wdk 20160419 have to recreate this here in order to process several files in a row.
        CEob m_rEob = new CEob(_strServer, _strDatabase, ref _eRR);
        m_rEob.CreateCommittableTransaction();

        // create the EOB record set it's rowguid then use the rowguid for the detail records
        //R_eob leob = new R_eob(m_strServer, m_strDatabase, ref m_ERR);
        //R_eob_detail leob_detail = new R_eob_detail(m_strServer, m_strDatabase, ref m_ERR);

        // create the loop through the records to add
        //foreach (DataGridViewRow dr in dgvEOB.Rows)

        foreach (DataRow dr in _dsRecords.Tables[dgvEOB.Name].Rows)
        {
            tspbRecords.PerformStep();
            Application.DoEvents();
            m_rEob.Reob.ClearMemberVariables();

            m_rEob.Reob.m_strAccount = dr[(int)Col835EOB.Account].ToString();

            m_rEob.Reob.m_strSubscriberID = dr[(int)Col835EOB.eHIC].ToString();
            m_rEob.Reob.m_strSubscriberName = RFCObject.staticSqlClean(dr[(int)Col835EOB.eSubscriberName].ToString()); //06/20/2008 wdk name [Carol'Kay] received
            m_rEob.Reob.m_strDateOfService = dr[(int)Col835EOB.eDOS].ToString();
            m_rEob.Reob.m_strICN = dr[(int)Col835EOB.eICN].ToString();
            m_rEob.Reob.m_strPatStat = dr[(int)Col835EOB.ePatStat].ToString();
            m_rEob.Reob.m_strClaimStatus = dr[(int)Col835EOB.eClaimSt].ToString();
            m_rEob.Reob.m_strClaimType = dr[(int)Col835EOB.eType].ToString();

            // charge section
            m_rEob.Reob.m_strChargesReported =
                    decimal.Parse(dr[(int)Col835EOB.eChargesReported].ToString(),
                     System.Globalization.NumberStyles.Currency).ToString();// string.Format("{0:F2}", dr.Cells[(int)col835EOB.eChargesReported].FormattedValue);
            m_rEob.Reob.m_strChargesDenied =
                    decimal.Parse(dr[(int)Col835EOB.eChargesDenied].ToString(),
                     System.Globalization.NumberStyles.Currency).ToString();
            m_rEob.Reob.m_strChargesNoncovered =
                    decimal.Parse(dr[(int)Col835EOB.eChargesNCovd].ToString(),
                     System.Globalization.NumberStyles.Currency).ToString();
            // patient libility section
            m_rEob.Reob.m_strPatLibCoinsurance =
                    decimal.Parse(dr[(int)Col835EOB.ePatientLibCoinsurance].ToString(),
                     System.Globalization.NumberStyles.Currency).ToString();
            m_rEob.Reob.m_strPatLibNoncovered =
                    decimal.Parse(dr[(int)Col835EOB.ePatientLibNCovdCharges].ToString(),
                     System.Globalization.NumberStyles.Currency).ToString();
            // Payment data section
            m_rEob.Reob.m_strPayDataReimbRate = dr[(int)Col835EOB.ePaymentDataReimbRate].ToString();
            m_rEob.Reob.m_strPayDataMSPPrimPay =
                    decimal.Parse(dr[(int)Col835EOB.ePaymentDataMSPPrimPay].ToString(),
                     System.Globalization.NumberStyles.Currency).ToString();
            m_rEob.Reob.m_strPayDataHcpcsAmt =
                    decimal.Parse(dr[(int)Col835EOB.ePaymentDataHCPCSAmt].ToString(),
                     System.Globalization.NumberStyles.Currency).ToString();
            m_rEob.Reob.m_strPayDataContAdjAmt =
                    decimal.Parse(dr[(int)Col835EOB.ePaymentDataContAdjAmt].ToString(),
                     System.Globalization.NumberStyles.Currency).ToString();
            m_rEob.Reob.m_strPayDataPatRefund =
                    decimal.Parse(dr[(int)Col835EOB.ePaymentDataPatRefund].ToString(),
                     System.Globalization.NumberStyles.Currency).ToString();
            m_rEob.Reob.m_strPayDataPerDiemRate = dr[(int)Col835EOB.ePaymentDataPerDiemRate].ToString();
            m_rEob.Reob.m_strPayDataNetReimbAmt =
                    decimal.Parse(dr[(int)Col835EOB.ePaymentDataNetReimbAmt].ToString(),
                         System.Globalization.NumberStyles.Currency).ToString();
            // 06/03/2008 claim forwarded to can contain apostrophies which require sqlclean to triple them up. Thanks Rick
            m_rEob.Reob.m_strClaimForwardedTo = RFCObject.staticSqlClean(dr[(int)Col835EOB.eClaimForwarded].ToString());
            m_rEob.Reob.m_strClaimForwardedId = dr[(int)Col835EOB.eClaimForwardedID].ToString();
            m_rEob.Reob.m_strEftFile = tbFileName.Tag.ToString();
            m_rEob.Reob.m_strEftNumber = tbFileNumber.Tag.ToString();
            m_rEob.Reob.m_strEftDate = tbFileDate.Tag.ToString();
            m_rEob.Reob.m_strBillCycleDate = tbCheckDate.Tag.ToString();
            //lEob.m_Reob.m_strPrintDate is left null until it is printed from Acc 

            DataGridView dgv = null;
            for (int i = 0; i < tc835.TabPages.Count - 1; i++)
            {

                dgv = GetSelectedTabsDataGrid(i);
                string strSelect = $"Account = '{m_rEob.Reob.m_strAccount.Replace("A", "")}' AND eClaimStatus = '{m_rEob.Reob.m_strClaimStatus}'";
                DataRow[] dgvArrRows = _dsRecords.Tables[dgv.Name].Select(strSelect);
                //  m_ERR.m_Logfile.WriteLogFile(string.Format("   [{0}] {1}", dgvArrRows.GetUpperBound(0), strSelect));
                for (int k = 0; k <= dgvArrRows.GetUpperBound(0); k++)
                {
                    Application.DoEvents();
                    DataRow dgRow = dgvArrRows[k];

                    m_rEob.ReobDetail.ClearMemberVariables();
                    m_rEob.ReobDetail.m_strRowguid = m_rEob.Reob.m_strRowguid;
                    m_rEob.ReobDetail.m_strAccount = m_rEob.Reob.m_strAccount;
                    m_rEob.ReobDetail.m_strClaimStatus = m_rEob.Reob.m_strClaimStatus;

                    // set the check number into both the eob and the eob detail as part of the link to prevent duplicate file postings
                    m_rEob.Reob.m_strCheckNo = dgRow[(int)Col835Grids.eCheckNo].ToString();
                    m_rEob.ReobDetail.m_strCheckNo = m_rEob.Reob.m_strCheckNo;
                    // set the billing cycle from the eob.
                    m_rEob.ReobDetail.m_strBillCycleDate = m_rEob.Reob.m_strBillCycleDate;

                    m_rEob.ReobDetail.m_strContractualAdjAmt =
                        decimal.Parse(dgRow[(int)Col835Grids.eContractualAdjAmt].ToString(),
                        System.Globalization.NumberStyles.Currency).ToString();
                    m_rEob.ReobDetail.m_strOtherAdjAmt =
                         decimal.Parse(dgRow[(int)Col835Grids.eOtherAdjAmt].ToString(),
                            System.Globalization.NumberStyles.Currency).ToString();

                    m_rEob.ReobDetail.m_strAllowedAmt =
                        decimal.Parse(dgRow[(int)Col835Grids.eAllowed].ToString(),
                        System.Globalization.NumberStyles.Currency).ToString();
                    m_rEob.ReobDetail.m_strApcNr =
                        dgRow[(int)Col835Grids.eAPC].ToString();
                    m_rEob.ReobDetail.m_strChargeAmt =
                        decimal.Parse(dgRow[(int)Col835Grids.eCharges].ToString(),
                        System.Globalization.NumberStyles.Currency).ToString();
                    m_rEob.ReobDetail.m_strDateOfService =
                        dgRow[(int)Col835Grids.eDateOfService].ToString();
                    m_rEob.ReobDetail.m_strPaidAmt =
                        decimal.Parse(dgRow[(int)Col835Grids.ePaid].ToString(),
                        System.Globalization.NumberStyles.Currency).ToString();
                    if (dgRow[(int)Col835Grids.eReason].ToString().IndexOf('/') > -1)
                    {
                        string[] strSplitReason = dgRow[(int)Col835Grids.eReason].ToString().Split(new char[] { '/' });
                        m_rEob.ReobDetail.m_strReasonType = strSplitReason[0];
                        m_rEob.ReobDetail.m_strReasonCode = strSplitReason[1];
                    }
                    m_rEob.ReobDetail.m_strRevCode = dgRow[(int)Col835Grids.eRevCode].ToString();
                    m_rEob.ReobDetail.m_strServiceCode = dgRow[(int)Col835Grids.eCPT4Code].ToString();
                    m_rEob.ReobDetail.m_strStat = dgRow[(int)Col835Grids.eStat].ToString();
                    m_rEob.ReobDetail.m_strUnits = dgRow[(int)Col835Grids.eUnits].ToString();
                    m_rEob.ReobDetail.m_strWght = dgRow[(int)Col835Grids.eWeight].ToString();

                    // is this eob in the table already?
                    strWhere = $"check_no = '{m_rEob.Reob.m_strCheckNo}' and bill_cycle_date = '{m_rEob.Reob.m_strBillCycleDate}' AND " +
                        $"account = '{m_rEob.Reob.m_strAccount}' and claim_status = '{m_rEob.Reob.m_strClaimStatus}'";

                    if (m_rEob.Reob.GetActiveRecords(strWhere) > 0)
                    {
                        _eRR.m_Logfile.WriteLogFile(string.Format(@"Account {0} already in EOB table.\r\n \tWHERE Clause: {1}", m_rEob.Reob.m_strAccount, strWhere));
                        break;
                    }

                    if (m_rEob.ReobDetail.AddCommittableRecord() == -1)
                    {
                        m_rEob.ReobDetail.propCommittableTransDetail.Rollback();
                        break;
                    }
                }
            }

            if (m_rEob.Reob.GetActiveRecords(strWhere) > 0)
            {
                continue;
            }
            if (m_rEob.AddComittableRecord() == -1)
            {
                m_rEob.ReobDetail.propCommittableTransDetail.Rollback();
                m_rEob.PropCommittableTrans.Rollback();
                break;
            }
        }
        this.Cursor = Cursors.Default;

        if (m_rEob.PropCommittableTrans.TransactionInformation.Status == TransactionStatus.Active)
        {
            try
            {
                m_rEob.ReobDetail.propCommittableTransDetail.Commit();
                m_rEob.PropCommittableTrans.Commit();
            }
            catch (Exception ex)
            {
                m_rEob.ReobDetail.propCommittableTransDetail.Rollback();
                m_rEob.PropCommittableTrans.Rollback();
                MessageBox.Show(ex.Message);
            }
        }
        if (m_rEob.Reob.propErrMsg.Length != 0)
        {
            MessageBox.Show("Posting EOB's completed");
        }
        else
        {
            MessageBox.Show(m_rEob.Reob.propErrMsg);
        }
    }

    private void tsmiFirst20_Click(object sender, EventArgs e)
    {
        _dsRecords.Tables["dgvProcessed"].Select(string.Format("{0} is not null", Col835Grids.eSubscriberName.ToString()), Col835Grids.eSubscriberName.ToString());

        _nCurrentDisplayed = 0;
        DataGridView dgv = null;
        if (e == null)
        {
            dgv = (DataGridView)sender;
        }
        else
        {
            dgv = GetSelectedTabsDataGrid(tc835.SelectedIndex);
        }
        dgv.Rows.Clear();
        while (dgv.Rows.Count < 20)
        {
            try
            {
                dgv.Rows.Add(_dsRecords.Tables[dgv.Name].Rows[_nCurrentDisplayed++].ItemArray);
            }
            catch (IndexOutOfRangeException)
            {
                break; // don't continue less than 100 records in the dataset
            }
        }
    }

    private void tsmiNext20_Click(object sender, EventArgs e)
    {
        DataGridView dgv = GetSelectedTabsDataGrid(tc835.SelectedIndex);
        dgv.Rows.Clear();
        while (dgv.Rows.Count < 20)
        {
            try
            {
                dgv.Rows.Add(_dsRecords.Tables[dgv.Name].Rows[_nCurrentDisplayed++].ItemArray);
            }
            catch (IndexOutOfRangeException)
            {
                break; // don't continue less than 100 records left in the dataset
            }
        }
    }

    private void tsmiLast20_Click(object sender, EventArgs e)
    {
        DataGridView dgv = GetSelectedTabsDataGrid(tc835.SelectedIndex);
        dgv.Rows.Clear();
        if (_nCurrentDisplayed > _dsRecords.Tables[dgv.Name].Rows.Count)
        {
            _nCurrentDisplayed = _dsRecords.Tables[dgv.Name].Rows.Count;
        }
        _nCurrentDisplayed = _dsRecords.Tables[dgv.Name].Rows.Count - 20;
        while (dgv.Rows.Count < 20)
        {
            try
            {
                dgv.Rows.Add(_dsRecords.Tables[dgv.Name].Rows[(_nCurrentDisplayed++)].ItemArray);
            }
            catch (IndexOutOfRangeException)
            {
                break; // don't continue less than 20 records left in the dataset
            }
        }
    }

    private void tsmiPrevious20_Click(object sender, EventArgs e)
    {
        DataGridView dgv = GetSelectedTabsDataGrid(tc835.SelectedIndex);
        dgv.Rows.Clear();
        _nCurrentDisplayed -= 40;
        if (_nCurrentDisplayed < 0)
        {
            _nCurrentDisplayed = 0; // don't scroll before the beginning of the data set.
        }

        while (dgv.Rows.Count < 20)
        {
            try
            {
                dgv.Rows.Add(_dsRecords.Tables[dgv.Name].Rows[(_nCurrentDisplayed++)].ItemArray);
            }
            catch (IndexOutOfRangeException)
            {
                break; // don't continue less than 20 records left in the dataset
            }
        }
    }

    private void tsbPrintEOB_Click(object sender, EventArgs e)
    {
        tspbRecords.Value = 0;
        tspbRecords.Maximum = _dsRecords.Tables[dgvEOB.Name].Rows.Count;
        tspbRecords.Style = ProgressBarStyle.Blocks;

        for (int i = 0; i < _dsRecords.Tables[dgvEOB.Name].Rows.Count; i++)
        {
            tspbRecords.PerformStep();
            Application.DoEvents();
            string strAccount = _dsRecords.Tables[dgvEOB.Name].Rows[i][(int)Col835EOB.Account].ToString().Replace("A", "");
            string strEOBAccount = _dsRecords.Tables[dgvEOB.Name].Rows[i][(int)Col835EOB.Account].ToString();
            _rAcc.GetBalance(strAccount, out string strBal);
            if (strBal == "ERR")
            {
                continue;
            }
            if (strBal != "0")
            {
                _rEob.Reob.m_strFilter = $"Account = '{strEOBAccount}'AND claim_status = '{_dsRecords.Tables[dgvEOB.Name].Rows[i][(int)Col835EOB.eClaimSt]}' and eft_number = '{tbFileNumber.Tag}'";
                if (_rEob.Reob.GetActiveRecords(_rEob.Reob.m_strFilter) > 0)
                {
                    _rEob.Reob.LoadMemberVariablesFromDataSet();
                    _rEob.PrintEOB();
                }
            }
        }
        MessageBox.Show("COMPLETE", "EOB PRINTING", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
    }

    /// <summary>
    /// Checks our Medicare remit directory for files and gets the date of the last one written in either the
    /// saved or root directory. Then checks the WTH directory for files with extensions .835 with dates newer 
    /// than our last file. If any are found they are copied to our remit directory for processing.
    /// If the file cannot be copied an entry is placed in the error log.
    /// </summary>
    /// <param name="sender"/>
    /// <param name="e"/>
    private void ImportMedicareFilesFromWTH(object sender, EventArgs e)
    {
        // Directory Work
        _diFrom = new DirectoryInfo(Program.AppEnvironment.ApplicationParameters.RemitImportDirectory);

        _diDirectories = _diFrom.GetDirectories(DateTime.Now.Year.ToString());
        if (_diDirectories.GetUpperBound(0) == -1)
        {
            Directory.CreateDirectory($@"{_diFrom}\{DateTime.Now.Year}");
        }

        //file work
        _fiCurrent = _diCurrent.GetFiles();
        _fiSaved = _diSaved.GetFiles();
        _fiInvalid = _diInvalid.GetFiles();
        _fiFrom = _diFrom.GetFiles();

        foreach (FileInfo fCurr in _fiCurrent)
        {
            Application.DoEvents();
            try
            {
                fCurr.MoveTo($@"{_diSaved.FullName}\{fCurr.Name}");
            }
            catch (Exception ex)
            {
                string strReplaceFile = fCurr.Name;
                strReplaceFile = strReplaceFile.Insert(strReplaceFile.LastIndexOf('.'), DateTime.Now.ToFileTimeUtc().ToString());

                fCurr.MoveTo($@"{_diInvalid.FullName}\{strReplaceFile}");

                MessageBox.Show($"{MethodBase.GetCurrentMethod().Name}.\r\n{ex.Message}.", PropAppName);
            }
        }
        // move the new files to our directory
        foreach (FileInfo fFrom in _fiFrom)
        {
            Application.DoEvents();
            try
            {
                fFrom.CopyTo($@"{_diCurrent.FullName}\{fFrom.Name}");

                fFrom.MoveTo($@"{_diFrom.FullName}\{DateTime.Now.Year}\{fFrom.Name}");
            }
            catch (Exception ex)
            {
                string strReplaceFile = fFrom.Name;
                strReplaceFile = strReplaceFile.Insert(strReplaceFile.LastIndexOf('.'), DateTime.Now.ToFileTimeUtc().ToString());

                fFrom.MoveTo($@"{_diFrom.FullName}\{DateTime.Now.Year.ToString()}\{strReplaceFile}");

                MessageBox.Show($"{MethodBase.GetCurrentMethod().Name}.\r\n{ex.Message}.", PropAppName);
            }
        }

        purgeInvalidFilesToolStripMenuItem_Click(null, null);
    }

    private void purgeInvalidFilesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        // 06/06/2008 wdk get the date of the newest file from the invalid directory.
        //DirectoryInfo diInvalid = 
        //    new DirectoryInfo(string.Format(@"{0}\invalid", diInvalid));
        //// 02/29/2008 wdk get the date of the newest file from the saved directory.
        DirectoryInfo diLocal = new(string.Format(@"{0}", _diCurrent));
        FileInfo[] fiLocal = diLocal.GetFiles();
        FileInfo[] fiInvalid = _diInvalid.GetFiles();

        if (fiLocal.GetUpperBound(0) == -1)
        {
            MessageBox.Show("No files available", "PURGE INVALID FILES");
            tspbRecords.Minimum = 0;
            tspbRecords.Maximum = 0;
            return;

        }
        tspbRecords.Minimum = 0;
        tspbRecords.Maximum = fiLocal.GetUpperBound(0);

        foreach (FileInfo f1 in fiLocal)
        {
            Application.DoEvents();
            tspbRecords.PerformStep();
            Application.DoEvents();
            string strContents = RFCObject.GetFileContents(f1.FullName).ToUpper();
            if (!(strContents.Contains("CLP*L") || strContents.Contains("CLP*C") || strContents.Contains("CLP*D")))
            {
                try
                {
                    f1.MoveTo(_diInvalid.FullName + "\\" + f1.Name);
                }
                catch (IOException ioe)
                {
                    string strReplaceFile = f1.Name;
                    strReplaceFile = strReplaceFile.Insert(strReplaceFile.LastIndexOf('.'), DateTime.Now.ToFileTimeUtc().ToString());

                    f1.MoveTo($@"{_diInvalid.FullName}\{strReplaceFile}");
                }
            }
        }
        MessageBox.Show("PURGE COMPLETE", PropAppName);
    }

    private void FindAccountInFilesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Form f = new()
        {
            Text = "FIND ACCOUNT"
        };
        TextBox t = new();
        f.Controls.Add(t);
        ListBox l = new()
        {
            Name = "FRED",
            Top = t.Bottom + 5,
            Width = f.Width
        };
        t.KeyDown += new KeyEventHandler(tKeyDown);
        ProgressBar p = new()
        {
            Name = "GEORGE",
            Dock = DockStyle.Bottom
        };

        f.Controls.Add(l);
        f.Controls.Add(p);
        f.Show();

    }

    void tKeyDown(object sender, KeyEventArgs e)
    {
        throw new NotImplementedException("inside t_KeyDown notify David");
    }

} // don't go below this line.