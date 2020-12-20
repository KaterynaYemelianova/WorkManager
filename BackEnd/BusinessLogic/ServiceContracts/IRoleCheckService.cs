using BusinessLogic.Models;
using System.Threading.Tasks;

namespace BusinessLogic.ServiceContracts
{
    internal interface IRoleCheckService
    {
        Task<bool> IsSuperadmin(int userId);
        Task<bool> IsInRole(RoleEnum role, int userId, int companyId, bool throwIfFailed = true);
        Task<RoleEnum> GetRole(int userId, int companyId);
        RoleEnum[] GetUpcheckingRoles(RoleEnum role);
    }
}
