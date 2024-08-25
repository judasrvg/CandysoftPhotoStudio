using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Interfases;
using App.Infrastructure;

namespace App.Infrastructure.Repositories
{
    public class WriteRepository<TEntity> : IWriteRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public WriteRepository(SQLSDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        //public async Task AddAsync(TEntity entity)
        //{
        //    await _dbSet.AddAsync(entity);
        //}

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync(); // Guarda los cambios para que el ID sea generado
            return entity; // Devuelve la entidad con el ID generado
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task BulkInsertAsync(IEnumerable<TEntity> entities)
        {
            await _context.BulkInsertAsync(entities.ToList());
        }

        public async Task BulkUpdateAsync(IEnumerable<TEntity> entities)
        {
            await _context.BulkUpdateAsync(entities.ToList());
        }
    }


}
