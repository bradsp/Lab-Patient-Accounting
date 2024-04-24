using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;// for SqlDataSourceEnumerator
// programmer added
using System.Collections.Specialized;// for StringCollection (new mapping type best I can tell wdk.)
using System.Data;
using System.Drawing.Printing;
using WinFormsLibrary;

namespace Utilities;

/// <summary>
/// This viewer allows manipulation of a basic sql viewer. It contains a tab control which
/// contains individual tab pages that contain a split container with two pannels. The first
/// pannel contains a text box for sql code to be executed. The second pannel contains a
/// DataGridView that can be printed.
/// 
/// 10/12/2007 David Kelly
/// </summary>
[Obsolete]
public partial class ViewerBase : Form
{
    #region VARIABLES
    private bool m_bAddDateRangeControls = true;

    public EventHandler<string> LaunchAccountEventHandler;
    /// <summary>
    /// Displays or hides the date range controls based on programmers needs
    /// </summary>
    public bool propAddDateRangeControls
    {
        get { return m_bAddDateRangeControls; }
        set { m_bAddDateRangeControls = value; }
    }

    private bool m_bReportPrinterOnly = false;
    /// <summary>
    /// When true this will only print the data that is in the selected data grid view
    /// </summary>
    public bool propReportPrinterOnly
    {
        get { return m_bReportPrinterOnly; }
        set { m_bReportPrinterOnly = value; }
    }

    ToolStripControlHost m_dpFrom;
    ToolStripControlHost m_dpThru;
    private string m_strThruDate = ""; // default value set in the ViewerBase_InitialFormLoad
    /// <summary>
    /// Gets or sets the forms Thru date range control
    /// </summary>
    public string propstrThruDate
    {
        get { return m_strThruDate; }
        set { m_strThruDate = value; }
    }


    private string m_strFromDate = ""; // default value set in the ViewerBase_InitialFormLoad
    /// <summary>
    /// Gets or sets the forms From date range control
    /// </summary>
    public string propstrFromDate
    {
        get { return m_strFromDate; }
        set { m_strFromDate = value; }
    }

    /// <summary>
    /// Gets or sets the number of tabs 
    /// </summary>
    public int propNumberTabs
    {
        get { return int.Parse(tstbTabs.Text); }
        set { tstbTabs.Text = value.ToString(); }
    }


    private string m_strDateRangeField = "";
    /// <summary>
    /// Gets or sets whether the date range fields will be visible on the form.
    /// </summary>
    public string propDateRangeField
    {
        get { return m_strDateRangeField; }
        set { m_strDateRangeField = value; }
    }

    /// <summary>
    /// Gets or sets the database to use on the viewer. For
    /// any call during a selectedIndexChanged use the combos .Text feature to get the correct value for the
    /// sqlConnection string.
    /// </summary>
    public string propDatabase
    {
        get { return tscbDatabase.Text; }
        set
        {
            tscbDatabase.Text = value;
        }
    }

    /// <summary>
    /// Gets or set the server to use on the viewer. For
    /// any call during a selectedIndexChanged use the combos .Text feature to get the correct value for the
    /// sqlConnection string.
    /// </summary>
    public string propServer
    {
        get { return tscbServer.Text; }
        set
        {
            tscbServer.Text = value;
        }
    }


    BindingSource m_bsDataSource;
    private ReportGenerator m_rgReport;
    private PrintDocument m_PrintDocument;
    private DataSet m_dsSource;
    /// <summary>
    /// Returns the data source
    /// </summary>
    public DataSet propDataSource
    {
        get { return m_dsSource; }
    }

    private string m_strQuery;
    /// <summary>
    /// Returns the sql query.
    /// </summary>
    public string propSqlQuery
    {
        get { return m_strQuery; }
    }

    private string m_strReportTitle;
    /// <summary>
    /// Gets or sets the report's title
    /// </summary>
    public string propReportTitle
    {
        get { return m_strReportTitle; }
        set { m_strReportTitle = value; }
    }

