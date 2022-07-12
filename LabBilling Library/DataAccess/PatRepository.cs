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
            record.Physician = phyRepository.GetByNPI(record.phy_id);

            if(record == null)
            {
                //there was not a pat record, so no need to proceed.
                return new Pat();
            }

            if (accRecord.trans_date == null)
            {
                Log.Instance.Fatal("Transaction date was not valid.");
                this.Errors = "Transaction date is not valid.";
            }

            if(!Str.ParseName(record.pat_full_name, out string strLastName, out string strFirstName, out string strMidName, out string strSuffix))
            {
                this.Errors = string.Format("Patient name could not be parsed. {0} {1}", record.pat_full_name, record.account);
            }
            else
            {
                record.pat_last_name = strLastName;
                record.pat_first_name = strFirstName;
                record.pat_middle_name = strMidName;
                record.pat_name_suffix = strSuffix;
            }

            if(!Str.ParseName(record.guarantor, out string strGuarLastName, out string strGuarFirstName, out string strGuarMidName, out string strGuarSuffix))
            {
                if (!string.IsNullOrEmpty(this.Errors))
                    this.Errors += Environment.NewLine;

                this.Errors += string.Format("Guarantor name could not be parsed. {0} {1}", record.guarantor, record.account);
            }
            else
            {
                record.GuarantorLastName = strGuarLastName;
                record.GuarantorFirstName = strGuarFirstName;
                record.GuarantorMiddleName = strGuarMidName;
                record.GuarantorNameSuffix = strGuarSuffix;
            }

            string amaYear = FunctionRepository.GetAMAYear(accRecord.trans_date.GetValueOrDefault(DateTime.Now));
            record.Diagnoses = new List<PatDiag>();
            if (record.icd9_1 != null && record.icd9_1 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.icd9_1, amaYear);
                record.Dx1Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 1, Code = record.icd9_1, Description = dictRecord.icd9_desc });
            }
            if (record.icd9_2 != null && record.icd9_2 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.icd9_2, amaYear);
                record.Dx2Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 2, Code = record.icd9_2, Description = dictRecord.icd9_desc });
            }
            if (record.icd9_3 != null && record.icd9_3 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.icd9_3, amaYear);
                record.Dx3Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 3, Code = record.icd9_3, Description = dictRecord.icd9_desc });
            }
            if (record.icd9_4 != null && record.icd9_4 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.icd9_4, amaYear);
                record.Dx4Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 4, Code = record.icd9_4, Description = dictRecord.icd9_desc });
            }
            if (record.icd9_5 != null && record.icd9_5 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.icd9_5, amaYear);
                record.Dx5Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 5, Code = record.icd9_5, Description = dictRecord.icd9_desc });
            }
            if (record.icd9_6 != null && record.icd9_6 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.icd9_6, amaYear);
                record.Dx6Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 6, Code = record.icd9_6, Description = dictRecord.icd9_desc });
            }
            if (record.icd9_7 != null && record.icd9_7 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.icd9_7, amaYear);
                record.Dx7Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 7, Code = record.icd9_7, Description = dictRecord.icd9_desc });
            }
            if (record.icd9_8 != null && record.icd9_8 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.icd9_8, amaYear);
                record.Dx8Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 8, Code = record.icd9_8, Description = dictRecord.icd9_desc });
            }
            if (record.icd9_9 != null && record.icd9_9 != "")
            {
                var dictRecord = dictDxDb.GetByCode(record.icd9_9, amaYear);
                record.Dx9Desc = dictRecord.icd9_desc;
                record.Diagnoses.Add(new PatDiag { No = 9, Code = record.icd9_9, Description = dictRecord.icd9_desc });
            }

            return record;
        }

        public bool SaveDiagnoses(Pat pat)
        {
            // this function will validate and save the dx from the model in both the pat record and in the patdx table

            //first - the updated diagnoses is in the PatDiag object. We need to update the individual fields in the Pat object from this
            // clear the individual fields
            pat.icd9_1 = "";
            pat.icd9_2 = "";
            pat.icd9_3 = "";
            pat.icd9_4 = "";
            pat.icd9_5 = "";
            pat.icd9_6 = "";
            pat.icd9_7 = "";
            pat.icd9_8 = "";
            pat.icd9_9 = "";

            foreach (PatDiag dx in pat.Diagnoses)
            {
                //check the individual dx code and update
                switch(dx.No)
                {
                    case 1:
                        pat.icd9_1 = dx.Code;
                        break;
                    case 2:
                        pat.icd9_2 = dx.Code;
                        break;
                    case 3:
                        pat.icd9_3 = dx.Code;
                        break;
                    case 4:
                        pat.icd9_4 = dx.Code;
                        break;
                    case 5:
                        pat.icd9_5 = dx.Code;
                        break;
                    case 6:
                        pat.icd9_6 = dx.Code;
                        break;
                    case 7:
                        pat.icd9_7 = dx.Code;
                        break;
                    case 8:
                        pat.icd9_8 = dx.Code;
                        break;
                    case 9:
                        pat.icd9_9 = dx.Code;
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
            table.pat_full_name =
                String.Format("{0},{1} {2} {3}",
                table.pat_last_name,
                table.pat_first_name,
                table.pat_middle_name,
                table.pat_name_suffix);
            table.pat_full_name = table.pat_full_name.Trim();

            table.guarantor =
                String.Format("{0},{1} {2} {3}",
                table.GuarantorLastName,
                table.GuarantorFirstName,
                table.GuarantorMiddleName,
                table.GuarantorNameSuffix);
            table.guarantor = table.guarantor.Trim();

            return base.Update(table);
        }

        public override bool Update(Pat table, IEnumerable<string> columns)
        {
            //generate full name from name parts
            table.pat_full_name =
                String.Format("{0},{1} {2} {3}",
                table.pat_last_name,
                table.pat_first_name,
                table.pat_middle_name,
                table.pat_name_suffix);
            table.pat_full_name = table.pat_full_name.Trim();

            table.guarantor =
                String.Format("{0},{1} {2} {3}",
                table.GuarantorLastName,
                table.GuarantorFirstName,
                table.GuarantorMiddleName,
                table.GuarantorNameSuffix);
            table.guarantor = table.guarantor.Trim();

            return base.Update(table, columns);
        }


        public void SaveAll(Pat pat)
        {
            dbConnection.Save(pat);
        }
    }
}
