using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace StudentManageSystem.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new RepositoryBase<TEntity>(_context);
            }
            return (IRepository<TEntity>)_repositories[type];
        }

        public int SaveChanges() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}