    private string m_strSqlConnection = "";
    /// <summary>
    /// Returns the sql connection.
    /// </summary>
    public string propSqlConnection
    {
        get
        {
            if (m_strSqlConnection.Length == 0)
            {
                m_strSqlConnection = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", propServer, propDatabase);
            }
            return m_strSqlConnection;
        }
        set { m_strSqlConnection = value; }
    }

    private string m_strErrMsg = "";
    /// <summary>
    /// returns the error message that this class sets.
    /// </summary>
    public string propErrMsg
    {
        get { return m_strErrMsg; }
    }

    SplitContainer[] m_arrSC;
    Font m_font;
    StringCollection m_scTabs;
    #endregion VARIABLES

    /// <summary>
    /// Constructor for the base viewer.
    /// document the string[] strArgs when discovered????!
    /// Don't remember what I was trying to do.
    /// 
    /// 10/12/2007 David Kelly
    /// </summary>
    public ViewerBase(string[] strArgs)
    {
        InitializeComponent();
    }

    /// <summary>
    /// Empty constructor to call with no arguments.
    /// 
    /// 10/14/2008 wdk
    /// </summary>
    public ViewerBase()
    {
        InitializeComponent();
    }

    private void ViewerBase_InitialFormLoad(object sender, EventArgs e)
    {
        if (m_bAddDateRangeControls)
        {
            AddDateRangeControls();
        }
        m_font = RFWinformObject.CreateFont("Arial", 18);

        //if (!m_bReportPrinterOnly)
        //{

        ////    // multi thread to load the servers
        ////    AutoResetEvent asyncOpIsDone = new AutoResetEvent(false);
        ////    ThreadPool.QueueUserWorkItem(new
        ////        WaitCallback(LoadServers), asyncOpIsDone);
        //}
        //else
        //{
        //    InitializeViewer(); // we are report printer only so we only need the datagridview
        //    return;
        //}
        CreateTabControl();
    }

    private void InitializeViewer()
    {
        tsbLoadGrid.Enabled = false;
        tslServer.Enabled = false;
        tscbServer.Enabled = false;
        tslDatabase.Enabled = false;
        tscbDatabase.Enabled = false;
        tstbTabs.Enabled = false;
        m_strSqlConnection = m_strSqlConnection = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", propServer, propDatabase);
        DataGridView dgvPrint = new DataGridView();
        dgvPrint.Dock = DockStyle.Fill;
        dgvPrint.AutoGenerateColumns = true;
        dgvPrint.AllowUserToDeleteRows = true;

        tabPage1.Controls.Add(dgvPrint);



    }

    /// <summary>
    /// Configures the scTabControl in the following manner.
    /// 1. Add a split container to each of the tab pages.
    /// 2. Add a rich text box to split container's panel one.
    /// 2. Add a data grid view to split container's panel two.
    /// 
    /// 10/12/2007 David Kelly
    /// </summary>
    private void CreateTabControl()
    {
        InitializeStringNumberMap();
        TabPage tp = new TabPage();
        tp.Margin = new Padding(3);

        m_tcSelect.TabPages.Clear();
        if (int.Parse(tstbTabs.Text) > 10)
        {
            tstbTabs.Text = "10";
        }
        for (int i = 0; i < int.Parse(tstbTabs.Text); i++)
        {
            m_tcSelect.TabPages.Add(new TabPage());
        }
        InitializeSplitContainerArray();

        foreach (SplitContainer sc in m_arrSC)
        {
            // add a second split container to the top pannel
            SplitContainer scTop = new SplitContainer();
            scTop.Dock = DockStyle.Fill;
            scTop.Orientation = Orientation.Horizontal;
            scTop.SplitterDistance = (int)(sc.Panel1.Height * .25);
            scTop.Refresh();

            // create a title box
            RichTextBox rtbTitle = new RichTextBox();
            rtbTitle.Width = 300;
            rtbTitle.Dock = DockStyle.Fill;
            rtbTitle.Font = m_font;
            rtbTitle.BackColor = Color.AliceBlue;


            // create an sql box
            RichTextBox rtbSql = new RichTextBox();
            rtbSql.Dock = DockStyle.Fill;
            rtbSql.Font = m_font;


            // add the title box to the 1st pannel of the newly created split container.
            scTop.Panel1.Controls.Add(rtbTitle);
            // add the sql box to the 2nd pannel of the newly craeated split container.
            scTop.Panel2.Controls.Add(rtbSql);


            // create the datagridview
            DataGridView dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;

            // add the split container created above and the datagrid view to the main split container
            sc.Panel1.Controls.Add(scTop);
            sc.Panel2.Controls.Add(dgv);
        }

        for (int i = 0; i < m_tcSelect.TabPages.Count; i++)//TabPage tp in tcSelect.TabPages)
        {
            m_tcSelect.TabPages[i].Text = string.Format("Tab {0}", m_scTabs[i + 1]);
            m_arrSC[i].Dock = DockStyle.Fill;
            m_arrSC[i].Orientation = Orientation.Horizontal;
            m_arrSC[i].SplitterDistance = 10;
            m_tcSelect.TabPages[i].Controls.Add(m_arrSC[i]);
        }

    }

