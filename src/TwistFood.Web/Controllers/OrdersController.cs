using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Interfaces.Orders;

namespace TwistFood.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly int _pageSize = 30;

        public OrdersController(IOrderService orderService)
        {
            this._orderService = orderService;
        }
        [HttpGet]
        public async Task<ViewResult> ActiveOrder()
        {
            var orders = await _orderService.GetAllAsync(new PagenationParams(1, _pageSize));
            return View("ActiveOrder", orders);
        }
    }
}
