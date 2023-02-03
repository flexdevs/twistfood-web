using OnlineMarket.Domain.Common;

namespace TwistFood.Service.ViewModels.Products
{
    public class ProductViewModel : BaseEntity
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public double Price { get; set; }
        public string ImagePath { get; set; } = string.Empty;

        public int Amount { get; set; }
    }
}
