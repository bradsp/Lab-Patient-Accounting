namespace Utilities;

/// <summary>
/// wdk 07/20/2007
/// this is a helper class for use with combobox's
/// Currently this only takes two variables to create but could be modified to a template structure so
/// it could take more paramaters.
/// 
/// It's primary use is to load both a code and a description at this time. it is created by
/// 1. create an ArrayList for use
///     ArrayList arrayFinCodes;
/// 2. Where you want to load the combobox. (I prefer application/form load function. In the ReportByInsuranceCompany it is Form1_Load()
///     a. Create a new ArrayList
///         arrayFinCodes = new ArrayList();
/// 
///     b. Inside the record set loop Start adding codes and descriptions of the type ListObject to the arraylist
///        arrayFinCodes.Add(new ListObject(rFin.m_strFinCode, rFin.m_strResParty));
///        cbFinCode.Items.Add(arrayFinCodes);
/// 
/// It is currently used in the ReportByInsurancePlanName, 
/// to display the Description as the tool tip for the combo control for the selected insurance
/// 
/// In the function below it is used to set the tooltip for the combo box for the selected fincode.
/// private void cbFinCode_SelectedIndexChanged(object sender, EventArgs e)
/// {
///    cbFinCode.ToolTipText = ((ListObject)((ToolStripComboBox)sender).SelectedItem).Description;
///     //note the above is a cast into a list object description of a toolstipcombobox selected item using the sender argument 
/// }
/// </summary>
public class ListObject
{
    /// <summary>
    /// Can be used to set the display or value reference for the combo box 
    /// </summary>
    public static string ShortName = "DisplayName";

    /// <summary>
    /// Can be used to set the display or value reference for the combo box 
    /// </summary>
    public static string LongName = "Description";

    /// <summary>
    /// The actual value set into the list item of the combobox must be set using the property DisplayName
    /// </summary>
    private string m_strDisplayName;

    /// <summary>
    /// The actual value set to describe the DisplayName in the combobox must be set using the property Description
    /// </summary>
    private string m_strDescription;
    /// <summary>
    /// wdk 07/20/2007 
    /// Constructs a ListObject with two associated strings i.e.
    /// FinCode and the description of the FinCode
    /// </summary>
    /// <param name="strDisplayName">Value to display in a combo or list box</param>
    /// <param name="strDescription">Data defining or associated with the displayed value</param>
    public ListObject(string strDisplayName, string strDescription)
    {
        this.m_strDisplayName = strDisplayName;
        this.m_strDescription = strDescription;
    }
    /// <summary>
    ///  Its primary use is to load both a code and a description at this time
    /// 1. Create By 
    ///     a. create an ArrayList for use
    ///         ArrayList arrayFinCodes;
    ///     b. new ListObject[] { "A", "MEDICARE"}; this can be directly added to a combobox and will display 
    ///         To return this value use the combobox itself ((ListObject)tscbFinCode.SelectedItem).DisplayName 
    ///             or
    ///         in an event handler
    ///         ((ListObject)((ToolStripComboBox)sender).SelectedItem).DisplayName.
    /// 2. Where you want to load the combobox. (I prefer application/form load function. In the ReportByInsuranceCompany it is Form1_Load()
    ///     a. Create a new ArrayList
    ///         arrayFinCodes = new ArrayList();
    /// 
    ///     b. Inside the record set loop Start adding codes and descriptions of the type ListObject to the arraylist
    ///        arrayFinCodes.Add(new ListObject(rFin.m_strFinCode, rFin.m_strResParty));
    ///        cbFinCode.Items.Add(arrayFinCodes);
    /// 
    /// private void cbFinCode_SelectedIndexChanged(object sender, EventArgs e)
    /// {
    ///    cbFinCode.ToolTipText = ((ListObject)((ToolStripComboBox)sender).SelectedItem).Description;
    ///     //note the above is a cast into a list object description of a toolstipcombobox selected item using the sender argument 
    /// }
    /// </summary>
    /// <param name="TName"></param>
    /// <param name="TValue"></param>
    public ListObject(object TName, object TValue)
    {
        m_strDisplayName = TName.ToString();
        m_strDescription = TValue.ToString();
    }

    /// <summary>
    /// Allows the user to inter the value to be displayed in the combobox
    /// </summary>
    public string DisplayName
    {
        get
        {
            return m_strDisplayName;
        }
    }
    /// <summary>
    /// Allows the user to enter the value that describes the value in the combo box.
    /// </summary>
    public string Description
    {
        get
        {
            return m_strDescription;
        }
    }
    /// <summary>
    /// Returns both parts of the list object as a string.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return this.DisplayName + " - " + this.Description;
    }
}
