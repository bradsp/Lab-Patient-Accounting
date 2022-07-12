using System;
using System.Collections.Generic;
using LabBilling.Logging;
using LabBilling.Core.Models;
using RFClassLibrary;

namespace LabBilling.Core.DataAccess
{
    public class InsRepository : RepositoryBase<Ins>
    {
        public InsRepository(string connection) : base("ins", connection)
        {

        }

        public InsRepository(string connection, PetaPoco.Database db) : base("ins", connection, db)
        {

        }

        public override Ins GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Ins> GetByAccount(string account)
        {
            Log.Instance.Debug("$Entering");

            var records = dbConnection.Fetch<Ins>("where account = @0", account);

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
    }
}
