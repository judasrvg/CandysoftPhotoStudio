using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfases
{
    public interface IReadRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> FindAndCountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetByIdAsync(params object[] keyValues);
    }

    public interface IWriteRepository<TEntity> where TEntity : class
    {
        //Task AddAsync(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task BulkInsertAsync(IEnumerable<TEntity> entities);
        Task BulkUpdateAsync(IEnumerable<TEntity> entities);
    }

    public interface IUnitOfWork : IDisposable
    {
        IReadRepository<TEntity> ReadRepository<TEntity>() where TEntity : class;
        IWriteRepository<TEntity> WriteRepository<TEntity>() where TEntity : class;
        Task<int> CompleteAsync();
    }


}
