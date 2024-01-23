using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities;

namespace PetaPoco.Providers
{
    public sealed class CustomSqlMsDatabaseProvider : SqlServerMsDataDatabaseProvider
    {
        private string GetInsertPreamble(string primaryKeyName)
        {
            return string.Format("DECLARE @result TABLE({0} sql_variant); ", primaryKeyName);
        }

        private string GetInsertOutputClauseWithInsertIntoResult(string primaryKeyName)
        {
            return string.Format(" OUTPUT INSERTED.[{0}] into @result({0})", primaryKeyName);
        }

        private string GetInsertPostScript(string primaryKeyName)
        {
            return string.Format("; SELECT {0} FROM @result; ", primaryKeyName);
        }

        private void ApplyWorkaroundForSQLError334(string primaryKeyName, ref IDbCommand cmd)
        {
            StringBuilder newCmdText = new StringBuilder();
            if (cmd.CommandText.ContainsCaseInsensitive($" OUTPUT INSERTED.[{primaryKeyName}]"))
            {
                //Amend the the original PetaPoco INSERT statement to replace the original " OUTPUT INSERTED.XXX" part with one that inserts the "OUTPUT INSERTED.[XXX]" value
                //into a @result table variable so the PK value can be selected out at the end as PetPoco expects.
                newCmdText.AppendLine(GetInsertPreamble(primaryKeyName));
                newCmdText.AppendLine(
                    cmd.CommandText.Replace(
                        $" OUTPUT INSERTED.[{primaryKeyName}]",
                        GetInsertOutputClauseWithInsertIntoResult(primaryKeyName)
                    )
                );
                newCmdText.AppendLine(GetInsertPostScript(primaryKeyName));
                cmd.CommandText = newCmdText.ToString();
            }
        }

        public override object ExecuteInsert(Database db, IDbCommand cmd, string primaryKeyName)
        {
            ApplyWorkaroundForSQLError334(primaryKeyName, ref cmd);

            //Call the base method to continue with the PetaPoco insert as expected
            return base.ExecuteInsert(db, cmd, primaryKeyName);
        }

        public override Task<object> ExecuteInsertAsync(CancellationToken cancellationToken, Database db, IDbCommand cmd, string primaryKeyName)
        {
            ApplyWorkaroundForSQLError334(primaryKeyName, ref cmd);

            //Call the base method to continue with the PetaPoco insert as expected
            return base.ExecuteInsertAsync(cancellationToken, db, cmd, primaryKeyName);
        }
    }
}
