using TwistFood.Domain.Common;

namespace TwistFood.Service.ViewModels.Orders
{
    public class OrderWithOrderDetailsViewModel : Auditable
    {
        public string UserPhoneNumber { get; set; } = string.Empty;
        public double TotalSum { get; set; }
        public string paymentType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<OrderDetailForAdminViewModel> OrderDetails { get; set; }
    }
}
