using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ViteReact.Database;

namespace ViteReact.Shared.Repository
{
    public abstract class Repository<TEntity, TReadDTO> : IRepository<TEntity, TReadDTO> where TEntity : IBaseEntity
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public abstract TReadDTO ParseToRead(TEntity entity);

        public IQueryable<TEntity> Get(out int count)
        {
            IQueryable<TEntity> entities = _context.Set<TEntity>().OrderBy(entity => entity.Id);
            count = entities.Count();

            return entities;
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, out int count)
        {
            IQueryable<TEntity> entities = _context.Set<TEntity>()
                .OrderBy(entity => entity.Id)
                .Where(predicate);
                
            count = entities.Count();

            return entities;
        }

        public TEntity GetOne(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void Update<TData>(TEntity entity, TData data)
        {
            foreach (PropertyInfo prop in data.GetType().GetProperties())
            {
                if (prop.GetValue(data) is null) continue;

                entity
                    .GetType()
                    .GetProperty(prop.Name)
                    .SetValue(entity, prop.GetValue(data));
            }
        }
        public bool SaveChanges()
        {
            var rows = _context.SaveChanges();

            return (rows > 0);
        }

        public IEnumerable<TEntity> GetAll(out int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, out int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetInRange(int skip, int limit, out int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetInRange(int skip, int limit, Expression<Func<TEntity, bool>> predicate, out int count)
        {
            throw new NotImplementedException();
        }
    }
}