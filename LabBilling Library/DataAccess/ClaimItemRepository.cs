using System;
using System.Collections.Generic;
using PetaPoco;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using Microsoft.Data.SqlClient;
using System.Data;
using LabBilling.Logging;
using LabBilling.Core.Services;

namespace LabBilling.Core.DataAccess
{
    public class ClaimItemRepository : RepositoryCoreBase<ClaimItem>
    {
        public ClaimItemRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context) {  }

        public List<ClaimItem> GetClaimItems(ClaimType claimType)
        {
            PetaPoco.Sql command;

            string selMaxRecords = string.Empty;

            if (AppEnvironment.ApplicationParameters.MaxClaimsInClaimBatch > 0)
            {
                selMaxRecords = $"TOP {AppEnvironment.ApplicationParameters.MaxClaimsInClaimBatch}";
            }

            string accTableName = unitOfWork.AccountRepository.TableName;
            string insTableName = unitOfWork.InsRepository.TableName;

            var selectCols = new[]
            {
                accTableName + "." + GetRealColumn(nameof(ClaimItem.status)),
                accTableName + "." + GetRealColumn(nameof(ClaimItem.account)),
                accTableName + "." + GetRealColumn(nameof(ClaimItem.pat_name)),
                accTableName + "." + GetRealColumn(nameof(ClaimItem.ssn)),
                accTableName + "." + GetRealColumn(nameof(ClaimItem.cl_mnem)),
                accTableName + "." + GetRealColumn(nameof(ClaimItem.fin_code)),
                accTableName + "." + GetRealColumn(nameof(ClaimItem.trans_date)),
                insTableName + "." + GetRealColumn(nameof(ClaimItem.ins_plan_nme))
            };

            //TODO: how to do TOP # in the select when done this way.
            command = PetaPoco.Sql.Builder
                .Select(selectCols)
                .From(accTableName)
                .InnerJoin(insTableName)
                .On($"{insTableName}.{unitOfWork.InsRepository.GetRealColumn(nameof(Ins.Account))} = {accTableName}.{unitOfWork.AccountRepository.GetRealColumn(nameof(Account.AccountNo))} and {unitOfWork.InsRepository.GetRealColumn(nameof(Ins.Coverage))} = '{InsCoverage.Primary}'");

            try
            {
                switch (claimType)
                {
                    case ClaimType.Institutional:
                        command.Where("status = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AccountStatus.Institutional });
                        break;
                    case ClaimType.Professional:
                        command.Where("status = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AccountStatus.Professional });
                        break;
                    default:
                        command = PetaPoco.Sql.Builder;
                        break;
                }

                command.OrderBy($"{GetRealColumn(nameof(Account.TransactionDate))}");

                var queryResult = Context.Fetch<ClaimItem>(command);

                return queryResult;
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                return new List<ClaimItem>();
            }


        }
    }
}
