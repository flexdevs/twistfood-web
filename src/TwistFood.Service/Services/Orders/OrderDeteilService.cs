using Microsoft.EntityFrameworkCore;
using System.Net;
using TwistFood.DataAccess.Interfaces;
using TwistFood.Domain.Entities.Order;
using TwistFood.Domain.Exceptions;
using TwistFood.Service.Dtos.Orders;
using TwistFood.Service.Interfaces.Orders;
using TwistFood.Service.ViewModels.Orders;

namespace TwistFood.Service.Services.Orders
{
    public class OrderDeteilService : IOrderDeteilsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;

        public OrderDeteilService(IUnitOfWork unitOfWork, IOrderService orderService)
        {
            this._unitOfWork = unitOfWork;
            this._orderService = orderService;
        }

        public async Task<OrderDetailForAdminViewModel> GetAsync(long Id)
        {
            var orderDetail = await _unitOfWork.OrderDetails.FindByIdAsync(Id);
            if (orderDetail == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Order detail not found"); }
            OrderDetailForAdminViewModel orderDetailForAdminViewModel = new OrderDetailForAdminViewModel()
            {
                Id = orderDetail.Id,
                Amount = orderDetail.Amount,
                Price = orderDetail.Price,
                ProductId = orderDetail.ProductId

            };
            var product = await _unitOfWork.Products.FindByIdAsync(orderDetail.ProductId);
            if (product == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Product not found"); }
            orderDetailForAdminViewModel.ProductImagePath = product.ImagePath;
            orderDetailForAdminViewModel.ProductName = product.ProductName;
            return orderDetailForAdminViewModel;
        }

        public async Task<bool> OrderCreateAsync(long OrderId, OrderDeteilsCreateDto orderDeteilsDto)
        {
            var order = await _unitOfWork.Orders.FindByIdAsync(OrderId);
            if (order is null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Order not found"); }

            _unitOfWork.Entry(order).State = EntityState.Detached;
            OrderDetail orderdetail = new OrderDetail() { OrderId = order.Id };

            var product = await _unitOfWork.Products.FindByIdAsync(orderDeteilsDto.ProductId);
            if (product == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Product not found"); }

            orderdetail.ProductId = orderDeteilsDto.ProductId;
            orderdetail.Amount = (orderDeteilsDto.Amount == 0) ? 1 : orderDeteilsDto.Amount;

            orderdetail.Price = (await _unitOfWork.Products.FindByIdAsync(orderdetail.ProductId))!.Price * orderdetail.Amount;
            order.TotalSum += orderdetail.Price;

            _unitOfWork.Orders.Update(order.Id, order);
            _unitOfWork.OrderDetails.Add(orderdetail);
            await _unitOfWork.SaveChangesAsync();
            return true;

        }

        public async Task<bool> OrderDeleteAsync(long id)
        {
            var orderDetail = await _unitOfWork.OrderDetails.FindByIdAsync(id);
            if (orderDetail == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Order detail not found"); }
            var order = await _unitOfWork.Orders.FindByIdAsync(orderDetail.OrderId);
            if (order == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Order not found"); }
            _unitOfWork.Entry(order).State = EntityState.Detached;

            order.TotalSum -= orderDetail.Price;
            _unitOfWork.Orders.Update(order.Id, order);
            _unitOfWork.OrderDetails.Delete(id);

            var result = await _unitOfWork.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> OrderUpdateAsync(OrderDetailUpdateDto dto)
        {
            var orderDetail = await _unitOfWork.OrderDetails.FindByIdAsync(dto.OrderDetailId);
            if (orderDetail == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Order detail not found"); }


            _unitOfWork.Entry(orderDetail).State = EntityState.Detached;

            var order = await _unitOfWork.Orders.FindByIdAsync(orderDetail.OrderId);
            if (order == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Order not found"); }
            //if (HttpContextHelper.IsUser)
            //{
            //    if (order.UserId != HttpContextHelper.UserId) { throw new StatusCodeException(HttpStatusCode.Unauthorized, "Unauthorized"); }
            //}
            _unitOfWork.Entry(order).State = EntityState.Detached;


            if (dto.ProductId is not null && dto.ProductId != 0)
            {
                var product = await _unitOfWork.Products.FindByIdAsync((long)dto.ProductId);
                if (product == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Product not found"); }
                orderDetail.ProductId = product.Id;
            }

            if (dto.Amount is not null)
            {
                orderDetail.Amount = (int)dto.Amount;
            }
            order.TotalSum -= orderDetail.Price;
            if (dto.Price is not null)
            {
                orderDetail.Price = (double)dto.Price;
            }

            orderDetail.Price = (await _unitOfWork.Products.FindByIdAsync(orderDetail.ProductId))!.Price * orderDetail.Amount;

            await _orderService.OrderUpdateAsync(new OrderUpdateDto()
            {
                OrderId = order.Id,
                TotalSum = order.TotalSum + orderDetail.Price
            });

            _unitOfWork.OrderDetails.Update(orderDetail.Id, orderDetail);



            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOrderWithOrderDetailsAsync(long OrderId, OrderWithOrderDetailsViewModel orderWithOrderDetails)
        {
            var order = await _unitOfWork.Orders.FindByIdAsync(OrderId);
            if (order == null) { throw new StatusCodeException(HttpStatusCode.NotFound, "Order not found"); }

            foreach (var item in orderWithOrderDetails.OrderDetails)
            {
                OrderDetailUpdateDto orderDetailUpdateDto = new OrderDetailUpdateDto()
                {
                    OrderDetailId = item.Id,
                    Amount = item.Amount,
                    ProductId = item.ProductId,

                };
                var product = await _unitOfWork.Products.FindByIdAsync(item.ProductId);
                orderDetailUpdateDto.Price = product!.Price * item.Amount;

                await OrderUpdateAsync(orderDetailUpdateDto);
            }
            return true;
        }
    }
}
