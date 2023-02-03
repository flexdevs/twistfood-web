using TwistFood.Domain.Entities.Categories;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Dtos;
using TwistFood.Service.ViewModels.Categories;

namespace TwistFood.Service.Interfaces.Categories
{
    public interface ICategoryService
    {
        public Task<bool> CreateCategoryAsync(CategoryDto categoryDto);

        public Task<PagedList<Category>> GetAllAsync(PagenationParams @params);

        public Task<CategoryViewModels> GetAsync(long id);

        public Task<bool> DeleteAsync(long id);

        public Task<bool> UpdateAsync(long Id, CategoryViewModels categoryView);
    }
}
