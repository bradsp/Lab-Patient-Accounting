/*'
 * ET 07/23/2003 Rick Crone
 * Expired time class
 * 
 * Hist:
 *  07/12/2005 Rick Crone
 *		Added CLSCompliant.
 * 
 *	11/16/2004 Rick Crone
 *		Rewritten to use timer to avoid issues with the
 *		PC clock being set.
*/
using System;
[assembly: CLSCompliant(true)]
namespace RFClassLibrary { }


namespace RFClassLibrary
{

    /// <summary>
    /// ET Elasped Time
    /// </summary>
    public class ElapsedTime
    {
        //private DateTime ExpirationTime;
        private System.Timers.Timer timer1;
        private bool bExpired;

        /// <summary>
        /// contructor
        /// 07/23/2003 Rick Crone
        /// mod 11/16/2004 rgc
        /// </summary>
        /// <param name="sec"></param>
        public ElapsedTime(double sec)
        {
            bExpired = false;
            timer1 = new System.Timers.Timer();
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(TimeElasped);
            Reset(sec);
        }

        /// <summary>
        /// Set the expiration time
        /// Rick Crone
        /// </summary>
        public void Reset(double sec)
        {
            bExpired = false;
            timer1.Enabled = false;
            timer1.Interval = (sec * 1000); // milliseconds
            timer1.Enabled = true;

        }

        /// <summary>
        /// Check to see if timer has elapsed.
        /// Rick Crone
        /// </summary>
        /// <returns></returns>
        public bool IsExpired()
        {
            return (bExpired);
        }

        /// <summary>
        /// 11/16/2004 rgc
        /// </summary>
        /// void System.Timers.ElapsedEventHandler(object, System.Timers.ElapsedEventArgs)'
        private void TimeElasped(object sender, System.Timers.ElapsedEventArgs e)
        {
            bExpired = true;
        }

        /// <summary>
        /// Set the timer in you app and call wait. the number of seconds to wait are set in
        /// the app initialization of the timer.
        /// wdk 10/12/2006 
        /// </summary>
        public void Wait()
        {
            // throw new System.NotImplementedException();
            while (!IsExpired())
            {

            }
        }
        /// <summary>
        /// 04/01/2003 Rick Crone - wait(milliseconds) 1000 = 1 sec
        /// </summary>
        public static void sWait(int milliseconds)
        {
            //string msg = string.Format("Entering wait({0})", milliseconds);
            //mlf.WriteLogFile(msg);
            System.Threading.Thread.Sleep(milliseconds);
            //msg = string.Format("Leaving wait({0})", milliseconds);
            //mlf.WriteLogFile(msg);
        }
    }
}
