using Autofac;

using BusinessLogic.Models;
using BusinessLogic.ServiceContracts;

using DataAccess;
using DataAccess.Entities;
using DataAccess.RepoContracts;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    internal class AuthService : IAuthService
    {
        private static IAccountRepo AccountRepo = DataAccessDependencyHolder.Dependencies.Resolve<IAccountRepo>();

        public async Task<AccountModel> Create(AccountModel account)
        {
            AccountEntity entity = new AccountEntity()
            {
                Login = account.Login,
                
            };

            return account;
        }

        public async Task<IEnumerable<AccountModel>> GetAccounts()
        {
            throw new System.NotImplementedException();
        }
    }
}
