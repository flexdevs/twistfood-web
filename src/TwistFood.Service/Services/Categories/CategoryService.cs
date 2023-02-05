using System.Data.Entity;
using System.Net;
using TwistFood.DataAccess.Interfaces;
using TwistFood.Domain.Entities.Categories;
using TwistFood.Domain.Exceptions;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Dtos;
using TwistFood.Service.Interfaces.Categories;
using TwistFood.Service.ViewModels.Categories;
using TwistFood.Service.ViewModels.Products;

namespace TwistFood.Service.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var res = await _unitOfWork.Categories.FirstOrDefaultAsync(x => x.CategoryName == categoryDto.CategoryName);
            if (res is not null)
                throw new StatusCodeException(HttpStatusCode.Conflict, "Category name is already exist");

            Category category = (Category)categoryDto;

            _unitOfWork.Categories.Add(category);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<PagedList<Category>> GetAllAsync(PagenationParams @params)
        {
            var query = _unitOfWork.Categories.GetAll()
            .OrderBy(x => x.Id);

            return await PagedList<Category>.ToPagedListAsync(query,
                @params);
        }

        public async Task<CategoryViewModels> GetAsync(long id)
        {
            var category = await _unitOfWork.Categories.FindByIdAsync(id);
            if (category is null) throw new StatusCodeException(HttpStatusCode.NotFound, "Category not found");

            var products = _unitOfWork.Products.GetAll().AsNoTracking().Where(x => x.CategoryId == category.Id).ToList();

            List<ProductViewModel> list = new List<ProductViewModel>();

            foreach (var product in products)
            {
                ProductViewModel productViewModel = new ProductViewModel()
                {
                    Id = product.Id,
                    ImagePath = product.ImagePath,
                    Price = product.Price,
                    ProductDescription = product.ProductDescription,
                    ProductName = product.ProductName
                };
                list.Add(productViewModel);

            }

            return new CategoryViewModels()
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Products = list
            };
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var res = await _unitOfWork.Categories.FindByIdAsync(id);
            if (res is not null)
            {
                var products = _unitOfWork.Products.Where(x => x.CategoryId == res.Id).ToList();
                foreach (var product in products)
                {
                    //_unitOfWork.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    product.CategoryId = null;
                    _unitOfWork.Products.Update(product.Id, product);
                }
                _unitOfWork.Categories.Delete(res.Id);
                var result = await _unitOfWork.SaveChangesAsync();

                return result > 0;
            }
            else throw new StatusCodeException(HttpStatusCode.NotFound, "Category not found");
        }

        public async Task<bool> UpdateAsync(long Id, CategoryViewModels categoryView)
        {
            var res = await _unitOfWork.Categories.FindByIdAsync(Id);
            if (res is not null)
            {
                _unitOfWork.Entry(res).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                Category category1 = new Category()
                {
                    Id = categoryView.Id,
                    CategoryName = categoryView.CategoryName

                };



                _unitOfWork.Categories.Update(Id, category1);
                var result = await _unitOfWork.SaveChangesAsync();
                return result > 0;
            }
            else throw new StatusCodeException(HttpStatusCode.NotFound, "category not found");
        }

        public async Task<PagedList<Category>> GetAllBySearchAsync(string search, PagenationParams @params)
        {
            var query = _unitOfWork.Categories.GetAll().Where(x=>x.CategoryName.ToLower().Contains(search.ToLower()))
          .OrderBy(x => x.Id);

            return await PagedList<Category>.ToPagedListAsync(query,
                @params);
        }
    }
}
