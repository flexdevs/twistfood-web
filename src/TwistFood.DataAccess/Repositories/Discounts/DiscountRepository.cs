using TwistFood.Api.DbContexts;
using TwistFood.DataAccess.Interfaces.Discounts;
using TwistFood.Domain.Entities.Discounts;

namespace TwistFood.DataAccess.Repositories.Discounts
{
    public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        public DiscountRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
    }
}