    /// <summary>
    /// Number map only goes to 10 at this time. If more tabs are needed just add to the
    /// end of the array.
    /// 
    /// 10/12/2007 David Kelly
    /// </summary>
    private void InitializeStringNumberMap()
    {
        m_scTabs = new StringCollection();
        m_scTabs.AddRange(new string[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten" });//strarrNumbers);
    }

    /// <summary>
    /// This initializes the split container array.
    /// The number of elements depends on the number of tab pages in the tab control.
    /// The Tab control was used because you can dynamically resize a tab control but not 
    /// an array.
    /// 
    /// WARNING: if you call this function the current data will be lost.
    /// 
    /// 10/12/2007 David Kelly
    /// </summary>
    private void InitializeSplitContainerArray()
    {
        m_arrSC = new SplitContainer[m_tcSelect.TabPages.Count];
        int i = 0;
        foreach (TabPage tp in m_tcSelect.TabPages)
        {
            m_tcSelect.TabPages.GetEnumerator();
            m_arrSC.SetValue(new SplitContainer(), i++);
        }

    }

    private void tstbTabs_KeyUp(object sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.D0:
            case Keys.D1:
            case Keys.D2:
            case Keys.D3:
            case Keys.D4:
            case Keys.D5:
            case Keys.D6:
            case Keys.D7:
            case Keys.D8:
            case Keys.D9:
            case Keys.NumPad0:
            case Keys.NumPad1:
            case Keys.NumPad2:
            case Keys.NumPad3:
            case Keys.NumPad4:
            case Keys.NumPad5:
            case Keys.NumPad6:
            case Keys.NumPad7:
            case Keys.NumPad8:
            case Keys.NumPad9:
            {
                CreateTabControl();
                break;
            }
            default:
            {
                bKeyuphandled = true;
                e.Handled = true; // this beeps and keeps the control from continuing. That way you can't create 'e' number of tabs etc.
                return;
            }


        }
    }

    private void LoadServers(object state)
    {
        SqlDataSourceEnumerator instance =
         SqlDataSourceEnumerator.Instance;
        DataTable datatableServers = instance.GetDataSources();

        foreach (System.Data.DataRow dr in datatableServers.Rows)
        {
            if (dr.ItemArray[0].ToString().IndexOf("MCL") == -1 || dr.ItemArray[0].ToString().IndexOf("MCLINET") > -1)
            {
                continue;
            }
            tscbServer.Items.Add(dr.ItemArray[0].ToString());
        }
        //// Signal that the thread is now complete.
        ((AutoResetEvent)state).Set();
        ////Enable necessary controls.
        if (propServer.Length > 0)
        {
            tscbServer.SelectedIndex = tscbServer.FindStringExact(propServer);
        }
    }

    private void tscbServer_SelectedIndexChanged(object sender, EventArgs e)
    {
        tscbDatabase.Items.Clear();

        m_strSqlConnection = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", tscbServer.Text, "master");
        string queryString = "EXEC sp_databases";
        using (SqlConnection connection = new SqlConnection(m_strSqlConnection))
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            try
            {
                connection.Open();
            }
            catch (SqlException se)
            {
                m_strErrMsg = (se.Message);
                connection.Close();
                return;
            }
            SqlDataReader reader = command.ExecuteReader();

            // Call Read before accessing data.
            while (reader.Read())
            {
                tscbDatabase.Items.Add(reader[0].ToString());
            }
            // Call Close when done reading.
            reader.Close();
            connection.Close();
            if (tscbDatabase.FindStringExact(tscbDatabase.Text) == -1)
            {
                tscbDatabase.Text = "";
            }
        }
    }

