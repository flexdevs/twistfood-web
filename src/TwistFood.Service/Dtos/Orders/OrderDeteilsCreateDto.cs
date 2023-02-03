using System.ComponentModel.DataAnnotations;
using TwistFood.Service.Attributes;

namespace TwistFood.Service.Dtos.Orders
{
    public class OrderDeteilsCreateDto
    {
        [Required]
        public long ProductId { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required, Integer]
        public double Price { get; set; }



    }
}
