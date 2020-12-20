using Autofac;

using DataAccess;
using DataAccess.Entities;
using DataAccess.RepoContracts;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUnitTestProject
{
    public class CompanyRepoTests : AbstractTest<CompanyEntity>
    {
        private IAccountRepo AccountRepo = DataAccessDependencyHolder.Dependencies.Resolve<IAccountRepo>();
        private IRepo<RoleEntity> RoleRepo = DataAccessDependencyHolder.Dependencies.Resolve<IRepo<RoleEntity>>();

        public bool AreMembersEqual(IDictionary<AccountEntity, RoleEntity> accountRoles, IDictionary<AccountEntity, RoleEntity> accountRolesOther)
        {
            return true;
        }

        public override bool AreEqual(CompanyEntity generated, CompanyEntity operated, CompanyEntity got)
        {
            bool membersOk = (generated.Members == null && operated.Members == null && got.Members == null) ||
                             (AreMembersEqual(generated.Members, operated.Members) && AreMembersEqual(operated.Members, got.Members));

            bool roomsOk = (generated.Rooms == null && operated.Rooms == null && got.Rooms == null) ||
                            RoomRepoTests.AreEqual(generated.Rooms, operated.Rooms, got.Rooms, RoomRepoTests.AreEqualCommon);

            bool extraOk = (string.IsNullOrEmpty(generated.ExtraData) && string.IsNullOrEmpty(operated.ExtraData) && string.IsNullOrEmpty(got.ExtraData)) ||
                            JsonConvert.SerializeObject(generated.ExtraData) == JsonConvert.SerializeObject(operated.ExtraData) &&
                            JsonConvert.SerializeObject(operated.ExtraData) == JsonConvert.SerializeObject(got.ExtraData);

            return generated.Name == operated.Name && 
                   operated.Name == got.Name &&
                   membersOk && roomsOk && extraOk;
        }

        public override async Task<CompanyEntity> GetInstering()
        {
            IEnumerable<AccountEntity> accounts = await AccountRepo.Get();
            IEnumerable<RoleEntity> roles = await RoleRepo.Get();

            return new CompanyEntity()
            {
                Name = "Test company",
                Members = new Dictionary<AccountEntity, RoleEntity>()
                 {
                     { accounts.ElementAt(0), roles.ElementAt(2) },
                     { accounts.ElementAt(1), roles.ElementAt(2) }
                 },
                Rooms = new List<RoomEntity>()
                 {
                     new RoomEntity()
                     {
                         Name = "hall",
                         Height = 3,
                         Square = 50
                     },
                     new RoomEntity()
                     {
                         Name = "toilet",
                         Height = 3,
                         Square = 20,
                         EnterPoints = new List<EnterPointEntity>()
                         {
                             new EnterPointEntity()
                             {
                                  PassCondition = "{user.role} >= 2 && {control[0].pin[0].data.avg()} < 0.1 && {roomloc.count() < 5}"
                             }
                         }
                     }
                 }
            };
        }

        public override async Task<CompanyEntity> GetUpdating()
        {
            IEnumerable<AccountEntity> accounts = await AccountRepo.Get(1);
            IEnumerable<RoleEntity> roles = await RoleRepo.Get();

            return new CompanyEntity()
            {
                Id = Inserted.Id,
                Name = "TestCompanyUpdated",
                Members = new Dictionary<AccountEntity, RoleEntity>()
                {
                    { accounts.ElementAt(0), roles.ElementAt(2) }
                },
                Rooms = new List<RoomEntity>()
                {
                    new RoomEntity()
                    {
                        Id = Inserted.Rooms.ElementAt(1).Id,
                        Name = "WC",
                        Height = 3.1f,
                        Square = 19
                    },
                    new RoomEntity()
                    {
                        Name = "Room1",
                        Height = 2.7f,
                        Square = 30
                    }
                }
            };
        }
    }
}
