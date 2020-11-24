using BusinessLogic.Models;
using System.Threading.Tasks;

namespace BusinessLogic.ServiceContracts
{
    internal interface IRoleCheckService
    {
        Task<bool> IsInRole(RoleEnum role, int userId, int companyId, bool throwIfFailed = true);
    }
}
