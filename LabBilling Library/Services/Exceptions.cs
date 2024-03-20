using LabBilling.Core.Models;
using System;

namespace LabBilling.Core.Services;

[Serializable]
public class DiscountRangeNotSupportedException : ApplicationException
{
    public DiscountRangeNotSupportedException() : base("EndCdm must equal Cdm. Cdm range discounts are no longer supported.")
    { }
}

[Serializable]
public class CdmNotFoundException : ApplicationException
{
    public string Cdm { get; }

    public CdmNotFoundException() { }

    public CdmNotFoundException(string message) : base(message) { }

    public CdmNotFoundException(string message, Exception inner)
        : base (message, inner) { }

    public CdmNotFoundException(string message, string cdm)
        : this(message)
    {
        Cdm = cdm;
    }
}

[Serializable]
public class InvalidClientException : ApplicationException
{
    public string ClientMnem { get; }

    public InvalidClientException() { }

    public InvalidClientException(string message) : base(message) { }

    public InvalidClientException(string message, Exception inner)
        : base(message, inner) { }

    public InvalidClientException(string message, string clientMnem)
        : this (message)
    {
        ClientMnem = clientMnem;
    }
}

[Serializable]
public class AccountNotFoundException : ApplicationException
{
    public string AccountNumber { get; }

    public AccountNotFoundException() { }

    public AccountNotFoundException(string message) : base(message) { }

    public AccountNotFoundException(string message, Exception inner)
        : base(message, inner) { }

    public AccountNotFoundException(string message, string accountNumber)
        : this(message)
    {
        AccountNumber = accountNumber;
    }
}

[Serializable]
public class AccountLockException : ApplicationException
{
    public AccountLock LockInfo { get; }

    public AccountLockException() { }

    public AccountLockException(AccountLock lockInfo, string message = "Exception acquiring account lock") : base(message)
    {
        this.LockInfo = lockInfo;        
    }


}

[Serializable]
public class InvalidParameterValueException : ApplicationException
{
    public string ParameterName { get; }

    public InvalidParameterValueException() { }

    public InvalidParameterValueException(string message)
        : base(message) { }

    public InvalidParameterValueException(string parameterName, Exception inner)
        : base(parameterName, inner) { }

    public InvalidParameterValueException(string message, string parameterName)
        : this(message)
    {
        ParameterName = parameterName;         
    }
}

[Serializable]
public class PatientNameParseException : ApplicationException
{
    public string PatientName { get; }
    public string Account { get; }

    public PatientNameParseException() { }

    public PatientNameParseException(string message)
        : base(message) { }

    public PatientNameParseException(string patientName, Exception inner)
        : base(patientName, inner) { }

    public PatientNameParseException(string message, string patientName, string account)
        : this(message)
    {
        PatientName = patientName;
        Account = account;
    }
}

public class RuleProcessException : ApplicationException
{
    public string RuleName { get; }

    public RuleProcessException() { }

    public RuleProcessException(string message, Exception innerException)
        : base(message, innerException) { }

    public RuleProcessException(string message, string ruleName)
        : base(message)
    {
        RuleName = ruleName;
    }
}
