using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StudentManageSystem.DAL
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void DeleteById(object id);

        TEntity GetById(object id);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        IList<TEntity> GetAll();
        IList<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

        bool Exists(Expression<Func<TEntity, bool>> predicate);
        int Count(Expression<Func<TEntity, bool>> predicate = null);
    }
}
