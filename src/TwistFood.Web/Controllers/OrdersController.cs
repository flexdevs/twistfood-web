using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Org.BouncyCastle.Asn1.X509;
using TwistFood.Domain.Entities.Order;
using TwistFood.Domain.Enums;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Dtos.Orders;
using TwistFood.Service.Interfaces.Common;
using TwistFood.Service.Interfaces.Orders;
using TwistFood.Service.ViewModels.Orders;

namespace TwistFood.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMemoryCache _cache;
        private readonly IIdentityService _identityService;
        private readonly IOrderDeteilsService _orderDeteilsService;
        private readonly int _pageSize = 30;

        public OrdersController(IOrderService orderService, IOrderDeteilsService orderDeteilsService,
                                IMemoryCache cache, IIdentityService identityService)
        {
            this._orderService = orderService;
            this._cache = cache;
            this._identityService = identityService;
            this._orderDeteilsService = orderDeteilsService;
        }
        [HttpGet]
        public async Task<IActionResult> ActiveOrder()
        {
            if (_cache.TryGetValue(_identityService.Id!.Value, out long orderId))
            {
                var order = await _orderService.GetOrderWithOrderDetailsAsync(orderId);
                if(order.Status == "New")
                {
                    return View("ActiveOrder", order);
                }
                return RedirectToAction("index", "products", new { area = "" });
            }
            else
            {
                return RedirectToAction("index", "products", new { area = "" });
            }
        }

        public async Task<IActionResult> Official()
        {
            if (_cache.TryGetValue(_identityService.Id!.Value, out long orderId))
            {
                OrderUpdateDto orderUpdateDto = new OrderUpdateDto()
                {
                    OrderId = orderId,
                    Status = (OrderStatus)1,
                };
                await _orderService.OrderUpdateAsync(orderUpdateDto);

                return RedirectToAction("index", "products", new { area = "" });
            }
            else
            {
                return RedirectToAction("index", "products", new { area = "" });
            }
            
        }

        public async Task<IActionResult> MyOrders(int page = 1)
        {
            var orders = await _orderService.GetAllAsync(new PagenationParams(page, _pageSize));
            List<OrderViewModel> userOrders = new List<OrderViewModel>();
            foreach(var order in orders)
            {
                if(order.UserPhoneNumber == _identityService.PhoneNumber)
                {
                    userOrders.Add(order);
                }
            }
            return View("MyOrders", userOrders);
        }

        public async Task<IActionResult> Delete(long Id)
        {
            await _orderDeteilsService.OrderDeleteAsync(Id);

            if (_cache.TryGetValue(_identityService.Id!.Value, out long orderId))
            {
                var order = await _orderService.GetOrderWithOrderDetailsAsync(orderId);
                if (order.Status == "New")
                {
                    return View("ActiveOrder", order);
                }
                return RedirectToAction("index", "products", new { area = "" });
            }
            else
            {
                return RedirectToAction("index", "products", new { area = "" });
            }
        }
    }
}
