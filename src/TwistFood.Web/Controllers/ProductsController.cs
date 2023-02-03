using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Protocol.Core.Types;
using TwistFood.Domain.Entities.Products;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Dtos.Orders;
using TwistFood.Service.Interfaces.Common;
using TwistFood.Service.Interfaces.Discounts;
using TwistFood.Service.Interfaces.Orders;
using TwistFood.Service.Interfaces.Products;
using TwistFood.Service.Services.Orders;
using TwistFood.Service.ViewModels.Discounts;
using TwistFood.Service.ViewModels.Products;

namespace TwistFood.Web.Controllers;

[Route("products")]
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly IMemoryCache _cache;
    private readonly IIdentityService _identityService;
    private readonly IOrderDeteilsService _orderDeteilService;
    private readonly IDiscountService _discountService;
    private readonly int _pageSize = 5;
    public ProductsController(IProductService productService, IOrderService orderService,
                              IMemoryCache cache, IIdentityService identityService,
                              IOrderDeteilsService orderDeteilService, IDiscountService discountService)
    {
        this._productService = productService;
        this._orderService = orderService;
        this._cache = cache;
        this._identityService = identityService;
        this._orderDeteilService = orderDeteilService;
        this._discountService = discountService;
    }
    public async Task<ViewResult> Index(int page = 1)
    {
        var products = await _productService.GetAllAsync(new PagenationParams(page, _pageSize));
        return View("Index", products);
    }


    [HttpGet("DiscountId")]
    public async Task<IActionResult> Dsicount(long DiscountId) 
    {
        var DiscountInfo = await _discountService.GetAsync(DiscountId);

        if(_cache.TryGetValue(_identityService.Id!.Value, out long orderId))
        {
            OrderDeteilsCreateDto orderDeteilsCreateDto = new()
            {
                ProductId = DiscountId,
                Price = DiscountInfo.Price,
                Amount = 1
            };

            await _orderDeteilService.OrderCreateAsync(orderId, orderDeteilsCreateDto);

            
            return RedirectToAction("index", "products", new { area = "" });
        }
        else
        {
            OrderCreateDto orderCreateDto = new OrderCreateDto()
            {
                Latitude = 41.282906,
                Longitude = 69.199571,
                DeliverPrice = 9000,
                IsDiscount = true,
                DiscountId= DiscountId
            };
            var OrderId = await _orderService.OrderCreateAsync(orderCreateDto);

            OrderDeteilsCreateDto orderDeteilsCreateDto = new()
            {
                ProductId = DiscountId,
                Price = DiscountInfo.Price,
                Amount = 1
            };

            await _orderDeteilService.OrderCreateAsync(OrderId, orderDeteilsCreateDto);

            _cache.Set(_identityService.Id!.Value, OrderId);

            
            return RedirectToAction("index", "products", new { area = "" });
        }
    }

    [HttpGet("ProductId")]
    public async Task<IActionResult> Product(long ProductId)
    {
        var ProductInfo = await _productService.GetAsync(ProductId);

        if (_cache.TryGetValue(_identityService.Id!.Value, out long orderId))
        {
            OrderDeteilsCreateDto orderDeteilsCreateDto = new()
            {
                ProductId = ProductId,
                Price = ProductInfo.Price,
                Amount = 1
            };

            await _orderDeteilService.OrderCreateAsync(orderId, orderDeteilsCreateDto);


            return RedirectToAction("index", "products", new { area = "" });
        }
        else
        {
            OrderCreateDto orderCreateDto = new OrderCreateDto()
            {
                Latitude = 41.282906,
                Longitude = 69.199571,
                DeliverPrice = 9000,
                IsDiscount = false
            };
            var OrderId = await _orderService.OrderCreateAsync(orderCreateDto);

            OrderDeteilsCreateDto orderDeteilsCreateDto = new()
            {
                ProductId = ProductId,
                Price = ProductInfo.Price,
                Amount = 1
            };

            await _orderDeteilService.OrderCreateAsync(OrderId, orderDeteilsCreateDto);

            _cache.Set(_identityService.Id!.Value, OrderId);


            return RedirectToAction("index", "products", new { area = "" });
        }
    }
}
