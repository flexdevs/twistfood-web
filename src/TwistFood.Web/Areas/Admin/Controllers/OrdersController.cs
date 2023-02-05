using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwistFood.Domain.Entities.Categories;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Dtos.Orders;
using TwistFood.Service.Interfaces.Orders;
using TwistFood.Service.Interfaces.Products;
using TwistFood.Service.ViewModels.Orders;
using TwistFood.Service.ViewModels.Products;

namespace TwistFood.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "head")]

    [Area("admin")]
    [Route("admins/orders")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDeteilsService _orderDetailService;
        private readonly IProductService _productService;

        public OrdersController(IOrderService orderService, IOrderDeteilsService orderDeteilsService, IProductService productService)
        {
            _orderService = orderService;
            _orderDetailService = orderDeteilsService;
            _productService = productService;
        }


        public async Task<ViewResult> Index(string search,int page = 1)
        {
            PagedList<OrderViewModel> result;
            if (!String.IsNullOrEmpty(search))
            {
                ViewBag.search = search;
                result = await _orderService.GetAllForSearchAsync(search, new PagenationParams(page, 10));
            }
            else
            {
                result = await _orderService.GetAllAsync(new PagenationParams(page, 10));
            }
            return View(result);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create(long OrderId, int page = 1)
        {
            var products = await _productService.GetAllAsync(new PagenationParams(page));
            var tuple = new Tuple<long, List<ProductViewModel>>(OrderId, products.ToList());
            return View(tuple);

        }

        [HttpGet("create-order")]

        public async Task<IActionResult> CreateOrderAsync(long OrderId, long productId)
        {
            OrderDeteilsCreateDto orderDeteils = new OrderDeteilsCreateDto()
            {
                ProductId = productId,
                Amount = 1,
                Price = 0,
            };
            var res = await _orderDetailService.OrderCreateAsync(OrderId, orderDeteils);
            var routeData = new RouteValueDictionary { { "Id", OrderId } };
            if (res)
                return RedirectToAction("UpdateTable", "orders", routeData);
            return View("index");
        }


        [HttpGet("updateTable")]
        public async Task<ViewResult> UpdateTable(long Id)
        {
            var Order = await _orderService.GetOrderWithOrderDetailsAsync(Id);
            if (Order != null)
            {

                return View(Order);
            }
            return View();
        }

        [HttpGet("update")]
        public async Task<ViewResult> Update(long Id)
        {
            var OrderDetails = await _orderDetailService.GetAsync(Id);
            if (OrderDetails != null)
            {

                return View(OrderDetails);
            }
            return View();
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(long Id, OrderDetailForAdminViewModel order)
        {
            if (ModelState.IsValid)
            {
                OrderDetailUpdateDto orderDetail = new OrderDetailUpdateDto()
                {
                    OrderDetailId = Id,
                    Amount = order.Amount,
                    Price = order.Price,
                    ProductId = order.ProductId
                };

                var result = await _orderDetailService.OrderUpdateAsync(orderDetail);
                if (result)
                {
                    return RedirectToAction("Index", "orders", new { area = "admin" });
                }
                return await UpdateTable(Id);
            }
            return View();
        }

        [HttpGet("delete")]
        public async Task<ViewResult> Delete(long Id)
        {
            var order = await _orderService.GetOrderWithOrderDetailsAsync(Id);
            if (order != null)
            {

                return View(order);
            }
            return View();
        }


        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync(long Id)
        {
            var res = await _orderService.DeleteAsync(Id);
            if (res)
                return RedirectToAction("Index", "orders", new { area = "admin" });
            return View();
        }

    }
}
