namespace Bulky.DataAccess.Repositories.Innterfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Func<T, bool>? filter = null, params string[] includeProperties);
        T? Get(Func<T, bool> filter, params string[] includeProperties);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T t);
    }
}
