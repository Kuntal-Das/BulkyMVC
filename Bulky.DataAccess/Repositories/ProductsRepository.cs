using Bulky.DataAccess.Data;
using Bulky.Models;

namespace Bulky.DataAccess.Repositories
{
    internal class ProductsRepository : Repository<Product>
    {
        public ProductsRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public override void Update(Product t)
        {
            _db.Update(t);
        }
    }
}
