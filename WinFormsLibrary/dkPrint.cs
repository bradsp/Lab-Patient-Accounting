// programmer added
using System.Drawing.Imaging; // for pixelformat
using System.Drawing.Printing;

namespace WinFormsLibrary;

/// <summary>
/// Print class used for
///     1. printing the screen without having to paste into another program
///     2. PrintPage Event handler for printing text from an application.
/// </summary>
public static class dkPrint
{
    /// <summary>
    /// List of possible values for printing from a document/screen (monitor(s))
    ///     PrimaryScreen = 0   -- If the computer has dual screens this retrieves the bounds of the screen 
    ///                             that has been set in the Display Properties as the primary screen.
    ///     CombinedScreens = 1 -- If the computer has dual screens this retrieves the bounds of both screens attached
    ///                             to the computer. If the computer has one screen it retrieves the bounds of 
    ///                             single screen.
    ///      WorkingArea = 2    -- Currently retrieves the same area as PrimaryScreen.
    ///      BothScreens = 3    -- Needs Work
    ///      Form = 4           -- Retrieves the currents windows bounds only. Called from an application it
    ///                             will only print the applications window.
    /// 
    /// 
    /// <code> Example of getting the string representation of an enum
    ///	foreach (string s in Enum.GetNames(typeof(CaptureType)))
    /// {
    ///     MessageBox.Show(s); // s will contain "PrimaryScreen" the first time and "Form" the last time.
    /// }
    /// 
    /// </code>
    /// 
    /// <code> Example of how to use.
    ///     RFClassLibrary.dkPrint.CaptureType ct; //Create an instance of the CaptureType.
    ///     // Cast in the example below the tag of the menu item's string (will be "Form", or "BothScreens")
    ///     // That has been parsed from the type of CaptureType via the static Enum function Parse.
    ///     ct = (RFClassLibrary.dkPrint.CaptureType)Enum.Parse(typeof(RFClassLibrary.dkPrint.CaptureType), e.ClickedItem.Tag.ToString());  // Gets the capture type from the clicked items tag.
    ///     Bitmap[] bmps = RFClassLibrary.dkPrint.Capture(ct);
    ///    </code>
    /// </summary>
    public enum CaptureType
    { ///<summary></summary>
        PrimaryScreen,
        ///<summary></summary>
        CombinedScreens,
        ///<summary></summary>
        WorkingArea,
        ///<summary></summary>
        BothScreens,
        ///<summary></summary>
        Form
    };
    private static StreamReader streamToPrint;

