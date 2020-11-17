using Autofac;

using DataAccess.Entities;
using DataAccess.RepoContracts;

using System.Collections.Generic;
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

        protected async override Task<CompanyEntity> LoadDependencies(CompanyEntity entity)
        {
            IEnumerable<AccountCompanyRoleEntity> accountCompanyRoles = 
                await AccountCompanyRoleRepo.Get(ent => ent.CompanyId, entity.Id);

            entity.Members = new Dictionary<AccountEntity, RoleEntity>();
            foreach (AccountCompanyRoleEntity accCompRole in accountCompanyRoles)
                entity.Members.Add(
                    await AccountRepo.GetById(accCompRole.AccountId),
                    await RoleRepo.GetById(accCompRole.RoleId)
                );

            entity.Rooms = await RoomRepo.Get(room => room.CompanyId, entity.Id);

            return entity;
        }

        protected override async Task<int> InsertEntity(CompanyEntity entity)
        {
            int id = await base.InsertEntity(entity);

            foreach (KeyValuePair<AccountEntity, RoleEntity> accountRole in entity.Members)
            {
                AccountCompanyRoleEntity accountCompanyRoleEntity = new AccountCompanyRoleEntity()
                {
                    AccountId = accountRole.Key.Id,
                    RoleId = accountRole.Value.Id,
                    CompanyId = entity.Id
                };

                await AccountCompanyRoleRepo.Insert(accountCompanyRoleEntity);
            }

            foreach(RoomEntity room in entity.Rooms)
                await RoomRepo.Insert(room);

            return id;
        }
    }
}
