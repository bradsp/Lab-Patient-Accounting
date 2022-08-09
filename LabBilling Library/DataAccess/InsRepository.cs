using System;
using System.Collections.Generic;
using LabBilling.Logging;
using LabBilling.Core.Models;
using RFClassLibrary;

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

            var records = dbConnection.Fetch<Ins>("where account = @0 order by ins_a_b_c", account);

            foreach(Ins ins in records)
            {
                ins.InsCompany = dbConnection.SingleOrDefault<InsCompany>("where code = @0", ins.InsCode);
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
            var record = dbConnection.SingleOrDefault<Ins>("where account = @0 and ins_a_b_c = @1", account, coverage.Value);
            if (record != null)
            {
                record.InsCompany = dbConnection.SingleOrDefault<InsCompany>("where code = @0", record.InsCode);
                if (record.InsCompany == null)
                    record.InsCompany = new InsCompany();

                Str.ParseCityStZip(record.HolderCityStZip, out string strCity, out string strState, out string strZip);
                record.HolderCity = strCity;
                record.HolderState = strState;
                record.HolderZip = strZip;
            }
            return record;
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
