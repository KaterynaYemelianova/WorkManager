using Autofac;
using BusinessLogic.Models;

using DataAccess;
using DataAccess.Entities;
using DataAccess.RepoContracts;

using Exceptions.BusinessLogic;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.ServiceContracts
{
    internal class RoleCheckService : IRoleCheckService
    {
        private static ICompanyRepo CompanyRepo = DataAccessDependencyHolder.Dependencies.Resolve<ICompanyRepo>();

        public async Task<bool> IsInRole(RoleModel role, int userId, int companyId, bool throwIfFailed = true)
        {
            CompanyEntity companyEntity = await CompanyRepo.GetById(companyId);

            if (companyEntity == null)
                return false;

            KeyValuePair<AccountEntity, RoleEntity> accountRole = companyEntity.Members.FirstOrDefault(
                member => member.Key.Id == userId
            );

            if (accountRole.Key == null || accountRole.Value == null)
                return false;

            RoleModel roleModel = (RoleModel)(accountRole.Value.Id - 1);

            if (roleModel != role && throwIfFailed)
                throw new NotAppropriateRoleException(role.ToString());

            return roleModel == role;
        }
    }
}