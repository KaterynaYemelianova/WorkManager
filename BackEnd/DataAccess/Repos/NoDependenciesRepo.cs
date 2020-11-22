using DataAccess.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    internal class NoDependenciesRepo<TEntity> : ConnectedRepoBase<TEntity> where TEntity : EntityBase, new()
    {
        protected override async Task<IEnumerable<TEntity>> LoadDependencies(IEnumerable<TEntity> entities)
        {
            return entities;
        }
    }
}
