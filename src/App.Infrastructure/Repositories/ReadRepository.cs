using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Interfases;
using App.Infrastructure;

namespace App.Infrastructure.Repositories
{
    public class ReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public ReadRepository(SQLSDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<int> FindAndCountAsync(Expression<Func<TEntity, bool>> predicate)
        {

            var result = await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
            return result.Count();
        }

        public async Task<TEntity> GetByIdAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

    }

}
