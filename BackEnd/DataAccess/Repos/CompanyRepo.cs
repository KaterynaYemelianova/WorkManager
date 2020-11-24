using Autofac;

using DataAccess.Entities;
using DataAccess.RepoContracts;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    internal class CompanyRepo : ConnectedRepoBase<CompanyEntity>, ICompanyRepo
    {
        private static IAccountRepo AccountRepo =
            DataAccessDependencyHolder.Dependencies.Resolve<IAccountRepo>();

        private static IRoomRepo RoomRepo =
            DataAccessDependencyHolder.Dependencies.Resolve<IRoomRepo>();

        private static NoDependenciesRepo<RoleEntity> RoleRepo = 
            DataAccessDependencyHolder.Dependencies.Resolve<NoDependenciesRepo<RoleEntity>>();

        private static NoDependenciesRepo<AccountCompanyRoleEntity> AccountCompanyRoleRepo =
            DataAccessDependencyHolder.Dependencies.Resolve<NoDependenciesRepo<AccountCompanyRoleEntity>>();

        protected async override Task<IEnumerable<CompanyEntity>> LoadDependencies(IEnumerable<CompanyEntity> entities)
        {
            int[] companyIds = entities.Select(entity => entity.Id).ToArray();

            IDictionary<int, List<AccountCompanyRoleEntity>> acrs = await AccountCompanyRoleRepo.Get(
                acr => acr.CompanyId, companyIds
            );

            int[] accountIds = acrs.SelectMany(
                acrGroup => acrGroup.Value.Select(acr => acr.AccountId)
            ).Distinct().ToArray();

            int[] roleIds = acrs.SelectMany(
                acrGroup => acrGroup.Value.Select(acr => acr.RoleId)
            ).Distinct().ToArray();

            IDictionary<int, AccountEntity> accounts = await AccountRepo.GetDictionaryByIds(accountIds);
            IDictionary<int, RoleEntity> roles = await RoleRepo.GetDictionaryByIds(roleIds);

            IDictionary<int, List<RoomEntity>> rooms = await RoomRepo.Get(
                room => room.CompanyId, companyIds
            );

            foreach (CompanyEntity company in entities)
            {
                if (acrs.ContainsKey(company.Id))
                {
                    company.Members = acrs[company.Id].ToDictionary(
                        acrGroup => accounts[acrGroup.AccountId],
                        acrGroup => roles[acrGroup.RoleId]
                    );
                }

                if (rooms.ContainsKey(company.Id))
                    company.Rooms = rooms[company.Id];
            }

            return entities;
        }

        public override async Task<CompanyEntity> Update(CompanyEntity entity)
        {
            CompanyEntity updated = await base.Update(entity);

            List<AccountCompanyRoleEntity> acrsOld = await AccountCompanyRoleRepo.GetByOne(
                acr => acr.CompanyId, entity.Id
            );

            List<AccountCompanyRoleEntity> updatedAcrs = new List<AccountCompanyRoleEntity>();
            foreach (KeyValuePair<AccountEntity, RoleEntity> acr in entity.Members)
            {
                AccountCompanyRoleEntity foundAcrOld = acrsOld.FirstOrDefault(
                    acrOld => acrOld.AccountId == acr.Key.Id
                );

                AccountCompanyRoleEntity newAcr = new AccountCompanyRoleEntity()
                {
                    AccountId = acr.Key.Id,
                    RoleId = acr.Value.Id
                };

                if (foundAcrOld != null)
                    newAcr.Id = foundAcrOld.Id;

                updatedAcrs.Add(newAcr);
            }

            await UpdateCollection(acrsOld, updatedAcrs, acr => acr.CompanyId, updated);
            await UpdateCollection(updated.Rooms, entity.Rooms, room => room.CompanyId, updated);

            return await GetById(entity.Id);
        }
    }
}
