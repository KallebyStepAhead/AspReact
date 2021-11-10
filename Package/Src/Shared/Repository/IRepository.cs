using System;
using System.Linq;
using System.Linq.Expressions;

namespace ViteReact.Shared.Repository
{
    public interface IRepository<TEntity, TReadDTO>
    {
        TReadDTO ParseToRead(TEntity entity);

        IQueryable<TEntity> Get(out int count);

        TEntity Find(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, out int count);


        TEntity GetOne(int id);

        void Add(TEntity entity);

        void Remove(TEntity entity);

        void Update<TData>(TEntity entity, TData data);

        bool SaveChanges();
    }
}
