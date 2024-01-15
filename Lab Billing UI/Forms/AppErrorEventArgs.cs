using System;

namespace LabBilling.Forms
{
    public class AppErrorEventArgs : EventArgs
    {
        public string ErrorMessage { get; set; }
        public ErrorLevelType ErrorLevel { get; set; }

        public enum ErrorLevelType
        {
            Trace,
            Debug,
            Info,
            Warning,
            Error,
            Fatal
        }
    }

}
