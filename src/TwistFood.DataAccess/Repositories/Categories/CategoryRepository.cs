using TwistFood.Api.DbContexts;
using TwistFood.DataAccess.Interfaces.Categories;
using TwistFood.Domain.Entities.Categories;

namespace TwistFood.DataAccess.Repositories.Categories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
