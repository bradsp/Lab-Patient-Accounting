using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace RFClassLibrary
{
    /// <summary>
    /// wdk 20120106 Returns .Net Exceptions from COM HResults
    /// </summary>
    public class NoAccessException : ApplicationException
    {
        NoAccessException()
        {
        
        }

        void ConvertToException(int hr)
        {
            Marshal.ThrowExceptionForHR(hr);
        }

    }
}
