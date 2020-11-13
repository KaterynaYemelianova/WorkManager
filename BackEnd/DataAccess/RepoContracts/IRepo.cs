using DataAccess.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.RepoContracts
{
    public interface IRepo<TEntity> where TEntity : EntityBase, new()
    {
        Task<IEnumerable<TEntity>> Get();
        Task<IEnumerable<TEntity>> Get(int limit);
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, object>> expression, object value);
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, object>> expression, object value, int limit);

        Task<TEntity> GetById(int id);
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, object>> expression, object value);

        Task<TEntity> Insert(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task Delete(int id);
    }
}
