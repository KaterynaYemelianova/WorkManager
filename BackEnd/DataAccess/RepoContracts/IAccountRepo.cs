using DataAccess.Entities;

using System.Threading.Tasks;

namespace DataAccess.RepoContracts
{
    public interface IAccountRepo : IRepo<AccountEntity>
    {
        Task<AccountEntity> GetByLogin(string login);
    }
}
