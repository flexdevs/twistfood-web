using OnlineMarket.Domain.Common;
using TwistFood.Service.ViewModels.Products;

namespace TwistFood.Service.ViewModels.Categories
{
    public class CategoryViewModels : BaseEntity
    {
        public string CategoryName { get; set; } = string.Empty;
        public List<ProductViewModel>? Products { get; set; }

    }
}
