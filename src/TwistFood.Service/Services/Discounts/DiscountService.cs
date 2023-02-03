using System.Net;
using TwistFood.DataAccess.Interfaces;
using TwistFood.Domain.Entities.Discounts;
using TwistFood.Domain.Exceptions;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Dtos;
using TwistFood.Service.Dtos.Discounts;
using TwistFood.Service.Interfaces;
using TwistFood.Service.Interfaces.Discounts;
using TwistFood.Service.ViewModels.Discounts;

namespace TwistFood.Service.Services.Discounts
{
    public class DiscountService : IDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public DiscountService(IUnitOfWork unitOfWork,
                               IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<bool> CreateDiscountAsync(DiscountCreateDto discountCreateDto)
        {
            var res = await _unitOfWork.Discounts.FirstOrDefaultAsync(x => x.DiscountName == discountCreateDto.DiscountName);
            if (res is not null)
                throw new StatusCodeException(HttpStatusCode.Conflict, "Discount is already exist");

            Discount discount = (Discount)discountCreateDto;

            if (discountCreateDto.Image is not null)
            {
                discount.ImagePath = await _fileService.SaveImageAsync(discountCreateDto.Image);
            }

            _unitOfWork.Discounts.Add(discount);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var res = await _unitOfWork.Discounts.FindByIdAsync(id);
            if (res is not null)
            {
                _unitOfWork.Discounts.Delete(id);
                var result = await _unitOfWork.SaveChangesAsync();

                return result > 0;
            }
            else throw new StatusCodeException(HttpStatusCode.NotFound, "Discount not found");
        }

        public async Task<PagedList<DiscountViewModel>> GetAllAsync(PagenationParams @params)
        {
            var query = _unitOfWork.Discounts.GetAll()
            .OrderBy(x => x.Id).ThenByDescending(x => x.Price).Select(discount => new DiscountViewModel()
            {
                Id = discount.Id,
                DiscountName = discount.DiscountName,
                DiscountDescription = discount.Description,
                StartTime = discount.StartTime.ToShortDateString(),
                EndTime = discount.EndTime.ToShortDateString(),
                Price = discount.Price,
                ImagePath = discount.ImagePath,
            });

            return await PagedList<DiscountViewModel>.ToPagedListAsync(query, @params);
        }

        public async Task<DiscountViewModel> GetAsync(long id)
        {
            var discount = await _unitOfWork.Discounts.FindByIdAsync(id);
            if (discount is not null)
            {
                DiscountViewModel discountViewModel = new DiscountViewModel()
                {
                    Id = discount.Id,
                    DiscountName = discount.DiscountName,
                    DiscountDescription = discount.Description,
                    StartTime = discount.StartTime.ToShortDateString(),
                    EndTime = discount.EndTime.ToShortDateString(),
                    Price = discount.Price,
                    ImagePath = "http://twistfood.uz:5055/" + discount.ImagePath,
                };

                return discountViewModel;
            }
            else throw new StatusCodeException(HttpStatusCode.NotFound, "Discount not found");
        }

        public async Task<bool> UpdateAsync(long id, DiscountUpdateDto discountUpdateDto)
        {
            var res = await _unitOfWork.Discounts.FindByIdAsync(id);
            if (res is not null)
            {
                _unitOfWork.Entry(res).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

                Discount discount = (Discount)discountUpdateDto;

                discount.Id = id;
                discount.CreatedAt = res.CreatedAt;

                if (discountUpdateDto.DiscountName is not null)
                {
                    discount.DiscountName = discountUpdateDto.DiscountName;
                }
                else
                {
                    discount.DiscountName = res.DiscountName;
                }

                if (discountUpdateDto.Description is not null)
                {
                    discount.Description = discount.Description;
                }
                else
                {
                    discount.Description = res.Description;
                }

                if (discountUpdateDto.StartTime is not null)
                {
                    discount.StartTime = (DateTime)discountUpdateDto.StartTime;
                }
                else
                {
                    discount.StartTime = res.StartTime;
                }

                if (discountUpdateDto.EndTime is not null)
                {
                    discount.EndTime = (DateTime)discountUpdateDto.EndTime;
                }
                else
                {
                    discount.EndTime = res.EndTime;
                }

                if (discountUpdateDto.Price is not null)
                {
                    discount.Price = (long)discountUpdateDto.Price;
                }
                else
                {
                    discount.Price = res.Price;
                }

                if (discountUpdateDto.Image is not null)
                {
                    discount.ImagePath = await _fileService.SaveImageAsync(discountUpdateDto.Image);
                }
                else
                {
                    discount.ImagePath = res.ImagePath;
                }

                _unitOfWork.Discounts.Update(id, discount);
                var result = await _unitOfWork.SaveChangesAsync();
                return result > 0;
            }
            else throw new StatusCodeException(HttpStatusCode.NotFound, "Discount not found");
        }
    }
}
