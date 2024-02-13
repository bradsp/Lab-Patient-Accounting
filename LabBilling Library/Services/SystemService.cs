using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using Microsoft.AspNetCore.CookiePolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Services
{
    public class SystemService
    {
        private IAppEnvironment appEnvironment;

        public SystemService(IAppEnvironment appEnvironment)
        {
            this.appEnvironment = appEnvironment;
        }

        public ApplicationParameters LoadSystemParameters()
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.SystemParametersRepository.LoadParameters();
        }

        public void SaveSystemParameter(SysParameter systemParameter)
        {
            using UnitOfWorkMain uow = new(appEnvironment, true);

            uow.SystemParametersRepository.Update(systemParameter, new[] { nameof(SysParameter.Value) });
        }

        public bool UpdateUser(UserAccount user)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            var retval = uow.UserAccountRepository.Update(user);
            uow.Commit();
            return retval;
        }

        public UserAccount AddUser(UserAccount user)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            var retval = uow.UserAccountRepository.Add(user);
            uow.Commit();

            return user;
        }

        public IList<UserAccount> GetUsers()
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.UserAccountRepository.GetAll();

        }

        public UserAccount GetUser(string username)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.UserAccountRepository.GetByUsername(username);
        }

        public IList<UserAccount> GetActiveUsers()
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.UserAccountRepository.GetActiveUsers();
        }

        public bool LoginCheck(string username, string password) 
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.UserAccountRepository.LoginCheck(username, password);
        }
    }
}
