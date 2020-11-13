using DataAccess.Entities;
using DataAccess.RepoContracts;

using System.Threading.Tasks;

namespace DataAccess.Repos
{
    internal class AccountRepo : ConnectedRepoBase<AccountEntity>, IAccountRepo
    {
        public async Task<AccountEntity> GetByLogin(string login)
        {
            return await FirstOrDefault(account => account.Login, login);
        }
    }
}
