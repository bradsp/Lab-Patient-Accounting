using System;
using PetaPoco;
using PetaPoco.Providers;

namespace LabBilling
{
    /// <summary>
    /// Static utility functions for quick database queries. Most will return a scalar value.
    /// </summary>
    public static class FunctionRepository
    {

        public static string GetAMAYear(DateTime transDate)
        {
            int year = transDate.Year;

            return transDate.Month >= 10 ? (year+1).ToString() : year.ToString();

        }

    }
}
