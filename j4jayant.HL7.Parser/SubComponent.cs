using System;

namespace j4jayant.HL7.Parser;

public class SubComponent
{
    private String _Value;

    public SubComponent()
    {
    }

    public SubComponent(String pValue)
    {
        _Value = pValue;
    }

    public String Value
    {
        get
        {
            if (_Value == null)
                return String.Empty;
            else
                return _Value;
        }
        set
        {
            _Value = value;

        }
    }

}
