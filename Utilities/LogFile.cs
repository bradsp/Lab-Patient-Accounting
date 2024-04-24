using System;
// added
using System.IO; // FileStream and StreamWriter


namespace Utilities;

/// <summary>
/// Summary description for LogFile.
/// 07/10/03 Rick Crone RFClassLibrary's LogFile class.
/// 
/// LogFile constructor takes two parameters.
/// The first is the name of the file to write log entries to.
/// The other is the name of a file to save any file with the
/// name passed in as the first parameter to.
/// 
/// Pass an empty string as the second parameter if you do not
/// wish to save the file.
/// 
/// 
/// Useage example:
/// <code>
/// LogFile lf;
///	lf = new LogFile("C:\\TEMP\\TEST.txt","C:\\TEMP\\SAVETEST.txt");
///	if(lf.fs == null)
///	{
///		Console.Write("failed");
///		return;
///	}
///					
///	if(!lf.WriteLogFile("Hello LogFile!"))
///		{
///		Console.Write("failed");
///		return;
///		}
/// </code>
/// 
/// mod hist:
///		06/16/2005 Rick Crone
///			These mods are two allow the log files to be used
///			as they have been by one process or with these new
///			mods with multiple processes (multiple copies for the
///			application accessing the same file). This was done
///			for FordMon. Send an empty string as the second parameter
///			when using this class for muliple process access: No
///			copy is made (same file is used - appended). 
///		
///			Now if there is no second parm the file is opened 
///			FileMode.OpenOrCreate instead of FileMode.Create
///			and there is a seek to the end of the file. Also
///			there is a seek to the end of the file before each
///			write.
///			
/// 
///		11/12/2003 Rick Crone
///			A mod in WriteLogFile() adding try and catch
///			to help trap errors.
///			
/// </summary>
public class LogFile : RFCObject
{
    /// <summary>
    /// wdk 20100518 added size
    /// </summary>
    public long m_lFileSize;
    /// the file stream
    public FileStream fs;
    static StreamWriter sw;
    /// <summary>
    /// Stream reader for the Log file.
    /// </summary>
    public StreamReader m_srLog;
    string m_strMsgPrefix;

