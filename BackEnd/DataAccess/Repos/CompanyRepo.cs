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
                company.Members = acrs[company.Id].ToDictionary(
                    acrGroup => accounts[acrGroup.AccountId],
                    acrGroup => roles[acrGroup.RoleId]
                );

                company.Rooms = rooms[company.Id];
            }

            return entities;
        }

        protected override async Task<int> InsertEntity(CompanyEntity entity)
        {
            int id = await base.InsertEntity(entity);

            foreach (KeyValuePair<AccountEntity, RoleEntity> accountRole in entity.Members)
                await AccountCompanyRoleRepo.Insert(
                    GetAccountCompanyRole(accountRole, entity.Id)
                );

            foreach(RoomEntity room in entity.Rooms)
                await RoomRepo.Insert(room);

            return id;
        }

        public override async Task<CompanyEntity> Update(CompanyEntity entity)
        {
            CompanyEntity updated = await base.Update(entity);

            //TODO
            //await UpdateCollection(updated.Members.Keys, entity.Members.Keys);
        }

        private AccountCompanyRoleEntity GetAccountCompanyRole(KeyValuePair<AccountEntity, RoleEntity> accountRole, int companyId)
        {
            return new AccountCompanyRoleEntity()
            {
                AccountId = accountRole.Key.Id,
                RoleId = accountRole.Value.Id,
                CompanyId = companyId
            };
        }
    }
}