    private void tscbDatabase_SelectedIndexChanged(object sender, EventArgs e)
    {
        m_strSqlConnection = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", tscbServer.Text, tscbDatabase.Text);
    }

    private void tsbLoadGrid_Click(object sender, EventArgs e)
    {
        string strClean = GetSqlText();

        if (Environment.UserName == "wkelly" || Environment.UserName == "rcrone")
        {
            // for testing Rick and I might need to do some neat stuff.
        }
        else
        {
            if (strClean.IndexOf("--") > -1 || strClean.IndexOf("ALTER") > -1 || strClean.IndexOf("CREATE") > -1 || strClean.IndexOf("EXEC") > -1 || strClean.IndexOf("SP_") > -1 || strClean.IndexOf("UNION") > -1 || strClean.IndexOf("DROP") > -1 || strClean.IndexOf("EMPTY") > -1 || strClean.IndexOf("DELETE ") > -1 || strClean.IndexOf("UPDATE") > -1)
            {
                MessageBox.Show("This control can only [SELECT] from the database. \r\n\nPlease reformat your query.");
                return;
            }
        }
        LoadGrid(strClean);
    }


    /// <summary>
    /// Get the sql text from the tab page that has focus
    /// </summary>
    /// <returns></returns>
    public string GetSqlText()
    {
        // get the split container in the main split container
        //  <code>((SplitContainer)m_arrSC[m_tcSelect.SelectedIndex].Panel1.Controls[0])
        //  then get the text that is in panel2's only control which is our text box for the sql
        //  .Panel2.Controls[0].Text.ToUpper();
        string strClean =
            ((SplitContainer)m_arrSC[m_tcSelect.SelectedIndex].Panel1.Controls[0]).Panel2.Controls[0].Text.ToUpper();
        RFCObject.staticSqlClean(strClean);
        return strClean;
    }

    /// <summary>
    /// Sets the sql text on the tab requested.
    /// </summary>
    /// <param name="strSql"></param>
    /// <param name="nTab"></param>
    public void SetSqlText(string strSql, int nTab)
    {
        // set the split container in the main split container
        //  <code>((SplitContainer)m_arrSC[m_tcSelect.SelectedIndex].Panel1.Controls[0])
        //  then get the text that is in panel2's only control which is our text box for the sql
        //  .Panel2.Controls[0].Text.ToUpper();

        ((SplitContainer)m_arrSC[nTab].Panel1.Controls[0]).Panel2.Controls[0].Tag = strSql;
        if (m_strDateRangeField.Length > 0)
        {
            strSql += string.Format(" WHERE {0} between '{1}' and '{2}'", m_strDateRangeField, m_strFromDate, m_strThruDate);//((DateTimePicker)m_dpFrom.Control).Value.ToString("d"), ((DateTimePicker)m_dpThru.Control).Value.ToString("d"));
        }
        ((SplitContainer)m_arrSC[nTab].Panel1.Controls[0]).Panel2.Controls[0].Text = strSql;
    }

