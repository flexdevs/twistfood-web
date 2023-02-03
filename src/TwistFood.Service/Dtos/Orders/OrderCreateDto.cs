using System.ComponentModel.DataAnnotations;
using TwistFood.Domain.Entities.Order;
using TwistFood.Domain.Enums;
using TwistFood.Service.Attributes;

namespace TwistFood.Service.Dtos.Orders
{
    public class OrderCreateDto
    {
        [Required, Integer]
        public double Latitude { get; set; }
        [Required, Integer]
        public double Longitude { get; set; }
        [Required, Integer]
        public double DeliverPrice { get; set; }
        [Required]
        public bool IsDiscount { get; set; }
        public long? DiscountId { get; set; }

        public static implicit operator Order(OrderCreateDto dto)
        {
            return new Order()
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DeleviryPrice = dto.DeliverPrice,
                IsDiscount = dto.IsDiscount,
                PaymentType = PaymentType.Cash,
                Status = OrderStatus.InQueue,
                TotalSum = 0

            };
        }
    }
}
