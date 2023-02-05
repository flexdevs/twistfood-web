using TwistFood.Domain.Entities.Products;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Dtos.Products;
using TwistFood.Service.ViewModels.Products;

namespace TwistFood.Service.Interfaces.Products
{
    public interface IProductService
    {
        public Task<bool> CreateProductAsync(CreateProductsDto createProductsDto);
        public Task<PagedList<ProductViewModel>> GetAllAsync(PagenationParams @params);
        public Task<PagedList<ProductViewModel>> SearchByNameAsync(string name, PagenationParams @params);

        public Task<ProductViewModel> GetAsync(long id);

        public Task<UpdateProductDto> GetForUpdateAsync(long id);
        public Task<bool> DeleteAsync(long id);

        public Task<bool> UpdateAsync(long id, UpdateProductDto updateProductDto);
        public Task<IEnumerable<Product>> GetAllForSearchAsync(string categoryName, string searchName);

    }
}
