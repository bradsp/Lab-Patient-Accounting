using System.Collections.Generic;

namespace LabBilling.Core.Models;

public class ChrgChrgDetailRelator
{
    public Chrg current;

    public Chrg MapIt(Chrg chrg, ChrgDetail a)
    {
        if (chrg == null)
            return current;

        if(current != null && current.ChrgId == chrg.ChrgId)
        {
            current.ChrgDetails.Add(a);

            return null;
        }

        var prev = current;

        current = chrg;
        current.ChrgDetails = new List<ChrgDetail>
        {
            a
        };

        return prev;
    }
}
