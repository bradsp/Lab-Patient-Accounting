using System;
using System.Collections.Generic;
using System.Text;
// programmer added
using System.Windows.Forms;
using System.Runtime.InteropServices; // for PrintDialog

namespace RFClassLibrary
{

    /// <summary>
    /// rgc/wdk 20090422 this class is to duplicate the capabilites that exist in RFC library in the CRawPrn class.
    /// </summary>
    public class RawPrn
    {
        IntPtr hPrn;
        object oNULL = null;
        /// <summary>
        /// rgc/wdk 20090423 Public member visible from RawPrn this is the name of default printers driver
        /// </summary>
        public string m_strDriverName = string.Empty;
        char[] m_chrPrinterName = new char[256];
        char[] m_chrDocName = new char[256];
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m_chrPrinterName"></param>
        /// <param name="hPrn"></param>
        /// <param name="oNULL"></param>
        /// <returns></returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto)]
        static extern bool OpenPrinter(char[] m_chrPrinterName, out IntPtr hPrn, object oNULL);
        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DRIVER_INFO_1
          {
            /// <summary>
            /// Actual driver name.
            /// </summary>
             [MarshalAs(UnmanagedType.LPTStr)]
             public string strDriverName;
          } 
       // [DllImport("winspool.drv", CharSet = CharSet.Auto)]
       // static extern bool GetPrinterDriver(IntPtr hPrn, string strnull, int level , ref DRIVER_INFO_1 pbyte, char[] buff, out int pcbNeeded);
  //      public class RawPrinterHelper
  //      {
  //          [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
  //          public class DRIVER_INFO_1
  //          {
  //             [MarshalAs(UnmanagedType.LPTStr)]
  //              public string strDriverName;
  //          }    
  //          // Structure and API declarions:
  //          [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
  //          public class DOCINFOA
  //          {
  //              [MarshalAs(UnmanagedType.LPStr)]
  //              public string pDocName;
  //              [MarshalAs(UnmanagedType.LPStr)]
  //              public string pOutputFile;
  //              [MarshalAs(UnmanagedType.LPStr)]
  //              public string pDataType;
  //          }
           


  //          //[DllImport("winspool.Drv", EntryPoint="OpenPrinterA", SetLastError=true, CharSet=CharSet.Ansi, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
  //          //public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

  //          //[DllImport("winspool.Drv", EntryPoint="ClosePrinter", SetLastError=true, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
  //          //public static extern bool ClosePrinter(IntPtr hPrinter);

          
  //          //[DllImport("winspool.Drv", EntryPoint="EndDocPrinter", SetLastError=true, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
  //          //public static extern bool EndDocPrinter(IntPtr hPrinter);

  //          //[DllImport("winspool.Drv", EntryPoint="StartPagePrinter", SetLastError=true, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
  //          //public static extern bool StartPagePrinter(IntPtr hPrinter);

  //          //[DllImport("winspool.Drv", EntryPoint="EndPagePrinter", SetLastError=true, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
  //          //public static extern bool EndPagePrinter(IntPtr hPrinter);

  //          //[DllImport("winspool.Drv", EntryPoint="WritePrinter", SetLastError=true, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
  //          //public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten );


  ////[DllImport("winspool.Drv", EntryPoint="StartDocPrinterA", SetLastError=true, CharSet=CharSet.Ansi, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
  //          //public static extern bool StartDocPrinter( IntPtr hPrinter, Int32 level,  
  //          //[In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

  //      }
        
        #region copied code not used
        //PrintDlg([In, Out] PRINTDLG lppd);
        //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        //[System.Runtime.InteropServices.ComVisible(false)]
        //internal class PRINTDLG
        //{
        //    public Int32 lStructSize;
        //    public IntPtr hwndOwner;
        //    public IntPtr hDevMode;
        //    public IntPtr hDevNames;
        //    public IntPtr hDC = IntPtr.Zero;
        //    public Int32 Flags;
        //    public Int16 FromPage = 0;
        //    public Int16 ToPage = 0;
        //    public Int16 MinPage = 0;
        //    public Int16 MaxPage = 0;
        //    public Int16 Copies = 0;
        //    public IntPtr hInstance = IntPtr.Zero;
        //    public IntPtr lCustData = IntPtr.Zero;
        //    public PrintHookProc lpfnPrintHook;
        //    public IntPtr lpfnSetupHook = IntPtr.Zero;
        //    public IntPtr lpPrintTemplateName = IntPtr.Zero;
        //    public IntPtr lpSetupTemplateName = IntPtr.Zero;
        //    public IntPtr hPrintTemplate = IntPtr.Zero;
        //    public IntPtr hSetupTemplate = IntPtr.Zero;
        //}
#endregion

        /// <summary>
        /// rgc/wdk 20090423 This class is designed to use the winspool.drv to get printer information for use in 
        /// our applications.
        /// </summary>
        public RawPrn()
        {
            m_chrDocName.Initialize();
            m_chrPrinterName.Initialize();
            m_strDriverName = string.Empty;
            PrintDialog pDialog = new PrintDialog();
            m_chrPrinterName = pDialog.PrinterSettings.PrinterName.ToCharArray(0,(pDialog.PrinterSettings.PrinterName.Length));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hPrn"></param>
        /// <param name="pEnvironment"></param>
        /// <param name="Level"></param>
        /// <param name="di1"></param>
        /// <param name="cbBuffer"></param>
        /// <param name="pcbNeeded"></param>
        /// <returns></returns>
        [DllImport("winspool.drv", EntryPoint = "GetPrinterDriverW", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetPrinterDriver(
            IntPtr hPrn,
                [MarshalAs(UnmanagedType.LPTStr)] string pEnvironment,
                    Int32 Level,
                        [Out, MarshalAs(UnmanagedType.LPStruct)] DRIVER_INFO_1 di1,
                            Int32 cbBuffer,
                                out Int32 pcbNeeded);
        /// <summary>
        /// rgc/wdk 20090323 Calls OpenPrinter [DLL call] and gets a pointer to the printer.
        /// Uses the pointer to the printer to set the driver name so its available for use.
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            if (!OpenPrinter(m_chrPrinterName, out hPrn, oNULL))
            {
                return false;
            }
            DRIVER_INFO_1 pDriverInfo = new DRIVER_INFO_1();
            Int32 bytesNeeded = new Int32();
            string strNull = string.Empty;
            Int32 m_chr = new Int32();
            m_chr = 1; // wdk 20090424 the System.ExecutionEngineException will be thrown by m_chr not being set to at least 1. The catch below will not catch the System.ExecutionEngineException.
            try
            {
                GetPrinterDriver(hPrn, strNull, 1, pDriverInfo, m_chr, out bytesNeeded);
            }
            catch (Exception) // see note on m_chr =1; 
            {
                m_chr = bytesNeeded + 1;
                GetPrinterDriver(hPrn, strNull, 1, pDriverInfo, m_chr, out bytesNeeded);
            }
            finally //catch (Exception e)
            {
                m_chr = bytesNeeded + 1;
                GetPrinterDriver(hPrn, strNull, 1, pDriverInfo, m_chr, out bytesNeeded);
            }
            m_strDriverName = pDriverInfo.strDriverName;

            return true;
        }
        /// <summary>
        /// Not implemented
        /// 
        /// </summary>
        public void Close()
        {
        }
    
    } // don't go below this line

}
