// added for DataGridViewPrinter
using System.Data;
using System.Drawing.Printing;
namespace Utilities;

/// <summary>
/// Creates a ReportGenerator with a DataGridViewPrinter object for setting up the printing for various reports.
/// 08/22/2007 rgc/wdk
/// </summary>
[Obsolete]
public class ReportGenerator
{
    /// <summary>
    /// Added to print a data set 06/12/2008 rgc/wdk
    /// </summary>
    public DataSet m_dsReport;
    /// <summary>
    /// This objects copy of the DataGridViewPrinter
    /// </summary>
    public DataGridViewPrinter m_dgvpReport;
    private DataGridView m_dgvGrid;
    /// <summary>
    /// 
    /// </summary>
    public string m_strFooter;
    private string m_strTitle;
    /// <summary>
    /// Gets or sets the title for the report after the creation of the report.
    /// </summary>
    public string propTitle
    {
        get { return m_strTitle; }
        set { m_strTitle = value; }
    }
    private string m_strDBase;

    /// <summary>
    /// ReportGenerator constructor
    /// 08/22/2007 rgc/wdk
    /// </summary>
    /// <param name="aDataGridView"></param>
    /// <param name="aPrintDocument"></param>
    /// <param name="aTitleText"></param>
    /// <param name="strDBase"></param>
    public ReportGenerator(DataGridView aDataGridView, PrintDocument aPrintDocument,
        string aTitleText,
        string strDBase) // 08/23/2007 wdk/rgc added for shell to ACC program.
    {
        m_strTitle = aTitleText;
        m_dgvGrid = aDataGridView;

        m_strDBase = strDBase;
        m_dgvpReport = new DataGridViewPrinter(aDataGridView, aPrintDocument, aTitleText);
    }


    /// <summary>
    /// 07/18/2007 Usage. Create an instance of the report generator, ex.
    ///    <code>m_rgIns = new ReportGenerator(dgReportInsurance, MyPrintDocument, true, true, "Insurance Report by Plan Name",
    ///                             new Font("Tahoma", 18, FontStyle.Bold, GraphicsUnit.Point), Color.Black, true, m_strFooterText);
    ///     </code>
    ///     then call this function with your print document and datagridview. If it can be setup call print for your print document.
    ///    <code>if (m_rgIns.SetupThePrinting(this.MyPrintDocument, this.dgReportInsurance))
    ///       MyPrintDocument.Print();
    ///     </code>
    /// </summary>
    /// <returns></returns>
    public bool SetupThePrinting(PrintDocument MyPrintDocument, DataGridView dgvToPrint)
    {
        PrintDialog MyPrintDialog = new PrintDialog();

        MyPrintDialog.AllowCurrentPage = true;
        MyPrintDialog.AllowPrintToFile = true;
        MyPrintDialog.AllowSelection = true;
        MyPrintDialog.AllowSomePages = true;
        //MyPrintDialog.PrintToFile = true;
        MyPrintDialog.ShowHelp = true;
        MyPrintDialog.ShowNetwork = true;

        if (MyPrintDialog.ShowDialog() != DialogResult.OK)
        {
            return false;
        }
        MyPrintDocument = m_dgvpReport.ThePrintDocument;
        return true;

    }

    /// <summary>
    ///  The PrintPage action for the PrintDocument control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void MyPrintDocument_PrintPage(object sender,
        System.Drawing.Printing.PrintPageEventArgs e)
    {
        bool more = m_dgvpReport.DrawDataGridView(e.Graphics);

        if (more == true)
        {
            e.HasMorePages = true;
        }
    }

} // don't type below this line
