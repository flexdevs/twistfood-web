using TwistFood.Api.DbContexts;
using TwistFood.DataAccess.Interfaces.Orders;
using TwistFood.Domain.Entities.Order;

namespace TwistFood.DataAccess.Repositories.Orders
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
    }
}
