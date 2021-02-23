using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PetaPoco.Providers
{
    public class CustomSqlServerDatabaseProvider : SqlServerDatabaseProvider
    {
        //public override object ExecuteInsert(Database db, IDbCommand cmd, string primaryKeyName)
        //{
        //    cmd.CommandText = ";\nSELECT SCOPE_IDENTITY();";
        //    return base.ExecuteScalarHelper(db, cmd);
        //}

        public override string GetInsertOutputClause(string primaryKeyName)
            => string.Empty;

    }
}
