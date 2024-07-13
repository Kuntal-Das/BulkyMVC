using Bulky.DataAccess.Data;
using Bulky.Models;

namespace Bulky.DataAccess.Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public override void Update(Category t)
        {
            _db.Update(t);
        }
    }
}