    /// <summary>
    /// 06/17/2008 rgc/wdk modified to check for the file name reguardless of the directory
    /// </summary>
    /// <param name="strFileName"></param>
    /// <param name="strSaveName"></param>
    /// <returns></returns>
    private string CheckSaveName(string strFileName, string strSaveName)
    {
        // 06/17/2008 rgc/wdk allows SaveFile to be in different directory and add timestamp to savefile name.
        if (strSaveName.Contains(strFileName.Substring(strFileName.LastIndexOf("\\"))))
        //if(strFileName == strSaveName)  
        {
            // add the date time to the end of the save file name

            strSaveName = strSaveName.Replace(".txt", "");
            strSaveName = strSaveName.Replace(".TXT", "");
            strSaveName = string.Format("{0}{1}.txt", strSaveName, Time.HL7TimeStamp());
        }
        return (strSaveName);
    }


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="strFileName"></param>
    /// <param name="strSaveName"></param>
    /// <param name="strMsgPrefix"></param>
    public LogFile(string strFileName, string strSaveName, string strMsgPrefix)
    {
        strSaveName = CheckSaveName(strFileName, strSaveName);
        m_strMsgPrefix = strMsgPrefix;

        if (strSaveName.Length > 0)
        {
#pragma warning disable 168
            // try to copy the file 
            try
            {
                File.Copy(strFileName, strSaveName);
            }
            catch (Exception excpt)
            {
                // ignore error - file might not exist
            }
#pragma warning restore 168


            //fs = new FileStream("C:\\TEMP\\TEST.txt", FileMode.Create, FileAccess.Write);
#pragma warning disable 168
            try
            {
                // 09/30/2008 wdk changed FileAccess from Write to ReadWrite to allow reading from the file
                // while it is open.
                fs = new FileStream(strFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Seek(0, SeekOrigin.End);
            }
            catch (Exception excpt)
            {
                // failed to create log file
                // fs will be null
                // calling application can check this 
                return;

            }
#pragma warning restore 168
        }
        else
        {

            //fs = new FileStream("C:\\TEMP\\TEST.txt", FileMode.Create, FileAccess.Write);
#pragma warning disable 168
            try
            {
                fs = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                fs.Seek(0, SeekOrigin.End);
            }
            catch (Exception excpt)
            {
                // failed to create log file
                // fs will be null
                // calling application can check this 
                return;

            }
#pragma warning restore 168
        }

        sw = new StreamWriter(fs);
        string strMsg;
        strMsg = string.Format("Parms to LogFile() are [{0}],[{1}]",
            strFileName,
            strSaveName);
        sw.WriteLine(strMsg);

        // 06/24/2008 rgc/wdk added new functionallity for StreamReader to use when printing.
        m_srLog = new StreamReader(fs);


    }

    /// <summary>
    /// construtor
    /// </summary>
    /// <param name="strFileName"></param>
    /// <param name="strSaveName"></param>
    public LogFile(string strFileName, string strSaveName)
    {
        //
        // TODO: Add constructor logic here
        //
        strSaveName = CheckSaveName(strFileName, strSaveName);
        if (strSaveName.Length > 0)
        {
#pragma warning disable 168
            // try to copy the file 
            try
            {
                File.Copy(strFileName, strSaveName);
            }
            catch (Exception excpt)
            {
                // ignore error - file might not exist
            }
#pragma warning restore 168

            //fs = new FileStream("C:\\TEMP\\TEST.txt", FileMode.Create, FileAccess.Write);
#pragma warning disable 168
            try
            {
                fs = new FileStream(strFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Seek(0, SeekOrigin.End);
            }
            catch (Exception excpt)
            {
                // failed to create log file
                // fs will be null
                // calling application can check this 
                return;

            }
#pragma warning restore 168
        }
        else
        {

            //fs = new FileStream("C:\\TEMP\\TEST.txt", FileMode.Create, FileAccess.Write);
#pragma warning disable 168
            try
            {
                fs = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Seek(0, SeekOrigin.End);
            }
            catch (Exception excpt)
            {
                // failed to create log file
                // fs will be null
                // calling application can check this 
                return;

            }
#pragma warning restore 168
        }


        sw = new StreamWriter(fs);
        string strMsg;
        strMsg = string.Format("Program {0} built on {1} started this Log file.\r\nParms to LogFile() are \r\n\t[{2}],\r\n\t[{3}]",
            this.propAppName,
        this.propAppBuildDate,
        strFileName,
            strSaveName);
        sw.WriteLine(strMsg);

        // 06/24/2008 rgc/wdk added new functionallity for StreamReader to use when printing.
        m_srLog = new StreamReader(fs);

    } // end of constructor

    /// <summary>
    /// 03/26/03 Rick Crone
    /// The calling application should check the
    /// return value of WriteLogFile(). If not it
    /// may not write and the application will NOT
    /// know it!
    /// 
    /// </summary>
    public bool WriteLogFile(string strLine)
    {
        if (fs == null)
        {
            // file is not open
            return false;
        }

        DateTime now = DateTime.Now;
#pragma warning disable 168
        try
        {
            fs.Seek(0, SeekOrigin.End);// 06/16/2005 rgc

            sw.Write(m_strMsgPrefix);
            sw.Write(now.ToString(" MM/dd/yy HH:mm:ss:ffff"));
            sw.Write(" - ");
            sw.Write(strLine);
            sw.Write("\r\n");
            sw.Flush();
            m_lFileSize = fs.Length;
            //fs.Close(); // 06/24/2008 rgc/wdk close the File Stream ????

        }
        catch (Exception)
        {
            // can't write to file?
            //strStatus = string.Format("{0} Exception caught.", excpt);
            return false;
        }
#pragma warning restore 168

        return true;
    }
}
