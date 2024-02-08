using LabBilling.Core.Models;
using LabBilling.Logging;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace LabBilling.Core.DataAccess
{

    public sealed class PatientStatementRepository : RepositoryBase<PatientStatement>
    {
        private readonly PatientStatementAccountRepository accountRepository;
        private readonly PatientStatementEncounterActivityRepository encounterActivityRepository;
        private readonly PatientStatementEncounterRepository encounterRepository;

        public PatientStatementRepository(IAppEnvironment appEnvironment) : base(appEnvironment)  
        { 
            accountRepository = new PatientStatementAccountRepository(appEnvironment);
            encounterActivityRepository = new PatientStatementEncounterActivityRepository(appEnvironment);
            encounterRepository = new PatientStatementEncounterRepository(appEnvironment);
        }

        public List<PatientStatement> GetByAccount(string account)
        {
            Log.Instance.Trace("Entering");
            var sql = Sql.Builder
                .Select(GetRealColumn(nameof(PatientStatementAccount.StatementNumber)))
                .From(accountRepository.TableInfo.TableName)
                .Where($"{GetRealColumn(nameof(PatientStatementAccount), nameof(PatientStatementAccount.AccountId))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

            List<string> statements = dbConnection.Fetch<string>(sql);

            var records = GetByStatement(statements);

            foreach(var record in records)
            {
                record.Accounts = accountRepository.GetByStatement(record.StatementNumber);
                record.Encounters = encounterRepository.GetByStatement(record.StatementNumber);
                record.EncounterActivity = encounterActivityRepository.GetByStatement(record.StatementNumber);
            }

            return records;
        }

        public PatientStatement GetByStatement(long statementNo)
        {
            Log.Instance.Trace("Entering");
            var sql = Sql.Builder
                .Where($"{GetRealColumn(nameof(PatientStatement.StatementNumber))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.BigInt, Value = statementNo });

            var result = dbConnection.SingleOrDefault<PatientStatement>(sql);

            result.Accounts = accountRepository.GetByStatement(result.StatementNumber);
            result.Encounters = encounterRepository.GetByStatement(result.StatementNumber);
            result.EncounterActivity = encounterActivityRepository.GetByStatement(result.StatementNumber);

            return result;
        }

        private List<PatientStatement> GetByStatement(IEnumerable<string> statementNos)
        {
            Log.Instance.Trace("Entering");
            var sql = Sql.Builder
                .Where($"{GetRealColumn(nameof(PatientStatement.StatementNumber))} in (@statementNos)");

            var statements = dbConnection.Query<PatientStatement>(sql.ToString(), statementNos.ToArray());

            return statements.ToList();
        }

        public int GetStatementCount(string batch)
        {
            Log.Instance.Trace("Entering");
            var sql = PetaPoco.Sql.Builder
                .Select("count(*) as 'Cnt'")
                .From("dbo.patbill_stmt")
                .Where($"{GetRealColumn(nameof(PatientStatement.BatchId))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = batch });

            int cnt = dbConnection.ExecuteScalar<int>(sql);

            return cnt;
        }

        public List<PatientStatement> GetByBatch(string batch)
        {
            Log.Instance.Trace("Entering");

            var sql = Sql.Builder;

            sql.Where($"{GetRealColumn(nameof(PatientStatement.BatchId))} = @0", 
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = batch});
            sql.Where($"{GetRealColumn(nameof(PatientStatement.StatementSubmittedDateTime))} is null or {GetRealColumn(nameof(PatientStatement.StatementSubmittedDateTime))} = '01/01/1900'");

            var results = dbConnection.Fetch<PatientStatement>(sql);

            foreach(var result in results)
            {
                result.Accounts = accountRepository.GetByStatement(result.StatementNumber);
                result.Encounters = encounterRepository.GetByStatement(result.StatementNumber);
                result.EncounterActivity = encounterActivityRepository.GetByStatement(result.StatementNumber);
            }

            Log.Instance.Debug(dbConnection.LastSQL);
            return results;
        }

        public override object Add(PatientStatement table)
        {
            base.Add(table);

            foreach(var account in table.Accounts)
            {
                accountRepository.Add(account);
            }

            foreach(var encounter in table.Encounters)
            {
                encounterRepository.Add(encounter);
            }

            foreach(var encntrActivity in table.EncounterActivity)
            {
                encounterActivityRepository.Add(encntrActivity);
            }

            var result = GetByStatement(table.StatementNumber);

            return result;
        }

    }
}
