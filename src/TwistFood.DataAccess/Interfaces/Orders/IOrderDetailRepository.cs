using TwistFood.Domain.Entities.Order;

namespace TwistFood.DataAccess.Interfaces.Orders
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {
        IQueryable<OrderDetail> GetAll(long orderId);
    }
}
