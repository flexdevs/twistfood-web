using System.ComponentModel.DataAnnotations;
using TwistFood.Domain.Enums;

namespace TwistFood.Service.Dtos.Orders
{
    public class OrderUpdateDto
    {
        [Required]
        public long OrderId { get; set; }
        public long? DeliverId { get; set; }
        public long? OperatorId { get; set; }
        public double? TotalSum { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public OrderStatus? Status { get; set; }
    }
}
