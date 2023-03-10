using System.Globalization;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Dtos.Orders;
using TwistFood.Service.ViewModels.Orders;

namespace TwistFood.Service.Interfaces.Orders
{
    public interface IOrderService
    {
        public Task<long> OrderCreateAsync(OrderCreateDto dto);
        public Task<bool> OrderUpdateAsync(OrderUpdateDto dto);
        public Task<PagedList<OrderViewModel>> GetAllAsync(PagenationParams @params);
        public Task<PagedList<OrderViewModel>> GetAllForSearchAsync(string search,PagenationParams @params);
        public Task<OrderWithOrderDetailsViewModel> GetOrderWithOrderDetailsAsync(long OrderId);
        public Task<bool> DeleteAsync(long OrderId);
    }
}
