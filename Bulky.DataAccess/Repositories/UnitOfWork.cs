using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositories.Innterfaces;
using Bulky.Models;

namespace Bulky.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _db = dbContext;
            Category = new CategoryRepository(dbContext);
            Product = new ProductsRepository(dbContext);
        }
        public IRepository<Category> Category { get; set; }

        public IRepository<Product> Product { get; set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
