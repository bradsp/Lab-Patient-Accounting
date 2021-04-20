using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core
{

    [Serializable]
    public class CdmNotFoundException : Exception
    {
        public string Cdm { get; }

        public CdmNotFoundException()
        {

        }

        public CdmNotFoundException(string message) : base(message)
        {

        }

        public CdmNotFoundException(string message, Exception inner)
            : base (message, inner)
        {

        }

        public CdmNotFoundException(string message, string cdm)
            : this(message)
        {
            Cdm = cdm;
        }

    }

    [Serializable]
    public class AccountNotFoundException : Exception
    {
        public string AccountNumber { get; }

        public AccountNotFoundException()
        {
        
        }

        public AccountNotFoundException(string message) : base(message)
        {

        }

        public AccountNotFoundException(string message, Exception inner) 
            : base(message, inner)
        {

        }

        public AccountNotFoundException(string message, string accountNumber)
            : this(message)
        {
            AccountNumber = accountNumber;
        }
    }

    [Serializable]
    public class InvalidParameterValueException : Exception
    {
        public string ParameterName { get; }

        public InvalidParameterValueException()
        {

        }

        public InvalidParameterValueException(string message)
            : base(message)
        {

        }

        public InvalidParameterValueException(string parameterName, Exception inner)
            : base(parameterName, inner)
        {

        }

        public InvalidParameterValueException(string message, string parameterName)
            : this(message)
        {
            ParameterName = parameterName;         
        }
    }

}
