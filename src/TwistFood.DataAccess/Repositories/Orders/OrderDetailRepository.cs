using TwistFood.Api.DbContexts;
using TwistFood.DataAccess.Interfaces.Orders;
using TwistFood.Domain.Entities.Order;

namespace TwistFood.DataAccess.Repositories.Orders
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
        public IQueryable<OrderDetail> GetAll(long orderId)
        => _dbSet.Where(x => x.OrderId == orderId);
    }
}

