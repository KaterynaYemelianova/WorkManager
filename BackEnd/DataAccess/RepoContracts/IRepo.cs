using DataAccess.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.RepoContracts
{
    public interface IRepo<TEntity> where TEntity : EntityBase, new()
    {
        Task<IEnumerable<TEntity>> Get();
        Task<IEnumerable<TEntity>> Get(int limit);
        Task<List<TEntity>> GetByOne<T>(Expression<Func<TEntity, T>> expression, int limit, T value);
        Task<List<TEntity>> GetByOne<T>(Expression<Func<TEntity, T>> expression, T value);
        Task<IDictionary<T, List<TEntity>>> Get<T>(Expression<Func<TEntity, T>> expression, int limit, params T[] value);
        Task<IDictionary<T, List<TEntity>>> Get<T>(Expression<Func<TEntity, T>> expression, params T[] values);
        Task<IEnumerable<TEntity>> Get<T>(IDictionary<Expression<Func<TEntity, T>>, T> wheres);

        Task<TEntity> GetById(int id);
        Task<IEnumerable<TEntity>> GetByIds(params int[] ids);
        Task<IDictionary<int, TEntity>> GetDictionaryByIds(params int[] ids);
        Task<TEntity> FirstOrDefault<T>(Expression<Func<TEntity, T>> expression, T value);

        Task<TEntity> Insert(TEntity entity);
        Task<TEntity> Update(TEntity entity);

        Task Delete(params int[] ids);
        Task Delete<T>(Expression<Func<TEntity, T>> expression, params T[] values);
        Task Delete<T>(IDictionary<Expression<Func<TEntity, T>>, T> wheres);
    }
}
