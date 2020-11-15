using BusinessLogic.Models;

using Dtos;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.ServiceContracts
{
    public interface IAuthService
    {
        Task<AccountModel> Create(SignUpDto signUpDto);
        Task<IEnumerable<AccountModel>> GetAccounts();
    }
}
