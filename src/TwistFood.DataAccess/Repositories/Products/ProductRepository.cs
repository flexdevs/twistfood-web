using TwistFood.Api.DbContexts;
using TwistFood.DataAccess.Interfaces.Products;
using TwistFood.Domain.Entities.Products;

namespace TwistFood.DataAccess.Repositories.Products
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
    }
}
