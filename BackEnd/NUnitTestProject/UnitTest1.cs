using Autofac;

using DataAccess;
using DataAccess.Entities;
using DataAccess.RepoContracts;

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

namespace NUnitTestProject
{
    public class CompanyRepoTests
    {
        private IAccountRepo AccountRepo = DataAccessDependencyHolder.Dependencies.Resolve<IAccountRepo>();
        private ICompanyRepo CompanyRepo = DataAccessDependencyHolder.Dependencies.Resolve<ICompanyRepo>();
        private IRepo<RoleEntity> RoleRepo = DataAccessDependencyHolder.Dependencies.Resolve<IRepo<RoleEntity>>();

        private CompanyEntity InsertingCompany = null;
        private CompanyEntity InsertedCompany = null;
        private CompanyEntity UpdatingCompany = null;
        private CompanyEntity UpdatedCompany = null;

        private async Task<CompanyEntity> GetInsertingEntity()
        {
            IEnumerable<AccountEntity> accounts = await AccountRepo.Get();
            IEnumerable<RoleEntity> roles = await RoleRepo.Get();

            return new CompanyEntity()
            {
                 Name = "Test company",
                 Members = new Dictionary<AccountEntity, RoleEntity>()
                 {
                     { accounts.ElementAt(0), roles.ElementAt(3) },
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
                         Square = 20
                     }
                 }
            };
        }

        private async Task<CompanyEntity> GetUpdatingEntity()
        {
            IEnumerable<AccountEntity> accounts = await AccountRepo.Get(1);
            IEnumerable<RoleEntity> roles = await RoleRepo.Get();

            return new CompanyEntity()
            {
                Id = InsertedCompany.Id,
                Name = "TestCompanyUpdated",
                Members = new Dictionary<AccountEntity, RoleEntity>()
                {
                    { accounts.ElementAt(0), roles.ElementAt(2) }
                },
                Rooms = new List<RoomEntity>()
                {
                    new RoomEntity()
                    {
                        Id = InsertedCompany.Rooms.ElementAt(1).Id,
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

        [Test]
        [Order(1)]
        public async Task TestInsert()
        {
            InsertingCompany = await GetInsertingEntity();

            try
            {
                InsertedCompany = await CompanyRepo.Insert(InsertingCompany);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [Test]
        [Order(2)]
        public async Task TestGetInserted()
        {
            try
            {
                CompanyEntity gottenCompany = await CompanyRepo.GetById(InsertedCompany.Id);
                bool condition = InsertingCompany.Members.Count == gottenCompany.Members.Count
                    && InsertingCompany.Name == gottenCompany.Name
                    && InsertingCompany.Rooms.Count() == gottenCompany.Rooms.Count();
                Assert.IsTrue(condition);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [Test]
        [Order(3)]
        public async Task TestUpdate()
        {
            UpdatingCompany = await GetUpdatingEntity();
            try
            {
                UpdatedCompany = await CompanyRepo.Update(UpdatingCompany);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [Test]
        [Order(4)]
        public async Task TestGetUpdated()
        {
            try
            {
                CompanyEntity gottenCompany = await CompanyRepo.GetById(UpdatedCompany.Id);
                bool condition = UpdatingCompany.Members.Count == gottenCompany.Members.Count
                    && UpdatingCompany.Name == gottenCompany.Name
                    && UpdatingCompany.Rooms.Count() == gottenCompany.Rooms.Count();
                Assert.IsTrue(condition);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [Test]
        [Order(5)]
        public async Task TestDelete()
        {
            try
            {
                await CompanyRepo.Delete(InsertedCompany.Id);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [Test]
        [Order(6)]
        public async Task TestGetDeleted()
        {
            try
            {
                CompanyEntity gottenCompany = await CompanyRepo.GetById(UpdatedCompany.Id);

                Assert.IsTrue(gottenCompany == null);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }
    }
}