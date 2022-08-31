using System;
using System.Collections.Generic;
using LabBilling.Logging;
using LabBilling.Core.Models;
using RFClassLibrary;
using System.Linq;

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

            var records = dbConnection.Fetch<Ins>("where account = @0 and deleted = 0 order by ins_a_b_c", account);

            foreach(Ins ins in records)
            {
                if(string.IsNullOrEmpty(ins.HolderLastName) || string.IsNullOrEmpty(ins.HolderFirstName))
                {
                    if (!Str.ParseName(ins.HolderName.ToString(),
                        out string lname, out string fname, out string mname, out string suffix))
                    {
                        //error parsing name
                        Log.Instance.Info($"Insurance holder name could not be parsed. {ins.HolderName}");
                    }

                    ins.HolderLastName = lname;
                    ins.HolderFirstName = fname;
                    ins.HolderMiddleName = mname;
                }

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

        public override bool Update(Ins table, IEnumerable<string> columns)
        {

            List<string> cols = columns.ToList();

            if(cols.Contains("HolderState") || cols.Contains("HolderCity") || cols.Contains("HolderZip"))
            {
                cols.Add("HolderCityStZip");
                cols.Remove("HolderState");
                cols.Remove("HolderCity");
                cols.Remove("HolderZip");
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
