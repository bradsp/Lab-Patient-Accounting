using LabBilling.Core.Models;
using LabBilling.Logging;
using Microsoft.Data.SqlClient;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LabBilling.Core.DataAccess;

public sealed class AccountSearchRepository : RepositoryBase<AccountSearch>
{
    public AccountSearchRepository(IAppEnvironment appEnvironment, IDatabase context) : base(appEnvironment, context)
    {

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
        try
        {
            var command = PetaPoco.Sql.Builder;

            foreach (var searchValue in searchValues)
            {
                Type accounttype = typeof(AccountSearch);
                var prop = accounttype.GetProperty(searchValue.propertyName);

                string propName = GetRealColumn(typeof(AccountSearch), prop.Name);
                Type propType = prop.PropertyType;

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
                if (op == "in" || op == "not in")
                {
                    var values = searchText.Split(',').Select(s => s.Trim().Trim('\'')).ToArray();
                    command.Where($"{propName} {op} (@0)", values);
                }
                else
                {
                    command.Where($"{propName} {op} @0",
                        new SqlParameter() { SqlDbType = GetType(propType), Value = searchText });
                }
            }
            command.OrderBy(GetRealColumn(nameof(AccountSearch.Name)));

            var results = Context.Fetch<AccountSearch>(command);

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

    private static SqlDbType GetType(Type propType)
    {
        if (propType == typeof(System.String))
        {
            return SqlDbType.VarChar;
        }
        else if (propType == typeof(System.Int32) || propType == typeof(System.Int16) || propType == typeof(System.Int64))
        {
            return SqlDbType.Int;
        }
        else if (propType == typeof(System.Double))
        {
            return SqlDbType.Decimal;
        }
        else if (propType == typeof(DateTime))
        {
            return SqlDbType.DateTime;
        }
        else if (propType == typeof(System.Nullable<System.DateTime>))
        {
            return SqlDbType.DateTime;
        }

        return SqlDbType.VarChar;
    }

    public IEnumerable<AccountSearch> GetBySearchAsync((string propertyName, operation oper, string searchText)[] searchValues)
    {
        try
        {
            var command = PetaPoco.Sql.Builder;

            foreach (var searchValue in searchValues)
            {
                Type accounttype = typeof(AccountSearch);
                var prop = accounttype.GetProperty(searchValue.propertyName);

                string propName = GetRealColumn(typeof(AccountSearch), prop.Name);
                Type propType = prop.PropertyType;
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
                if (op == "in" || op == "not in")
                {
                    var values = searchText.Split(',').Select(s => s.Trim().Trim('\'')).ToArray();
                    command.Where($"{propName} {op} (@0)", values);
                }
                else
                {
                    command.Where($"{propName} {op} @0",
                        new SqlParameter() { SqlDbType = GetType(propType), Value = searchText });
                }
            }
            command.OrderBy(GetRealColumn(typeof(AccountSearch), nameof(AccountSearch.Name)));
            var results = Context.Fetch<AccountSearch>(command);

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
            var command = PetaPoco.Sql.Builder
                .Where("deleted = 0 ");

            command.Where($"(@0 IS NULL OR {GetRealColumn(nameof(AccountSearch.LastName))} like @1)",
                string.IsNullOrEmpty(lastNameSearchText) ? (object)DBNull.Value : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = lastNameSearchText + "%" },
                string.IsNullOrEmpty(lastNameSearchText) ? (object)DBNull.Value : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = lastNameSearchText + "%" });

            command.Where($"(@0 IS NULL OR {GetRealColumn(nameof(AccountSearch.FirstName))} like @1)",
                string.IsNullOrEmpty(firstNameSearchText) ? (object)DBNull.Value : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = firstNameSearchText + "%" },
                string.IsNullOrEmpty(firstNameSearchText) ? (object)DBNull.Value : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = firstNameSearchText + "%" });

            command.Where($"(@0 IS NULL OR {GetRealColumn(nameof(AccountSearch.Account))} = @1)",
                string.IsNullOrEmpty(accountSearchText) ? (object)DBNull.Value : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountSearchText },
                string.IsNullOrEmpty(accountSearchText) ? (object)DBNull.Value : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountSearchText });

            command.Where($"(@0 IS NULL OR {GetRealColumn(nameof(AccountSearch.MRN))} = @1)",
                string.IsNullOrEmpty(mrnSearchText) ? (object)DBNull.Value : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = mrnSearchText },
                string.IsNullOrEmpty(mrnSearchText) ? (object)DBNull.Value : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = mrnSearchText });

            command.Where($"(@0 IS NULL OR {GetRealColumn(nameof(AccountSearch.Sex))} = @1)",
                string.IsNullOrEmpty(sexSearch) ? (object)DBNull.Value : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = sexSearch },
                string.IsNullOrEmpty(sexSearch) ? (object)DBNull.Value : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = sexSearch });

            command.Where($"(@0 IS NULL OR {GetRealColumn(nameof(AccountSearch.SSN))} = @1)",
                string.IsNullOrEmpty(ssnSearchText) ? (object)DBNull.Value : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = ssnSearchText },
                string.IsNullOrEmpty(ssnSearchText) ? (object)DBNull.Value : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = ssnSearchText });

            DateTime? dobDt = null;
            if (!string.IsNullOrEmpty(dobSearch) && DateTime.TryParse(dobSearch, out DateTime parsed))
                dobDt = parsed;

            command.Where($"(@0 IS NULL OR {GetRealColumn(nameof(AccountSearch.DateOfBirth))} = @1)",
                dobDt.HasValue ? (object)new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = dobDt.Value } : DBNull.Value,
                dobDt.HasValue ? (object)new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = dobDt.Value } : DBNull.Value);

            command.OrderBy($"{GetRealColumn(nameof(AccountSearch.ServiceDate))} desc");

            return Context.Fetch<AccountSearch>(command);
        }
        catch (Exception ex)
        {
            Log.Instance.Fatal(ex, $"Exception in");
        }

        return new List<AccountSearch>();
    }

}
