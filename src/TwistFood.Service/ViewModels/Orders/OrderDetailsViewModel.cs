using OnlineMarket.Domain.Common;

namespace TwistFood.Service.ViewModels.Orders
{
    public class OrderDetailViewModel : BaseEntity
    {
        public string ProductName { get; set; } = string.Empty;
        public double Price { get; set; }

    }
}
