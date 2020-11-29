using Autofac;
using BusinessLogic.Models;

using DataAccess;
using DataAccess.Entities;
using DataAccess.RepoContracts;

using Exceptions.BusinessLogic;
using Exceptions.DataAccess;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.ServiceContracts
{
    internal class RoleCheckService : IRoleCheckService
    {
        private static ICompanyRepo CompanyRepo = DataAccessDependencyHolder.Dependencies.Resolve<ICompanyRepo>();

        public async Task<RoleEnum> GetRole(int userId, int companyId)
        {
            CompanyEntity companyEntity = await CompanyRepo.GetById(companyId);

            if (companyEntity == null)
                throw new NotFoundException("Company was not found");

            KeyValuePair<AccountEntity, RoleEntity> accountRole = companyEntity.Members.FirstOrDefault(
                member => member.Key.Id == userId
            );

            if (accountRole.Key == null || accountRole.Value == null)
                throw new NotFoundException("Role was not found");

            return (RoleEnum)accountRole.Value.Id;
        }

        public RoleEnum[] GetUpcheckingRoles(RoleEnum role)
        {
            if (role == RoleEnum.DIRECTOR || role == RoleEnum.MANAGER)
                return new RoleEnum[] { RoleEnum.DIRECTOR, RoleEnum.SUPERADMIN };

            if (role == RoleEnum.SUPERADMIN)
                return new RoleEnum[] { RoleEnum.SUPERADMIN };

            return new RoleEnum[] { RoleEnum.MANAGER, RoleEnum.DIRECTOR, RoleEnum.SUPERADMIN };
        }

        public async Task<bool> IsInRole(RoleEnum role, int userId, int companyId, bool throwIfFailed = true)
        {
            RoleEnum roleModel = await GetRole(userId, companyId);

            if (roleModel != role && throwIfFailed)
                throw new NotAppropriateRoleException(role.ToString());

            return roleModel == role;
        }
    }
}