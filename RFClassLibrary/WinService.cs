using System;
// added
using System.ServiceProcess; // ServiceStatus()

namespace RFClassLibrary
{
    /// <summary>
    /// Summary description for WinService.
    /// Once trick pony - Member function Status()
    /// 03/27/03 Rick Crone - Pass a Windows service name,
    ///  the target machine the service should be running on
    ///  and this function returns the status of that service.
	/// </summary>
	public class WinService
	{
		/// <summary>
		/// Empty constructor
		/// </summary>
        public WinService()
		{
			
		}

		/// <summary>
		/// 03/27/03 Rick Crone - Pass a Windows service name, the target machine the service should be running on and this function returns the status of that service.
		/// </summary>
		public string Status(string strServiceName, string strTargetMachine)
		{
			string strStatus = string.Format("Failed to get status for {0} on {1}",
												strServiceName,
												strTargetMachine);
			ServiceController sc;
			try
			{
				sc = new ServiceController(strServiceName);
				sc.MachineName = strTargetMachine;
			}
			catch (Exception excpt)
			{
				
				strStatus = string.Format("{0} Exception caught.", excpt);
				//MessageBox.Show(strMsg);
				return strStatus;
			}
			
			try
			{
				strStatus = sc.Status.ToString();
			}
			catch (Exception excpt)
			{
				
				strStatus = string.Format("{0} Exception caught.", excpt);
				//MessageBox.Show(strMsg);
				return strStatus;
			}

			return strStatus;
		}
		

	}
}
