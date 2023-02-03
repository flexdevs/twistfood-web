using System.Data.Entity;
using System.Net;
using TwistFood.DataAccess.Interfaces;
using TwistFood.Domain.Common;
using TwistFood.Domain.Entities.Order;
using TwistFood.Domain.Enums;
using TwistFood.Domain.Exceptions;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Dtos.Orders;
using TwistFood.Service.Interfaces.Common;
using TwistFood.Service.Interfaces.Orders;
using TwistFood.Service.ViewModels.Orders;

namespace TwistFood.Service.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityService _identityService;

        public OrderService(IUnitOfWork unitOfWork,
                            IIdentityService identityService)
        {
            _unitOfWork = unitOfWork;
            _identityService = identityService;

        }

        public async Task<bool> DeleteAsync(long OrderId)
        {
            var order = await _unitOfWork.Orders.FindByIdAsync(OrderId);
            if (order == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Orders not found"); }
            _unitOfWork.Orders.Delete(OrderId);

            var res = await _unitOfWork.SaveChangesAsync();

            return res > 0;
        }

        public async Task<PagedList<OrderViewModel>> GetAllAsync(PagenationParams @params)
        {

            var query = (from order in _unitOfWork.Orders.Where(x => x.Status != OrderStatus.New && x.Status != OrderStatus.Successful && x.TotalSum > 0).OrderByDescending(x => x.CreatedAt)
                         let userPhone = _unitOfWork.Users.GetAll().FirstOrDefault(x => x.Id == order.UserId).PhoneNumber
                         let orderDetails = (from orderDetail in _unitOfWork.OrderDetails.GetAll().Where(x => x.OrderId == order.Id)
                                             join product in _unitOfWork.Products.GetAll() on orderDetail.ProductId equals product.Id
                                             select product.ProductName).ToList()
                         select new OrderViewModel()
                         {
                             Id = order.Id,
                             CreatedAt = order.CreatedAt,
                             paymentType = order.PaymentType.ToString(),
                             Status = order.Status.ToString(),
                             TotalSum = order.TotalSum,
                             UserPhoneNumber = userPhone,
                             deliverId = (order.DeliverId == null) ? 0 : order.DeliverId.Value,
                             operatorId = (order.OperatorId == null) ? 0 : order.OperatorId.Value,
                             OrderDetails = orderDetails,
                             UpdatedAt = order.UpdatedAt
                         }).AsNoTracking();

            return await PagedList<OrderViewModel>.ToPagedListAsync(query, @params);
        }

        public async Task<OrderWithOrderDetailsViewModel> GetOrderWithOrderDetailsAsync(long id)
        {
            var order = await _unitOfWork.Orders.FindByIdAsync(id);
            if (order is null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Order not found"); }

            OrderWithOrderDetailsViewModel orderDetailsViewModel = new OrderWithOrderDetailsViewModel()
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                paymentType = order.PaymentType.ToString(),
                Status = order.Status.ToString(),
                TotalSum = order.TotalSum,
                UpdatedAt = order.UpdatedAt,
            };
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(x => x.Id == order.UserId);

            orderDetailsViewModel.UserPhoneNumber = user!.PhoneNumber;

            List<OrderDetailForAdminViewModel> list = new List<OrderDetailForAdminViewModel>();

            var orderDetails = _unitOfWork.OrderDetails.GetAll(id).AsNoTracking().ToList();
            foreach (var orderDetail in orderDetails)
            {

                OrderDetailForAdminViewModel detailsViewModel = new OrderDetailForAdminViewModel()
                {
                    Id = orderDetail.Id,
                    Amount = orderDetail.Amount,

                    Price = orderDetail.Price,
                };
                var product = await _unitOfWork.Products.FindByIdAsync(orderDetail.ProductId);



                detailsViewModel.ProductName = product!.ProductName;
                detailsViewModel.ProductImagePath = product.ImagePath;
                detailsViewModel.ProductId = product.Id;


                list.Add(detailsViewModel);
            }
            orderDetailsViewModel.OrderDetails = list;

            return orderDetailsViewModel;


        }

        public async Task<long> OrderCreateAsync(OrderCreateDto dto)
        {
            var user = await _unitOfWork.Users.FindByIdAsync(_identityService.Id!.Value);
            if (user == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "User not found"); }
            Location location = new Location() { Latitude = dto.Latitude, Longitude = dto.Longitude };
            _unitOfWork.Locations.Add(location);
            await _unitOfWork.SaveChangesAsync();
            var Ilocation = await _unitOfWork.Locations.FirstOrDefaultAsync(x => x.Latitude == location.Latitude && x.Longitude == location.Longitude);
            if (Ilocation == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Location not found"); }

            var order = (Order)dto;
            order.Status = 0;
            order.UserId = user.Id;
            order.ILocationId = Ilocation.Id;
            _unitOfWork.Orders.Add(order);
            await _unitOfWork.SaveChangesAsync();

            var returnOrder = await _unitOfWork.Orders.FirstOrDefaultAsync(x => x.UserId == order.UserId && x.CreatedAt == order.CreatedAt);
            if (returnOrder == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Order not found"); }
            return returnOrder.Id;
        }

        public async Task<bool> OrderUpdateAsync(OrderUpdateDto dto)
        {

            var order = await _unitOfWork.Orders.FindByIdAsync(dto.OrderId);
            if (order == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Order not found"); }

            if (dto.Status is not null)
            {
                order.Status = (OrderStatus)dto.Status;
            }

            if (dto.OperatorId is not null)
            {
                var @operator = await _unitOfWork.Operators.FirstOrDefaultAsync(x => x.Id == dto.OperatorId);
                if (@operator == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Operator not found"); }
                order.OperatorId = @operator.Id;
            }
            if (dto.DeliverId is not null)
            {
                var deliver = await _unitOfWork.Delivers.FirstOrDefaultAsync(x => x.Id == dto.DeliverId);
                if (deliver == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Deliver not found"); }
                order.DeliverId = deliver.Id;
            }
            if (dto.DeliveredAt is not null)
            {
                order.DeliveredAt = dto.DeliveredAt.Value.ToUniversalTime();
            }
            if (dto.TotalSum is not null)
            {
                order.TotalSum = dto.TotalSum.Value;
            }
            order.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Orders.Update(order.Id, order);
            await _unitOfWork.SaveChangesAsync();

            return true;

        }


    }
}
