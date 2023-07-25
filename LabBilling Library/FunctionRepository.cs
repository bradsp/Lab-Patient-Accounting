using System;
using System.Text;
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

            if(year < DateTime.Now.Year - 20)
            {
                //likely an invalid year, return current year
                return DateTime.Now.Year.ToString();
            }

            return transDate.Month >= 10 ? (year+1).ToString() : year.ToString();

        }

        public static string GetArgs(this IDatabase db)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var arg in db.LastArgs)
            {
                sb.Append(arg.ToString() + "|");
            }

            return sb.ToString();
        }

    }
}
