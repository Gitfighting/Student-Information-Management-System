using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace StudentManageSystem.DAL
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public void Add(TEntity entity) => _dbSet.Add(entity);

        public void Update(TEntity entity)
        {
            // 获取实体的主键名称（EF6 方式）
            var objectContext = ((IObjectContextAdapter)_context).ObjectContext;
            var objectSet = objectContext.CreateObjectSet<TEntity>();
            var keyNames = objectSet.EntitySet.ElementType.KeyMembers.Select(k => k.Name).ToArray();

            // 获取当前实体的主键值
            var entityEntry = _context.Entry(entity);
            var keyValues = keyNames.Select(keyName => entityEntry.Property(keyName).CurrentValue).ToArray();

            // 在 Local 中查找是否有相同主键的实体已被跟踪
            var trackedEntity = _dbSet.Local.FirstOrDefault(e =>
            {
                var trackedEntry = _context.Entry(e);
                var trackedKeyValues = keyNames.Select(keyName => trackedEntry.Property(keyName).CurrentValue).ToArray();
                return keyValues.SequenceEqual(trackedKeyValues);
            });

            // 如果找到已跟踪的实体，先分离它
            if (trackedEntity != null && trackedEntity != entity)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            // 附加并标记为修改
            if (entityEntry.State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity) => _dbSet.Remove(entity);

        public void DeleteById(object id)
        {
            var entity = GetById(id);
            if (entity != null) _dbSet.Remove(entity);
        }

        public TEntity GetById(object id) => _dbSet.Find(id);

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate) => _dbSet.FirstOrDefault(predicate);

        public IList<TEntity> GetAll() => _dbSet.ToList();

        public IList<TEntity> Where(Expression<Func<TEntity, bool>> predicate) => _dbSet.Where(predicate).ToList();

        public bool Exists(Expression<Func<TEntity, bool>> predicate) => _dbSet.Any(predicate);

        public int Count(Expression<Func<TEntity, bool>> predicate = null) =>
            predicate == null ? _dbSet.Count() : _dbSet.Count(predicate);
    }
}
