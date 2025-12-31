using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LabBilling.Core.Services;

/// <summary>
/// Helper class to send raw data directly to a printer using Win32 APIs.
/// Optimized for dot-matrix and pin-fed forms that require PCL5 commands.
/// </summary>
public static class RawPrinterHelper
{
    // Structure and API declarations for Win32 printer functions
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class DOCINFOA
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string pDocName;
        [MarshalAs(UnmanagedType.LPStr)]
        public string pOutputFile;
        [MarshalAs(UnmanagedType.LPStr)]
        public string pDataType;
    }

    [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

    [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern bool ClosePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

    [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern bool EndDocPrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern bool StartPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern bool EndPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

    /// <summary>
    /// Sends raw data (PCL5 commands, text, etc.) directly to the specified printer.
    /// Bypasses Windows GDI for direct hardware control.
    /// </summary>
    /// <param name="printerName">Name of the printer as it appears in Windows</param>
    /// <param name="dataToSend">Raw byte array containing PCL5 commands and data</param>
    /// <param name="documentName">Name of the print job (appears in print queue)</param>
    /// <returns>True if successful, false otherwise</returns>
    public static bool SendBytesToPrinter(string printerName, byte[] dataToSend, string documentName = "Raw Print Job")
    {
        IntPtr hPrinter = IntPtr.Zero;
        DOCINFOA di = new DOCINFOA();
        bool success = false;

        di.pDocName = documentName;
        di.pDataType = "RAW"; // RAW data type bypasses Windows print processor

        try
        {
            // Open the printer
            if (!OpenPrinter(printerName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                return false;
            }

            // Start a document
            if (!StartDocPrinter(hPrinter, 1, di))
            {
                return false;
            }

            // Start a page
            if (!StartPagePrinter(hPrinter))
            {
                return false;
            }

            // Write the data
            IntPtr pUnmanagedBytes = Marshal.AllocCoTaskMem(dataToSend.Length);
            Marshal.Copy(dataToSend, 0, pUnmanagedBytes, dataToSend.Length);

            int bytesWritten = 0;
            success = WritePrinter(hPrinter, pUnmanagedBytes, dataToSend.Length, out bytesWritten);

            Marshal.FreeCoTaskMem(pUnmanagedBytes);

            // End the page
            EndPagePrinter(hPrinter);

            // End the document
            EndDocPrinter(hPrinter);
        }
        finally
        {
            // Close the printer handle
            if (hPrinter != IntPtr.Zero)
            {
                ClosePrinter(hPrinter);
            }
        }

        return success;
    }

    /// <summary>
    /// Sends a string directly to the printer.
    /// Converts string to bytes using specified encoding (default is ASCII for dot-matrix compatibility).
    /// </summary>
    /// <param name="printerName">Name of the printer</param>
    /// <param name="stringToSend">String data to send</param>
    /// <param name="documentName">Document name for print queue</param>
    /// <param name="encoding">Text encoding (default ASCII for maximum dot-matrix compatibility)</param>
    /// <returns>True if successful</returns>
    public static bool SendStringToPrinter(string printerName, string stringToSend, string documentName = "Text Print Job", Encoding encoding = null)
    {
        encoding ??= Encoding.ASCII; // Default to ASCII for dot-matrix printers
        byte[] bytes = encoding.GetBytes(stringToSend);
        return SendBytesToPrinter(printerName, bytes, documentName);
    }

    /// <summary>
    /// Gets the last Win32 error message for debugging printer communication issues.
    /// </summary>
    /// <returns>Error message string</returns>
    public static string GetLastErrorMessage()
    {
        int errorCode = Marshal.GetLastWin32Error();
        return $"Error Code {errorCode}: {new System.ComponentModel.Win32Exception(errorCode).Message}";
    }
}