    private void tspPrintGrid_Click(object sender, EventArgs e)
    {
        int nRows = ((DataGridView)m_arrSC[m_tcSelect.SelectedIndex].Panel2.Controls[0]).Rows.Count;
        if (nRows < 1)
        {
            MessageBox.Show("Must have data in the grid to print.");
            return;
        }
        // create our reportgenerator and assign event handlers
        m_PrintDocument = new PrintDocument();

        string strWhere = GetSqlText();
        int nWhere = strWhere.IndexOf("WHERE");
        if (nWhere == -1)
        {
            strWhere = "NO WHERE CLAUSE PROVIDED";
        }
        else
        {
            strWhere = strWhere.Substring(nWhere, (strWhere.Length - nWhere));
            strWhere = strWhere.Replace(Environment.NewLine, " ");
        }
        m_rgReport = new ReportGenerator(((DataGridView)m_arrSC[m_tcSelect.SelectedIndex].Panel2.Controls[0]), m_PrintDocument, string.Format("{0}", m_strReportTitle), tscbDatabase.SelectedItem.ToString());
        m_rgReport.m_dgvpReport.propFooterText = strWhere;
        this.m_PrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(m_rgReport.MyPrintDocument_PrintPage);


        ((DataGridView)m_arrSC[m_tcSelect.SelectedIndex].Panel2.Controls[0]).RowHeaderMouseDoubleClick += OnRowHeaderMouseDoubleClick;


        if (m_rgReport.SetupThePrinting(this.m_PrintDocument, ((DataGridView)m_arrSC[m_tcSelect.SelectedIndex].Panel2.Controls[0])))
        {
            m_PrintDocument.Print();
        }

    }

    private void OnRowHeaderMouseDoubleClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        if (sender is not DataGridView dgv)
            return;

        if (dgv == null)
            return;

