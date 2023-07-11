using System;
using System.Collections.Generic;
using LabBilling.Logging;
using LabBilling.Core.Models;
using RFClassLibrary;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace LabBilling.Core.DataAccess
{
    public sealed class InsRepository : RepositoryBase<Ins>
    {
        public InsRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {

        }

        public List<Ins> GetByAccount(string account)
        {
            Log.Instance.Debug($"Entering - account {account}");

            var records = dbConnection.Fetch<Ins>("where account = @0 and deleted = 0 order by ins_a_b_c", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

            foreach(Ins ins in records)
            {
                if (!string.IsNullOrEmpty(ins.HolderFullName))
                {
                    if (string.IsNullOrEmpty(ins.HolderLastName) || string.IsNullOrEmpty(ins.HolderFirstName))
                    {
                        if (!StringExtensions.ParseName(ins.HolderFullName.ToString(),
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
                }
                else
                {
                    ins.HolderFullName = string.Empty;
                }

                if (ins.InsCode != null)
                {
                    ins.InsCompany = dbConnection.SingleOrDefault<InsCompany>("where code = @0",
                        new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = ins.InsCode });
                }
                if (ins.InsCompany == null)
                    ins.InsCompany = new InsCompany();

                StringExtensions.ParseCityStZip(ins.HolderCityStZip, out string strCity, out string strState, out string strZip);
                ins.HolderCity = strCity;
                ins.HolderState = strState;
                ins.HolderZip = strZip;

                StringExtensions.ParseCityStZip(ins.PlanCityState, out strCity, out strState, out strZip);
                ins.PlanCity = strCity;
                ins.PlanState = strState;
                ins.PlanZip = strZip;

            }

            return records;
        }
        
        public Ins GetByAccount(string account, InsCoverage coverage)
        {
            Log.Instance.Trace($"Entering - account {account} coverage {coverage.ToString()}");

            if(coverage == null)
            {
                throw new ArgumentNullException("coverage");
            }

            var record = dbConnection.SingleOrDefault<Ins>("where account = @0 and ins_a_b_c = @1",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = coverage.ToString() });
            if (record != null)
            {
                if (record.InsCode != null)
                {
                    record.InsCompany = dbConnection.SingleOrDefault<InsCompany>("where code = @0",
                        new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = record.InsCode });
                }
                if (record.InsCompany == null)
                    record.InsCompany = new InsCompany();

                StringExtensions.ParseCityStZip(record.HolderCityStZip, out string strCity, out string strState, out string strZip);
                record.HolderCity = strCity;
                record.HolderState = strState;
                record.HolderZip = strZip;

                StringExtensions.ParseCityStZip(record.PlanCityState, out strCity, out strState, out strZip);
                record.PlanCity = strCity;
                record.PlanState = strState;
                record.PlanZip = strZip;

            }
            return record;
        }

        public override object Add(Ins table)
        {
            table.HolderCityStZip = $"{table.HolderCity}, {table.HolderState} {table.HolderZip}";
            table.HolderFullName = $"{table.HolderLastName},{table.HolderFirstName} {table.HolderMiddleName}".TrimEnd();
            return base.Add(table);
        }

        public override bool Delete(Ins table)
        {
            var result = base.Delete(table);

            //deleting an insurance may result in other insurances being reassigned ordering
            var insurances = GetByAccount(table.Account);

            int iteration = 1;
            foreach(var ins in insurances)
            {
                switch(iteration)
                {
                    case 1:
                        if(ins.Coverage != "A")
                        {
                            ins.Coverage = "A";
                            Update(ins);
                        }
                        break;
                    case 2:
                        if(ins.Coverage != "B")
                        {
                            ins.Coverage = "B";
                            Update(ins);
                        }
                        break;
                    case 3:
                        if(ins.Coverage != "C")
                        {
                            ins.Coverage = "C";
                            Update(ins);
                        }
                        break;
                    default:
                        break;
                }
                iteration++;
            }

            return result;
        }

        public override bool Save(Ins table)
        {
            if(table.Coverage == null)
            {
                return false;
            }

            var record = this.GetByAccount(table.Account, InsCoverage.Parse(table.Coverage));

            try
            {
                if (record == null)
                    this.Add(table);
                else
                    this.Update(table);
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Exception in InsRepository.Save", ex);         
            }

            return true;
        }

        public override bool Update(Ins table, IEnumerable<string> columns)
        {
            Log.Instance.Trace($"Entering - account {table.Account}");
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

    public struct InsCoverage
    {
        private string value;

        public static IEnumerable<InsCoverage> AllInsCoverages
        {
            get
            {
                yield return Primary;
                yield return Secondary;
                yield return Tertiary;
                yield return Temporary;
            }
        }

        public static InsCoverage Primary { get; } = new InsCoverage("A");
        public static InsCoverage Secondary { get; } = new InsCoverage("B");
        public static InsCoverage Tertiary { get; } = new InsCoverage("C");
        public static InsCoverage Temporary { get; } = new InsCoverage("X");


        /// <summary>
        /// primary constructor
        /// </summary>
        /// <param name="value">The string value that this is a wrapper for</param>
        private InsCoverage(string value)
        {
            this.value = value;
        }
        /// <summary>
        /// Compares the Group to another group, or to a string value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is InsCoverage)
            {
                return this.value.Equals(((InsCoverage)obj).value);
            }

            string otherString = obj as string;
            if (otherString != null)
            {
                return this.value.Equals(otherString);
            }

            throw new ArgumentException("obj is neither a InsCoverage nor a String");
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        /// <summary>
        /// returns the internal string that this is a wrapper for.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static implicit operator string(InsCoverage insCoverage)
        {
            return insCoverage.value;
        }

        /// <summary>
        /// Parses a string and returns an instance that corresponds to it.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static InsCoverage Parse(string input)
        {
            return AllInsCoverages.Where(item => item.value == input).FirstOrDefault();
        }

        /// <summary>
        /// Syntatic sugar for the Parse method.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static explicit operator InsCoverage(string other)
        {
            return Parse(other);
        }

        public override string ToString()
        {
            return value;
        }
    }
}
