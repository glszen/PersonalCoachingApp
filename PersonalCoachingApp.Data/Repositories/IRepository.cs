using System;
using System.Linq.Expressions;

namespace PersonalCoachingApp.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(int id);

        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity, bool softDelete = true);

        void Delete(int id);

        TEntity Get(Expression<Func<TEntity, bool>> predicate);
    }
}