        var account = dgv.Rows[e.RowIndex].Cells["Account"].Value.ToString();
        if(!string.IsNullOrEmpty(account))
            LaunchAccountEventHandler?.Invoke(this, account);


    }


    ///// <summary>
    ///// Creates the data set using the string arguments passed in. 
    ///// manditory to have the server and the database for the initial call as we set the connection
    ///// here.
    ///// 
    ///// 10/12/2007 David Kelly
    ///// </summary>
    ///// <param name="strSql"></param>
    //private void CreateDataSet(string strSql)
    //{

    //}
    /// <summary>
    /// Gets the title text from the current tab
    /// </summary>
    /// <returns></returns>
    public string GetTitleText()
    {
        // get the split container in the main split container
        //  ((SplitContainer)m_arrSC[m_tcSelect.SelectedIndex].Panel1.Controls[0])
        //  then get the text that is in panel1's only control which is our text box for the title
        //  .Panel1.Controls[0].Text.ToUpper();
        string strTitle =
            ((SplitContainer)m_arrSC[m_tcSelect.SelectedIndex].Panel1.Controls[0]).Panel1.Controls[0].Text.ToUpper();
        RFCObject.staticSqlClean(strTitle);
        return strTitle;
    }
    /// <summary>
    /// Sets the title text from the current tab
    /// 10/01/2008 wdk/rgc modified to pass in a tab number
    /// </summary>
    /// <returns></returns>
    public void SetTitleText(string strTitle, int nTab)
    {// may be getting called to early
        if (this == null)
            return;
        // Get the split container in the main split container
        //  ((SplitContainer)m_arrSC[m_tcSelect.SelectedIndex].Panel1.Controls[0])
        //  then set the text that is in panel1's only control which is our text box with the title
        //  .Panel1.Controls[0].Text = strTitle.ToUpper();

        //((SplitContainer)
        //    m_arrSC[m_tcSelect.SelectedIndex].Panel1.Controls[0]).Panel1.Controls[0].Text = strTitle.ToUpper();           
        ((SplitContainer)
          m_arrSC[nTab].Panel1.Controls[0]).Panel1.Controls[0].Text = strTitle.ToUpper();
    }

    /// <summary>
    /// Loads the DataGridView associated with the tab that is passed to the function.
    /// </summary>
    /// <param name="nTab"></param>
    /// <returns></returns>
    public bool LoadGrid(int nTab)
    {
        m_tcSelect.SelectedIndex = nTab;
        string strText = GetSqlText();
        if (strText.Length == 0)
        {
            m_strErrMsg = string.Format("Tab {0} does not contain any text", nTab);
            return false;
        }
        LoadGrid(strText);
        return true;

    }
    /// <summary>
    /// Loads the grid using the selected data set
    /// 
    /// 08/10/2007 wdk
    /// </summary> 
    public void LoadGrid(string strQuery)
    {
        m_strQuery = strQuery; // so we can put parts on the print out.
        // create a new DataSet
        m_dsSource = new DataSet("PrintDataSet");
        // Set up the data source.
        m_bsDataSource = new BindingSource();
        ((DataGridView)m_arrSC[m_tcSelect.SelectedIndex].Panel2.Controls[0]).AutoGenerateColumns = true;

        // m_dsSource = new DataSet("PrintDataSet");
        // set the dataset from the returned dataset
        m_dsSource = SelectRows(m_dsSource, propSqlConnection, m_strQuery);
        // set the binding source from the returned m_instanceTable
        try
        {
            m_bsDataSource.DataSource = m_dsSource.Tables[0];    // always 0 [Zero] becuase our current dataset only uses one m_instanceTable
        }
        catch (IndexOutOfRangeException)
        {
            // just ignore and return.
            return;
        }
        catch (SqlException se)
        {
            m_strErrMsg = se.Message;
        }
        // set the gridviews data source
        ((DataGridView)m_arrSC[m_tcSelect.SelectedIndex].Panel2.Controls[0]).DataSource = m_bsDataSource;
        ((DataGridView)m_arrSC[m_tcSelect.SelectedIndex].Panel2.Controls[0]).AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;

        int rowNum = 1;
        foreach (DataGridViewRow row in ((DataGridView)m_arrSC[m_tcSelect.SelectedIndex].Panel2.Controls[0]).Rows)
        {
            row.HeaderCell.Value = rowNum.ToString();
            rowNum++;
        }
        ((DataGridView)m_arrSC[m_tcSelect.SelectedIndex].Panel2.Controls[0]).AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        ((DataGridView)m_arrSC[m_tcSelect.SelectedIndex].Panel2.Controls[0]).Invalidate();
    }

    /// <summary>
    ///  Returns the data set matching the query using the connection string passed.
    ///  05/25/2007 wdk
    /// </summary>
    /// <param name="dataset"></param>
    /// <param name="connectionString"></param>
    /// <param name="queryString"></param>
    /// <returns></returns>
    private DataSet SelectRows(DataSet dataset,
        string connectionString, string queryString)
    {
        using (SqlConnection connection =
            new SqlConnection(connectionString))
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(
                queryString, connection);
            try
            {
                adapter.Fill(dataset);
            }
            catch (SqlException se)
            {
                m_strErrMsg = se.Message + "SQL EXCEPTION\r\n\nCannot process request";
            }
            catch (InvalidOperationException ioe)
            {
                m_strErrMsg = ioe.Message;
            }
            return dataset;
        }
    }

    /// <summary>
    /// Allows the application to execute SQL stored procedures without having to interact with the 
    /// user. Example 
    /// </summary>
    /// <param name="strStoredProcedure"></param>
    public void ExecuteStoredProcedure(string strStoredProcedure)
    {
        if (strStoredProcedure.Length == 0)
        {
            m_strErrMsg = "No stored procedure selected for execution.";
            return;
        }
        tscbDatabase_SelectedIndexChanged(null, null);
        m_strQuery = string.Format("EXECUTE {0}", strStoredProcedure);
        LoadGrid(m_strQuery);
    }

    /// <summary>
    /// Setting the text of the control actually sets the value. If the control is not displayed the text is 
    /// empty. use ((DateTimePicker)m_dpFrom.Control).Value to get the date in datetime format. 
    /// add .ToString("d") for a string representation without time.
    /// </summary>
    private void AddDateRangeControls()
    {
        if (m_bReportPrinterOnly) // for reports only we don't need and can't change the query.
        {
            return;
        }
        if (m_strThruDate.Length == 0)
        {
            m_strThruDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("d");
        }
        if (m_strFromDate.Length == 0)
        {
            m_strFromDate = DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)).ToString("d");
        }

        this.Size = new Size(this.Width + 10, this.Height + 10);
        Invalidate();

        if (m_dpFrom != null) // don't add this twice.
        {
            return;
        }
        DateTime dtTemp;
        if (!DateTime.TryParse(m_strFromDate, out dtTemp) || !DateTime.TryParse(m_strThruDate, out dtTemp))
        {
            m_strErrMsg = "From/Thru date is invalid";
            return;
        }
        int nSert = tsMain.Items.Count;
        // create the datetime controls for the From and Thru dates   
        m_dpFrom = new ToolStripControlHost(new DateTimePicker());
        ((DateTimePicker)m_dpFrom.Control).Format = DateTimePickerFormat.Short;
        ((DateTimePicker)m_dpFrom.Control).Text = m_strFromDate;
        ((DateTimePicker)m_dpFrom.Control).ValueChanged += new EventHandler(FromDateTimePickerValue_Changed);
        m_dpFrom.Control.Width = 95; // this is the actual control host.
        tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
        ToolStripLabel tslFrom = new ToolStripLabel("From: ");
        tsMain.Items.Insert(tsMain.Items.Count, tslFrom);
        tsMain.Items.Insert(tsMain.Items.Count, m_dpFrom);


        m_dpThru = new ToolStripControlHost(new DateTimePicker());
        ((DateTimePicker)m_dpThru.Control).Format = DateTimePickerFormat.Short;
        ((DateTimePicker)m_dpThru.Control).Text = m_strThruDate;
        ((DateTimePicker)m_dpThru.Control).ValueChanged += new EventHandler(ThruDateTimePickerValue_Changed);
        m_dpThru.Control.Width = 95; // this is the actual control host.
        tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
        ToolStripLabel tslThru = new ToolStripLabel("Thru: ");
        tsMain.Items.Insert(tsMain.Items.Count, tslThru);
        tsMain.Items.Insert(tsMain.Items.Count, m_dpThru);


        tsMain.Refresh();

    }
    private void ThruDateTimePickerValue_Changed(object sender, EventArgs e)
    {
        m_strThruDate = ((DateTimePicker)sender).Text;
        SetSqlText(((SplitContainer)m_arrSC[m_tcSelect.SelectedIndex].Panel1.Controls[0]).Panel2.Controls[0].Tag.ToString(), m_tcSelect.SelectedIndex);
    }
    private void FromDateTimePickerValue_Changed(object sender, EventArgs e)
    {
        m_strFromDate = ((DateTimePicker)sender).Text;
        SetSqlText(((SplitContainer)m_arrSC[m_tcSelect.SelectedIndex].Panel1.Controls[0]).Panel2.Controls[0].Tag.ToString(), m_tcSelect.SelectedIndex);
    }

    bool bKeyuphandled = false;
    private void tstbTabs_TextChanged(object sender, EventArgs e)
    {
        if (!bKeyuphandled)
        {
            CreateTabControl();
        }
    }

    private void tsbAbout_Click(object sender, EventArgs e)
    {
        Form ab = new Form();
        ab.Size = new Size(300, 200);

        TableLayoutPanel tp = new TableLayoutPanel();
        tp.RowCount = 5;
        Size sz = ab.Size;
        sz.Height -= 10;
        tp.Size = sz;
        tp.Top += 10;


        ab.Text = "About Viewer";

        Label lblVer = new Label();
        lblVer.Dock = DockStyle.Fill;
        lblVer.Text = string.Format("Version: {0}", "1.0.0.0");
        tp.Controls.Add(lblVer);

        Label lblBuild = new Label();
        lblBuild.Dock = DockStyle.Fill;
        lblBuild.Text = string.Format("Build Date: {0}", "10/18/2007");
        tp.Controls.Add(lblBuild);

        Label lblProg = new Label();
        lblProg.Dock = DockStyle.Fill;
        lblProg.Text = string.Format("Programmers: David Kelly and Rick Crone");
        tp.Controls.Add(lblProg);

        RichTextBox lblText = new RichTextBox();
        lblText.ReadOnly = true;
        lblText.AutoSize = true;
        tp.SetRowSpan(lblText, 2);
        lblText.Dock = DockStyle.Fill;
        lblText.Text = "This is a generic Sql viewer. Resetting the number of tabs clears ALL tabs.";
        tp.Controls.Add(lblText);

        ab.Controls.Add(tp);
        ab.ShowDialog();
    }

    private void ViewerBase_Shown(object sender, EventArgs e)
    {
        Size sz = this.Size;
        sz.Height += 20;
        this.Size = sz;
        m_tcSelect.SelectedIndex = 0;
        this.Invalidate();
    }



} // don't go below this line