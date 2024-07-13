using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositories.Innterfaces;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
            dbSet = _db.Set<T>();
            //_db.Products.Include(p => p.Category);
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual IEnumerable<T> GetAll(Func<T, bool>? filter, params string[] includeProperties)
        {
            IQueryable<T> query = dbSet;
            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }

            return filter is null ? query.ToList() : query.Where(filter);
        }

        public virtual T? Get(Func<T, bool> filter, params string[] includeProperties)
        {
            IQueryable<T> query = dbSet;
            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }

            return filter is null ? query.FirstOrDefault() : query.FirstOrDefault(filter);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public abstract void Update(T t);
    }
}
