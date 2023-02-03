using TwistFood.Service.Common.Utils;
using TwistFood.Service.Dtos;
using TwistFood.Service.Dtos.Discounts;
using TwistFood.Service.ViewModels.Discounts;

namespace TwistFood.Service.Interfaces.Discounts
{
    public interface IDiscountService
    {
        public Task<bool> CreateDiscountAsync(DiscountCreateDto discountCreateDto);
        public Task<PagedList<DiscountViewModel>> GetAllAsync(PagenationParams @params);

        public Task<DiscountViewModel> GetAsync(long id);

        public Task<bool> DeleteAsync(long id);

        public Task<bool> UpdateAsync(long id, DiscountUpdateDto discountUpdateDto);
    }
}
