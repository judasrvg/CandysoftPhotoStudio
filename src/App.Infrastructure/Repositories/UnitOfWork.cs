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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SQLSDBContext _context;
        private readonly Dictionary<Type, object> _readRepositories;
        private readonly Dictionary<Type, object> _writeRepositories;

        public UnitOfWork(SQLSDBContext context)
        {
            _context = context;
            _readRepositories = new Dictionary<Type, object>();
            _writeRepositories = new Dictionary<Type, object>();
        }

        public IReadRepository<TEntity> ReadRepository<TEntity>() where TEntity : class
        {
            if (_readRepositories.ContainsKey(typeof(TEntity)))
                return _readRepositories[typeof(TEntity)] as IReadRepository<TEntity>;

            var repository = new ReadRepository<TEntity>(_context);
            _readRepositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public IWriteRepository<TEntity> WriteRepository<TEntity>() where TEntity : class
        {
            if (_writeRepositories.ContainsKey(typeof(TEntity)))
                return _writeRepositories[typeof(TEntity)] as IWriteRepository<TEntity>;

            var repository = new WriteRepository<TEntity>(_context);
            _writeRepositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
