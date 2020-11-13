using BusinessLogic.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.ServiceContracts
{
    public interface IAuthService
    {
        Task<AccountModel> Create(AccountModel account);
        Task<IEnumerable<AccountModel>> GetAccounts();
    }
}
