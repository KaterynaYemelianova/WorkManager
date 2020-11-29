using Autofac;

using BusinessLogic.Models;
using BusinessLogic.ServiceContracts;

using Dtos;
using Exceptions.BusinessLogic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    internal class ServiceBase
    {
        protected ISessionService SessionService = BusinessLogicDependencyHolder.Dependencies.Resolve<ISessionService>();
        protected IRoleCheckService RoleCheckService = BusinessLogicDependencyHolder.Dependencies.Resolve<IRoleCheckService>();

        protected async Task CheckSessionAndRole(SessionDto session, int companyId, params RoleEnum[] roles)
        {
            SessionService.CheckSession(session);
            RoleEnum role = await RoleCheckService.GetRole(session.UserId, companyId);

            if(!roles.Contains(role))
                throw new NotAppropriateRoleException(string.Join(" or ", roles));
        }
    }
}
