﻿using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco;
using System;
using Microsoft.Data.SqlClient;
using System.Data;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class NumberRepository : RepositoryBase<Number>
    {
        public NumberRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {

        }

        public decimal GetNumber(string key)
        {
            Log.Instance.Debug($"Entering");

            //string sql = "exec GetNextNumber @key";
            //var param = new Microsoft.Data.SqlClient.SqlParameter("keyfield", keyfield);
            var number = new SqlParameter("@NextSequence", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var s = Sql.Builder.Append(";EXEC GetNextNumber @0, @1 OUTPUT", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = key }, 
                number);

            var result = Context.Execute(s);

            int NumberReturn = (int)number.Value;

            return NumberReturn;
        }
    }
}
