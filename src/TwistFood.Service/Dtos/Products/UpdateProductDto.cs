using Microsoft.AspNetCore.Http;
using TwistFood.Domain.Entities.Products;
using TwistFood.Service.Attributes;

namespace TwistFood.Service.Dtos.Products
{
    public class UpdateProductDto
    {
        public long? ProductId { get; set; }
        public long? CategoryId { get; set; }
        public string? ProductName { get; set; }

        public string? ProductDescription { get; set; }

        [MaxFileSize(2), AllowedFiles(new string[] { ".jpg", ".png", ".jpeg", ".svg", ".webp" })]
        public IFormFile? Image { get; set; }
        [Integer]
        public double? Price { get; set; }

        public static implicit operator Product(UpdateProductDto dto)
        {
            return new Product()
            {
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
