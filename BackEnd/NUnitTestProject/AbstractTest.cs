using Autofac;
using DataAccess;
using DataAccess.Entities;
using DataAccess.RepoContracts;

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUnitTestProject
{
    public abstract class AbstractTest<TEntity> where TEntity : EntityBase, new()
    {
        public IRepo<TEntity> Repo = DataAccessDependencyHolder.Dependencies.Resolve<IRepo<TEntity>>();

        public TEntity Inserting { get; set; }
        public TEntity Inserted { get; set; }

        public TEntity Updating { get; set; }
        public TEntity Updated { get; set; }

        public abstract Task<TEntity> GetInstering();
        public abstract Task<TEntity> GetUpdating();

        public abstract bool AreEqual(TEntity generated, TEntity operated, TEntity got);

        public static bool AreEqual(
            IEnumerable<TEntity> generated, IEnumerable<TEntity> operated, IEnumerable<TEntity> got,
            Func<TEntity, TEntity, TEntity, bool> comparer
        ) {
            if (generated == null && operated == null && got == null)
                return true;

            if (generated?.Count() != operated?.Count() || operated?.Count() != got?.Count())
                return false;

            int count = generated.Count();
            return Enumerable.Range(0, count).All(
                i => comparer(
                    generated.ElementAt(i), 
                    operated.ElementAt(i), 
                    got.ElementAt(i)
                )
            );
        }

        [Test]
        [Order(1)]
        public async Task TestInsert()
        {
            Inserting = await GetInstering();

            try
            {
                Inserted = await Repo.Insert(Inserting);
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
                TEntity got = await Repo.GetById(Inserted.Id);
                Assert.IsTrue(AreEqual(Inserting, Inserted, got));
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
            Updating = await GetUpdating();
            try
            {
                Updated = await Repo.Update(Updating);
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
                TEntity got = await Repo.GetById(Updated.Id);
                Assert.IsTrue(AreEqual(Updating, Updated, got));
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
                await Repo.Delete(Inserted.Id);
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
                TEntity got = await Repo.GetById(Inserted.Id);
                Assert.IsTrue(got == null);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }
    }
}
