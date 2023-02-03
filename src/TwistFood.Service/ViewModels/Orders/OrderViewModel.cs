using TwistFood.Domain.Common;

namespace TwistFood.Service.ViewModels.Orders
{
    public class OrderViewModel : Auditable
    {
        public string UserPhoneNumber { get; set; } = string.Empty;
        public double TotalSum { get; set; }
        public string paymentType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public IEnumerable<string> OrderDetails { get; set; } = default!;
        public long @operatorId { get; set; }

        public long deliverId { get; set; }

    }
}
