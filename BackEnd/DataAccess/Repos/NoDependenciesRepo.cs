using DataAccess.Entities;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    internal class NoDependenciesRepo<TEntity> : ConnectedRepoBase<TEntity> where TEntity : EntityBase, new()
    {
        protected override async Task<TEntity> LoadDependencies(TEntity entity)
        {
            return entity;
        }
    }
}
