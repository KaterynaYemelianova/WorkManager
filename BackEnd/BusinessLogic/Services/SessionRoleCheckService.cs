using Autofac;

using BusinessLogic.Models;
using BusinessLogic.ServiceContracts;

using Dtos;

using Exceptions.BusinessLogic;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    internal class SessionRoleCheckService
    {
        public ISessionService SessionService = BusinessLogicDependencyHolder.Dependencies.Resolve<ISessionService>();
        public IRoleCheckService RoleCheckService = BusinessLogicDependencyHolder.Dependencies.Resolve<IRoleCheckService>();

        public async Task CheckSessionAndRole(SessionDto session, int companyId, bool acceptSuperadmin, params RoleEnum[] roles)
        {
            SessionService.CheckSession(session);

            if (acceptSuperadmin && await RoleCheckService.IsSuperadmin(session.UserId))
                return;

            RoleEnum role = await RoleCheckService.GetRole(session.UserId, companyId);

            if (!roles.Contains(role))
                throw new NotAppropriateRoleException(string.Join(" or ", roles));
        }

        public async Task CheckSessionAndRole(SessionDto session, int companyId, params RoleEnum[] roles)
        {
            await CheckSessionAndRole(session, companyId, true, roles);
        }
    }
}
