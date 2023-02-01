using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using TwistFood.Domain.Entities.Order;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Interfaces.Common;
using TwistFood.Service.Interfaces.Orders;

namespace TwistFood.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMemoryCache _cache;
        private readonly IIdentityService _identityService;
        private readonly int _pageSize = 30;

        public OrdersController(IOrderService orderService,
                                IMemoryCache cache, IIdentityService identityService)
        {
            this._orderService = orderService;
            this._cache = cache;
            this._identityService = identityService;
        }
        [HttpGet]
        public async Task<IActionResult> ActiveOrder()
        {
            var order = await _orderService.GetOrderWithOrderDetailsAsync(19);
            return View("ActiveOrder", order);
            /*if (_cache.TryGetValue(_identityService.Id!.Value, out long orderId))
            {
                var order = await _orderService.GetOrderWithOrderDetailsAsync(orderId);
                return View("ActiveOrder", order);
            }
            else
            {
                return RedirectToAction("index", "products", new { area = "" });
            }*/
        }
    }
}
