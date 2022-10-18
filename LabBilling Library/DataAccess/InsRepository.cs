using System;
using System.Collections.Generic;
using LabBilling.Logging;
using LabBilling.Core.Models;
using RFClassLibrary;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace LabBilling.Core.DataAccess
{
    public class InsRepository : RepositoryBase<Ins>
    {
        public InsRepository(string connection) : base(connection)
        {

        }

        public InsRepository(PetaPoco.Database db) : base(db)
        {

        }

        public override Ins GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Ins> GetByAccount(string account)
        {
            Log.Instance.Debug("$Entering");

            var records = dbConnection.Fetch<Ins>("where account = @0 and deleted = 0 order by ins_a_b_c", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

            foreach(Ins ins in records)
            {
                if(string.IsNullOrEmpty(ins.HolderLastName) || string.IsNullOrEmpty(ins.HolderFirstName))
                {
                    if (!Str.ParseName(ins.HolderFullName.ToString(),
                        out string lname, out string fname, out string mname, out string suffix))
                    {
                        //error parsing name
                        Log.Instance.Info($"Insurance holder name could not be parsed. {ins.HolderFullName}");
                    }

                    ins.HolderLastName = lname;
                    ins.HolderFirstName = fname;
                    ins.HolderMiddleName = mname;
                }
                else
                {
                    ins.HolderFullName = $"{ins.HolderLastName},{ins.HolderFirstName} {ins.HolderMiddleName}";
                }

                ins.InsCompany = dbConnection.SingleOrDefault<InsCompany>("where code = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = ins.InsCode });
                if (ins.InsCompany == null)
                    ins.InsCompany = new InsCompany();

                Str.ParseCityStZip(ins.HolderCityStZip, out string strCity, out string strState, out string strZip);
                ins.HolderCity = strCity;
                ins.HolderState = strState;
                ins.HolderZip = strZip;
            }

            return records;
        }
        
        public Ins GetByAccount(string account, InsCoverage coverage)
        {

            var record = dbConnection.SingleOrDefault<Ins>("where account = @0 and ins_a_b_c = @1",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = coverage.Value });
            if (record != null)
            {
                record.InsCompany = dbConnection.SingleOrDefault<InsCompany>("where code = @0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = record.InsCode });
                if (record.InsCompany == null)
                    record.InsCompany = new InsCompany();

                Str.ParseCityStZip(record.HolderCityStZip, out string strCity, out string strState, out string strZip);
                record.HolderCity = strCity;
                record.HolderState = strState;
                record.HolderZip = strZip;
            }
            return record;
        }

        public override bool Update(Ins table, IEnumerable<string> columns)
        {

            List<string> cols = columns.ToList();

            if(cols.Contains(nameof(Ins.HolderState)) || 
                cols.Contains(nameof(Ins.HolderCity)) || 
                cols.Contains(nameof(Ins.HolderZip)))
            {
                cols.Add(nameof(Ins.HolderCityStZip));
                cols.Remove(nameof(Ins.HolderState));
                cols.Remove(nameof(Ins.HolderCity));
                cols.Remove(nameof(Ins.HolderZip));
            }
            if(cols.Contains(nameof(Ins.HolderLastName)) || 
                cols.Contains(nameof(Ins.HolderFirstName)) || 
                cols.Contains(nameof(Ins.HolderMiddleName)))
            {
                cols.Add(nameof(Ins.HolderFullName));
            }
            return base.Update(table, cols);
        }
    }

    public class InsCoverage
    {
        private InsCoverage(string value) { Value = value; }

        public string Value { get; private set; }

        public static InsCoverage Primary => new InsCoverage("A");
        public static InsCoverage Secondary => new InsCoverage("B");
        public static InsCoverage Tertiary => new InsCoverage("C");

    }
}