    /// <summary>
    /// 
    /// </summary>
    public static StreamReader propStreamToPrint
    {
        get { return streamToPrint; }
        set { streamToPrint = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public static Bitmap m_memoryImage;

    /// <summary>
    /// 
    /// </summary>
    public static Bitmap[] images;
    /// <summary>
    /// This function captures the image from the monitor(s) (Screen(s)) attached to the computer
    /// To capture the form use the code in formToolStripMenuItem_Click() SendKeys.Send...
    /// </summary>
    /// <param name="typeOfCapture"></param>
    /// <returns></returns>
    public static Bitmap[] Capture(CaptureType typeOfCapture)
    {
        // used to capture then screen in <A class=iAs style="FONT-WEIGHT: normal; FONT-SIZE: 100%; PADDING-BOTTOM: 1px; COLOR: darkgreen; BORDER-BOTTOM: darkgreen 0.07em solid; BACKGROUND-COLOR: transparent; TEXT-DECORATION: underline" href="#" target=_blank itxtdid="4379950">memory</A>

        // number of screens to capture,
        // will be updated below if necessary
        int count = 1;

        try
        {
            Screen[] screens = Screen.AllScreens;
            Rectangle rc;

            // setup the area to capture
            // depending on the supplied parameter
            switch (typeOfCapture)
            {
                case CaptureType.Form:
                    //rc = this.Bounds;
                    //MessageBox.Show(string.Format("{0}",Application.OpenForms.Count));
                    rc = Application.OpenForms[0].Bounds;
                    break;
                case CaptureType.PrimaryScreen:
                    rc = Screen.PrimaryScreen.Bounds;
                    break;
                case CaptureType.CombinedScreens:
                    rc = SystemInformation.VirtualScreen;
                    break;
                case CaptureType.WorkingArea:
                    rc = Screen.PrimaryScreen.WorkingArea;
                    break;
                case CaptureType.BothScreens:
                    count = screens.Length;
                    typeOfCapture = CaptureType.WorkingArea;
                    rc = screens[0].WorkingArea;
                    break;
                default:
                    rc = SystemInformation.VirtualScreen;
                    break;
            }
            // allocate a member for saving the captured image(s)
            images = new Bitmap[count];

            // cycle across all desired screens
            for (int index = 0; index < count; index++)
            {
                if (index > 0)
                    rc = screens[index].WorkingArea;
                // redefine the size on multiple screens

                m_memoryImage = new Bitmap(rc.Width, rc.Height,
                              PixelFormat.Format32bppArgb);
                using (Graphics memoryGrahics =
                        Graphics.FromImage(m_memoryImage))
                {
                    // copy the screen data
                    // to the memory allocated above
                    memoryGrahics.CopyFromScreen(rc.X, rc.Y,
                       0, 0, rc.Size, CopyPixelOperation.SourceCopy);
                }
                images[index] = m_memoryImage;
                // save it in the class member for later use
            }
        }
        catch (Exception ex)
        {
            // handle any erros which occured during capture
            MessageBox.Show(ex.ToString(), "Capture failed",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        return images;
    }

    /// <summary>
    /// This function prints graphic objects. To print text call PrintText_PrintPage()
    /// Can be used instead of having to create a PrintPage event Handler in the application.
    /// /// <code> <example>
    ///     PrintDocument printDoc = new PrintDocument();
    ///     printDoc.PrintPage += new PrintPageEventHandler(RFClassLibrary.dkPrint.PrintGraphic_PrintPage);
    /// </example></code>
    /// 09/21/2007 wdk
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public static void PrintGraphic_PrintPage(object sender, PrintPageEventArgs e)
    {
        if (m_memoryImage != null)
        {
            Bitmap page;
            if (m_memoryImage.Height < e.PageBounds.Height && m_memoryImage.Width < e.PageBounds.Width)
            {
                page = m_memoryImage;
            }
            else
            {
                page = new Bitmap(m_memoryImage, e.PageBounds.Width, e.PageBounds.Height);
            }
            e.Graphics.DrawImage(m_memoryImage, 0, 0);
        }
    }

    /// <summary>
    /// This function prints text objects. To print graphics call PrintGraphic_PrintPage
    /// Can be used instead of having to create a PrintPage event Handler in the application.
    /// 
    /// <code> <example>
    ///     PrintDocument printDoc = new PrintDocument();
    ///     printDoc.PrintPage += new PrintPageEventHandler(RFClassLibrary.dkPrint.PrintText_PrintPage);
    /// </example></code>
    /// 
    /// 09/21/2007 wdk
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
    public static void PrintText_PrintPage(object sender, PrintPageEventArgs ev)
    {
        Font printFont = new Font("Arial", 10);
        float linesPerPage = 0;
        float yPos = 0;
        int count = 0;
        float leftMargin = ev.MarginBounds.Left;
        float topMargin = ev.MarginBounds.Top;
        string line = null;

        // Calculate the number of lines per page.
        linesPerPage = ev.MarginBounds.Height /
           printFont.GetHeight(ev.Graphics);

        // Print each line of the file.
        while (count < linesPerPage &&
           (line = streamToPrint.ReadLine()) != null)
        {
            yPos = topMargin + count *
               printFont.GetHeight(ev.Graphics);
            ev.Graphics.DrawString(line, printFont, Brushes.Black,
               leftMargin, yPos, new StringFormat());
            count++;
        }

        // If more lines exist, print another page.
        if (line != null)
            ev.HasMorePages = true;
        else
            ev.HasMorePages = false;
    }


} // don't type below this line.
