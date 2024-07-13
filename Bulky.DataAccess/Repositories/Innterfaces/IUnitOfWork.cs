using Bulky.Models;

namespace Bulky.DataAccess.Repositories.Innterfaces
{
    public interface IUnitOfWork
    {
        IRepository<Category> Category { get; }
        IRepository<Product> Product { get; }
        void Save();
    }
}
