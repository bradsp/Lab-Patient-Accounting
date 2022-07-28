using System;
using System.Collections.Generic;
using LabBilling.Logging;
using LabBilling.Core.Models;
using RFClassLibrary;

namespace LabBilling.Core.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class PatRepository : RepositoryBase<Pat>
    {
        private readonly DictDxRepository dictDxDb;
        private readonly PhyRepository phyRepository;

        public PatRepository(string connection) : base("pat", connection)
        {
            dictDxDb = new DictDxRepository(connection);
            phyRepository = new PhyRepository(connection);
        }

        public PatRepository(string connection, PetaPoco.Database db) : base("pat", connection, db)
        {
            dictDxDb = new DictDxRepository(connection);
            phyRepository = new PhyRepository(connection);
        }

        public override Pat GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Pat GetByAccount(string account)
        {
            Log.Instance.Debug("$Entering");

            var record = dbConnection.SingleOrDefault<Pat>("where account = @0", account);
            var accRecord = dbConnection.SingleOrDefault<Account>("where account = @0", account);
            record.Physician = phyRepository.GetByNPI(record.ProviderId);

            if(record == null)
            {
                //there was not a pat record, so no need to proceed.
                return new Pat();
            }

            if (accRecord.TransactionDate == null)
            {
                Log.Instance.Fatal("Transaction date was not valid.");
                this.Errors = "Transaction date is not valid.";
            }

            if(!Str.ParseName(record.PatFullName, out string strLastName, out string strFirstName, out string strMidName, out string strSuffix))
            {
                this.Errors = string.Format("Patient name could not be parsed. {0} {1}", record.PatFullName, record.AccountNo);
            }
            else
            {
                record.PatLastName = strLastName;
                record.PatFirstName = strFirstName;
                record.PatMiddleName = strMidName;
                record.PatNameSuffix = strSuffix;
            }

            if(!Str.ParseName(record.GuarantorFullName, out string strGuarLastName, out string strGuarFirstName, out string strGuarMidName, out string strGuarSuffix))
            {
                if (!string.IsNullOrEmpty(this.Errors))
                    this.Errors += Environment.NewLine;

                this.Errors += string.Format("Guarantor name could not be parsed. {0} {1}", record.GuarantorFullName, record.AccountNo);
            }
            else
            {
                record.GuarantorLastName = strGuarLastName;
                record.GuarantorFirstName = strGuarFirstName;
                record.GuarantorMiddleName = strGuarMidName;
                record.GuarantorNameSuffix = strGuarSuffix;
            }

            string amaYear = FunctionRepository.GetAMAYear(accRecord.TransactionDate.GetValueOrDefault(DateTime.Now));
            record.Diagnoses = new List<PatDiag>();
            if (record.Dx1 != null && record.Dx1 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.Dx1, amaYear);
                record.Dx1Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 1, Code = record.Dx1, Description = dictRecord.icd9_desc });
            }
            if (record.Dx2 != null && record.Dx2 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.Dx2, amaYear);
                record.Dx2Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 2, Code = record.Dx2, Description = dictRecord.icd9_desc });
            }
            if (record.Dx3 != null && record.Dx3 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.Dx3, amaYear);
                record.Dx3Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 3, Code = record.Dx3, Description = dictRecord.icd9_desc });
            }
            if (record.Dx4 != null && record.Dx4 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.Dx4, amaYear);
                record.Dx4Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 4, Code = record.Dx4, Description = dictRecord.icd9_desc });
            }
            if (record.Dx5 != null && record.Dx5 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.Dx5, amaYear);
                record.Dx5Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 5, Code = record.Dx5, Description = dictRecord.icd9_desc });
            }
            if (record.Dx6 != null && record.Dx6 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.Dx6, amaYear);
                record.Dx6Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 6, Code = record.Dx6, Description = dictRecord.icd9_desc });
            }
            if (record.Dx7 != null && record.Dx7 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.Dx7, amaYear);
                record.Dx7Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 7, Code = record.Dx7, Description = dictRecord.icd9_desc });
            }
            if (record.Dx8 != null && record.Dx8 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.Dx8, amaYear);
                record.Dx8Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 8, Code = record.Dx8, Description = dictRecord.icd9_desc });
            }
            if (record.Dx9 != null && record.Dx9 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.Dx9, amaYear);
                record.Dx9Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 9, Code = record.Dx9, Description = dictRecord.icd9_desc });
            }

            return record;
        }

        public bool SaveDiagnoses(Pat pat)
        {
            // this function will validate and save the dx from the model in both the pat record and in the patdx table

            //first - the updated diagnoses is in the PatDiag object. We need to update the individual fields in the Pat object from this
            // clear the individual fields
            pat.Dx1 = "";
            pat.Dx2 = "";
            pat.Dx3 = "";
            pat.Dx4 = "";
            pat.Dx5 = "";
            pat.Dx6 = "";
            pat.Dx7 = "";
            pat.Dx8 = "";
            pat.Dx9 = "";

            foreach (PatDiag dx in pat.Diagnoses)
            {
                //check the individual dx code and update
                switch(dx.No)
                {
                    case 1:
                        pat.Dx1 = dx.Code;
                        break;
                    case 2:
                        pat.Dx2 = dx.Code;
                        break;
                    case 3:
                        pat.Dx3 = dx.Code;
                        break;
                    case 4:
                        pat.Dx4 = dx.Code;
                        break;
                    case 5:
                        pat.Dx5 = dx.Code;
                        break;
                    case 6:
                        pat.Dx6 = dx.Code;
                        break;
                    case 7:
                        pat.Dx7 = dx.Code;
                        break;
                    case 8:
                        pat.Dx8 = dx.Code;
                        break;
                    case 9:
                        pat.Dx9 = dx.Code;
                        break;
                    default:
                        break;
                }                                   
            }
            if (dbConnection.Update(pat) > 0)
                return true;
            else
                return false;

        }

        public override bool Update(Pat table)
        {
            //generate full name from name parts
            table.PatFullName =
                String.Format("{0},{1} {2} {3}",
                table.PatLastName,
                table.PatFirstName,
                table.PatMiddleName,
                table.PatNameSuffix);
            table.PatFullName = table.PatFullName.Trim();

            table.GuarantorFullName =
                String.Format("{0},{1} {2} {3}",
                table.GuarantorLastName,
                table.GuarantorFirstName,
                table.GuarantorMiddleName,
                table.GuarantorNameSuffix);
            table.GuarantorFullName = table.GuarantorFullName.Trim();

            return base.Update(table);
        }

        public override bool Update(Pat table, IEnumerable<string> columns)
        {
            //generate full name from name parts
            table.PatFullName =
                String.Format("{0},{1} {2} {3}",
                table.PatLastName,
                table.PatFirstName,
                table.PatMiddleName,
                table.PatNameSuffix);
            table.PatFullName = table.PatFullName.Trim();

            table.GuarantorFullName =
                String.Format("{0},{1} {2} {3}",
                table.GuarantorLastName,
                table.GuarantorFirstName,
                table.GuarantorMiddleName,
                table.GuarantorNameSuffix);
            table.GuarantorFullName = table.GuarantorFullName.Trim();

            return base.Update(table, columns);
        }


        public void SaveAll(Pat pat)
        {
            dbConnection.Save(pat);
        }
    }
}
