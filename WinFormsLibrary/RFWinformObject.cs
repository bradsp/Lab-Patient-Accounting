using Utilities;

namespace WinFormsLibrary
{
    public class RFWinformObject : RFCObject
    {
        /// <summary>
        /// Creates a new font of the size specified.
        /// The default font name is "Arial"  and the size is 12 unless a name is passed in.
        /// 
        /// 10/12/2007 David Kelly
        /// </summary>
        /// <param name="strFontName"></param>
        /// <param name="nFontSize"></param>
        public static Font CreateFont(string strFontName, int nFontSize)
        {
            Font ftRetVal;
            // create the default font.
            ftRetVal = new Font(
               new FontFamily("Arial"),
               12,
               FontStyle.Bold,
               GraphicsUnit.Pixel);

            // Check the name that was passed to see if it is a valid font.
            bool bValidFontName = false;
            if (strFontName.Length > 0)
            {
                foreach (FontFamily ff in FontFamily.Families)
                {
                    if (strFontName == ff.Name)
                    {
                        // the font name is valid so go to work.
                        bValidFontName = true;
                        break;
                    }
                }
            }

            if (!bValidFontName)
            {
                strFontName = "Arial";
            }
            FontFamily fontFamily = new FontFamily(strFontName);
            ftRetVal = new Font(
               fontFamily,
               nFontSize,
               FontStyle.Bold,
               GraphicsUnit.Pixel);

            return ftRetVal;
        }

        /// <summary>
        /// Creates From/Thru DateTime Controls on a ToolStrip. Must pass in two ToolStripControlHosts variables.
        /// Passing in the default datetimes is optional. If no value is passed then the 
        ///     From Date will be today minus two days and the
        ///     Thru Date will be today minus one day.
        /// 
        /// From the application call the three lines of code below.
        /// <code><example>
        ///     private void AddDateRangeControls()
        ///     {
        ///         RFClassLibrary.RFCObject.AddDateRangeControls(ref m_dpFrom, ref m_dpThru, "", "");
        ///         tsMain.Items.Insert(7, m_dpFrom);
        ///         tsMain.Items.Insert(9, m_dpThru);
        ///     }
        ///</example></code>
        /// 
        /// 09/21/2007 wdk
        /// </summary>
        /// <param name="dpFrom">The From date control defined in the application</param>
        /// <param name="dpThru">The Thru date control defined in the application</param>
        /// <param name="strFrom">Optional value for setting a date in the from control</param>
        /// <param name="strThru">Optional value for setting a date in the thru control</param>
        public static void CreateDateTimes(ref ToolStripControlHost dpFrom, ref ToolStripControlHost dpThru, string strFrom, string strThru)
        {
            // create the datetime controls for the From and Thru dates
            dpFrom = new ToolStripControlHost(new DateTimePicker());
            ((DateTimePicker)dpFrom.Control).Format = DateTimePickerFormat.Short;
            dpFrom.Control.Width = 95;

            dpThru = new ToolStripControlHost(new DateTimePicker());
            ((DateTimePicker)dpThru.Control).Format = DateTimePickerFormat.Short;
            dpThru.Control.Width = 95;


            // Check the from and thru dates passed to see if they are valid.
            DateTime dtResult;
            // FROM control set the value to the default
            dpFrom.Text = DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)).ToString("d");
            // if the value passed in is not empty try to use it. If it is not valid the default will be used
            if (strFrom.Length > 0) // we have some type of value
            {
                if (DateTime.TryParse(strFrom, out dtResult))
                {
                    dpFrom.Text = strFrom; // the value is a valid datetime use it.
                }
            }

            // THRU control set the value to the default
            dpThru.Text = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("d");
            // if the value passed in is not empty try to use it. If it is not valid the default will be used
            if (strThru.Length > 0) // we have some type of value
            {
                if (DateTime.TryParse(strThru, out dtResult))
                {
                    dpThru.Text = strThru; // the value is a valid datetime use it.
                }
            }

            //dpFrom.Control.Refresh();
            //dpFrom.Invalidate();
            //tsMain.Items.Insert(7, dpFrom);

            //            dpThru.Text = DateTime.Now.Subtract(new TimeSpan(DateTime.Now.Day, 0, 0, 0)).ToString("d");// "01/10/2007";
            //dpThru.Control.Refresh();
            //dpThru.Invalidate();
            // tsMain.Items.Insert(9, dpThru);
            //  tsMain.Refresh();
        }
    }
}
