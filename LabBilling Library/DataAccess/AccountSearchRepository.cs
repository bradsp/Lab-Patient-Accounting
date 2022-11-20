using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using RFClassLibrary;
using LabBilling.Core.BusinessLogic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace LabBilling.Core.DataAccess
{
    public class AccountSearchRepository : RepositoryBase<AccountSearch>
    {
        public AccountSearchRepository(string connectionString) : base(connectionString)
        {

        }

        public AccountSearchRepository(PetaPoco.Database db) : base(db)
        {

        }
        public override AccountSearch GetById(int id)
        {
            throw new NotImplementedException();
        }

        public enum operation
        {
            Equal,
            Like,
            GreaterThan,
            GreaterThanOrEqual,
            LessThan,
            LessThanOrEqual,
            NotEqual,
            OneOf,
            NotOneOf
        }

        public IList<AccountSearch> GetBySearch((string propertyName, operation oper, string searchText)[] searchValues)
        {
            InsRepository insRepository = new InsRepository(dbConnection);
            try
            {
                var command = PetaPoco.Sql.Builder;

                foreach (var searchValue in searchValues)
                {
                    Type accounttype = typeof(AccountSearch);
                    var prop = accounttype.GetProperty(searchValue.propertyName);

                    string propName = GetRealColumn(typeof(AccountSearch), prop.Name);
                    string op;
                    string searchText = searchValue.searchText;

                    switch (searchValue.oper)
                    {
                        case operation.Equal:
                            op = "=";
                            break;
                        case operation.Like:
                            op = "like";
                            searchText += "%";
                            break;
                        case operation.GreaterThanOrEqual:
                            op = ">=";
                            break;
                        case operation.GreaterThan:
                            op = ">";
                            break;
                        case operation.LessThanOrEqual:
                            op = "<=";
                            break;
                        case operation.LessThan:
                            op = "<";
                            break;
                        case operation.NotEqual:
                            op = "<>";
                            break;
                        case operation.OneOf:
                            op = "in";
                            break;
                        case operation.NotOneOf:
                            op = "not in";
                            break;
                        default:
                            op = "=";
                            break;
                    }
                    if (op == "in")
                    {
                        command.Where($"{propName} {op} ({searchText})");
                    }
                    else
                    {
                        command.Where($"{propName} {op} '{searchText}'");
                    }
                }
                command.OrderBy(GetRealColumn(nameof(AccountSearch.Name)));
                
                var results = dbConnection.Fetch<AccountSearch>(command);

                return results;
            }
            catch (NullReferenceException nre)
            {
                Log.Instance.Fatal(nre, $"Exception in");
            }
            catch (Exception ex)
            {
                Log.Instance.Fatal(ex, $"Exception in");
            }

            return new List<AccountSearch>();
        }

        public async Task<IEnumerable<AccountSearch>> GetBySearchAsync((string propertyName, operation oper, string searchText)[] searchValues)
        {
            InsRepository insRepository = new InsRepository(dbConnection);
            try
            {
                var command = PetaPoco.Sql.Builder;

                foreach (var searchValue in searchValues)
                {
                    //var expr = (MemberExpression)searchValue.propertyName.Body;
                    Type accounttype = typeof(AccountSearch);
                    var prop = accounttype.GetProperty(searchValue.propertyName);

                    string propName = GetRealColumn(typeof(AccountSearch), prop.Name);
                    string op;
                    string searchText = searchValue.searchText;

                    switch (searchValue.oper)
                    {
                        case operation.Equal:
                            op = "=";
                            break;
                        case operation.Like:
                            op = "like";
                            searchText += "%";
                            break;
                        case operation.GreaterThanOrEqual:
                            op = ">=";
                            break;
                        case operation.GreaterThan:
                            op = ">";
                            break;
                        case operation.LessThanOrEqual:
                            op = "<=";
                            break;
                        case operation.LessThan:
                            op = "<";
                            break;
                        case operation.NotEqual:
                            op = "<>";
                            break;
                        case operation.OneOf:
                            op = "in";
                            break;
                        case operation.NotOneOf:
                            op = "not in";
                            break;
                        default:
                            op = "=";
                            break;
                    }
                    command.Where($"{propName} {op} '{searchText}'");
                }
                command.OrderBy(GetRealColumn(typeof(AccountSearch), nameof(AccountSearch.Name)));
                var results = dbConnection.Fetch<AccountSearch>(command);

                foreach (var result in results)
                {
                    var ins = insRepository.GetByAccount(result.Account, InsCoverage.Primary);
                    if (ins != null)
                        result.PrimaryInsCode = ins.InsCode;
                }

                return results;
            }
            catch (NullReferenceException nre)
            {
                Log.Instance.Fatal(nre, $"Exception in");
            }
            catch (Exception ex)
            {
                Log.Instance.Fatal(ex, $"Exception in");
            }

            return new List<AccountSearch>();
        }



        public IEnumerable<AccountSearch> GetBySearch(string lastNameSearchText, string firstNameSearchText, string mrnSearchText, string ssnSearchText, string dobSearch, string sexSearch, string accountSearchText)
        {
            Log.Instance.Debug($"Entering");

            if ((lastNameSearchText == string.Empty) && (firstNameSearchText == string.Empty) && (mrnSearchText == string.Empty)
                && (ssnSearchText == string.Empty)
                && (dobSearch == string.Empty) && (sexSearch == string.Empty) && (accountSearchText == string.Empty))
            {
                return new List<AccountSearch>();
            }

            try
            {
                string nameSearch = "";
                if (!(lastNameSearchText == "" && firstNameSearchText == ""))
                    nameSearch = string.Format("{0}%,{1}%", lastNameSearchText, firstNameSearchText);

                var command = PetaPoco.Sql.Builder
                    .Where("deleted = 0 ");

                if (!String.IsNullOrEmpty(lastNameSearchText))
                    command.Where($"{GetRealColumn(nameof(AccountSearch.LastName))} like @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = lastNameSearchText+"%" });

                if(!string.IsNullOrEmpty(firstNameSearchText))
                    command.Where($"{GetRealColumn(nameof(AccountSearch.FirstName))} like @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = firstNameSearchText + "%" });

                if (!string.IsNullOrEmpty(accountSearchText))
                    command.Where($"{GetRealColumn(nameof(AccountSearch.Account))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountSearchText });

                if (!string.IsNullOrEmpty(mrnSearchText))
                    command.Where($"{GetRealColumn(nameof(AccountSearch.MRN))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = mrnSearchText });

                if (!string.IsNullOrEmpty(sexSearch))
                    command.Where($"{GetRealColumn(nameof(AccountSearch.Sex))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = sexSearch });

                if (!string.IsNullOrEmpty(ssnSearchText))
                    command.Where($"{GetRealColumn(nameof(AccountSearch.SSN))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = ssnSearchText });

                if (!string.IsNullOrEmpty(dobSearch))
                {
                    _ = DateTime.TryParse(dobSearch, out DateTime dobDt);
                    command.Where($"{GetRealColumn(nameof(AccountSearch.DateOfBirth))} = @0", new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = dobDt });
                }
                    
                command.OrderBy($"{GetRealColumn(nameof(AccountSearch.ServiceDate))} desc");

                return dbConnection.Fetch<AccountSearch>(command);
            }
            catch (Exception ex)
            {
                Log.Instance.Fatal(ex, $"Exception in");
            }

            return new List<AccountSearch>();
        }

    }
}
