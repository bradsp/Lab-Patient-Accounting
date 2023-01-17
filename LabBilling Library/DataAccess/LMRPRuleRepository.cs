using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using LabBilling.Core.Models;
using System.Data.SqlClient;
using System.Data;

namespace LabBilling.Core.DataAccess
{
    public class LMRPRuleRepository : RepositoryBase<LMRPRule>
    {
        public LMRPRuleRepository(string connectionString) : base(connectionString)
        {

        }

        public LMRPRuleRepository(PetaPoco.Database db) : base(db)
        {

        }

        public LMRPRule GetRule(string cpt, string dx, DateTime serviceDate)
        {
            string cptRealName = this.GetRealColumn(typeof(LMRPRule), nameof(LMRPRule.CptCode));
            string begDxName = this.GetRealColumn(typeof(LMRPRule), nameof(LMRPRule.BegDx));
            string endDxName = this.GetRealColumn(typeof(LMRPRule), nameof(LMRPRule.EndingDx));
            string amaYearName = this.GetRealColumn(typeof(LMRPRule), nameof(LMRPRule.AmaYear));
            string rbDateName = this.GetRealColumn(typeof(LMRPRule), nameof(LMRPRule.RBDate));
            string expireDateName = this.GetRealColumn(typeof(LMRPRule), nameof(LMRPRule.ExpirationDate));

            var result = dbConnection.SingleOrDefault<LMRPRule>($"where {cptRealName} = @0 and @1 between {begDxName} and {endDxName} " + 
                "and {amaYearName} = @2 and  {rbDateName} <= @3 and {expireDateName} >= @4",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cpt },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = dx },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AmaYear(serviceDate) },
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = serviceDate },
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = serviceDate });

            return result;
        }

        private int AmaYear(DateTime dt)
        {
            int nAmaYear = dt.Year;
            DateTime nNewAmaYear = DateTime.Parse(
                string.Format("10/01/{0}", nAmaYear));// October 1 is the start of the new AMAYEAR
            if (dt >= nNewAmaYear)
            {
                nAmaYear++;
            }

            return nAmaYear;
        }

        public LMRPRuleDefinition GetRuleDefinition(string cpt, DateTime serviceDate)
        {
            string amaYearName = this.GetRealColumn(typeof(LMRPRule), nameof(LMRPRule.AmaYear));
            string cptRealName = this.GetRealColumn(typeof(LMRPRule), nameof(LMRPRule.CptCode));
            string expireDateName = this.GetRealColumn(typeof(LMRPRule), nameof(LMRPRule.ExpirationDate));
            string rbDateName = this.GetRealColumn(typeof(LMRPRule), nameof(LMRPRule.RBDate));

            var result = dbConnection.SingleOrDefault<LMRPRuleDefinition>($"where {cptRealName} = @0 " +
                $"and {amaYearName} = @1 and  {rbDateName} <= @2 and {expireDateName} >= @3",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cpt },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AmaYear(serviceDate) },
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = serviceDate },
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = serviceDate });

            return result;
        }

        public int RulesLoaded(DateTime serviceDate)
        {

            string amaYearName = this.GetRealColumn(typeof(LMRPRule), nameof(LMRPRule.AmaYear));
            var result = dbConnection.ExecuteScalar<int>($"select count(*) from {_tableName} where {amaYearName} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AmaYear(serviceDate) });

            return result;
        }

        public List<LMRPRule> GetRules(DateTime serviceDate)
        {
            string amaYearName = this.GetRealColumn(typeof(LMRPRule), nameof(LMRPRule.AmaYear));

            var result = dbConnection.Fetch<LMRPRule>($"where {amaYearName} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AmaYear(serviceDate) });

            return result;
        }
    }
}
