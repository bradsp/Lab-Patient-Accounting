// programmer added
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;

namespace Utilities
{
    /// <summary>
    /// calculates the .Duration() between .Start() and .Stop()
    /// </summary>
    public class HiPerfTimer
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(
            out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(
            out long lpFrequency);

        private long startTime, stopTime;
        private long freq;

        /// <summary>
        /// Constructor
        /// </summary>
        public HiPerfTimer()
        {
            startTime = 0;
            stopTime = 0;

            if (QueryPerformanceFrequency(out freq) == false)
            {
                // high-performance counter not supported
                throw new Win32Exception();
            }
        }

        /// <summary>
        /// Starts the timeer
        /// </summary>
        public void Start()
        {
            // lets do the waiting threads there work
            Thread.Sleep(0);

            QueryPerformanceCounter(out startTime);
        }

        /// <summary>
        /// Stop the timer
        /// </summary>
        public void Stop()
        {
            QueryPerformanceCounter(out stopTime);
        }

        /// <summary>
        /// Returns the duration of the timer (in seconds)
        /// </summary>
        public double Duration
        {
            get
            {
                return (double)(stopTime - startTime) / (double)freq;
            }
        }

        /// <summary>
        /// Returns the duration of the timer as a formatted stirng (in hours, minutes, seconds)
        /// </summary>
        public string DurationToString
        {
            get
            {
                //string strRetVal = null;
                double dSec = (double)(stopTime - startTime) / (double)freq;
                double dHour = dSec / 3600;
                double dMin = (dSec % 3600) / 60;
                dSec = ((dSec % 3600) % 60);
                return string.Format("{0}Minutes {1} and Seconds {2}",
                    double.Parse(dHour.ToString("F0")) > 0 ? string.Format("Hours {0} ", dHour.ToString("F")) : "",
                    dMin.ToString("F0"), dSec.ToString("F2"));
            }

        }

    }
}
