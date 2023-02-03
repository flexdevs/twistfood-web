using System.ComponentModel.DataAnnotations;

namespace TwistFood.Service.Dtos.Orders
{
    public class OrderDetailUpdateDto
    {
        [Required]
        public long OrderDetailId { get; set; }
        public long? ProductId { get; set; }
        public int? Amount { get; set; }
        public double? Price { get; set; }
    }
}
