using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.RepoContracts
{
    public interface IRepo<TEntity> where TEntity : IEntity
    {
        Task<IEnumerable<TEntity>> Get(int limit);
        Task<IEnumerable<TEntity>> Get();
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, object>> expression, object value, int limit);
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, object>> expression, object value);
        Task<IEnumerable<TEntity>> Insert();
        Task<IEnumerable<TEntity>> Update();
        Task<IEnumerable<TEntity>> Delete();

    }
}
