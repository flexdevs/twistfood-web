using Microsoft.AspNetCore.Mvc;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Interfaces.Orders;

namespace TwistFood.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService) 
        {
            _orderService = orderService;
        }
        
        public async Task<ViewResult> Index()
        {
            var result = await _orderService.GetAllAsync(new PagenationParams(5));
            return View(result);
        }
    }
}
