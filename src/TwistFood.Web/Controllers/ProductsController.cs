using Microsoft.AspNetCore.Mvc;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Interfaces.Products;

namespace TwistFood.Web.Controllers;

[Route("products")]
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly int _pageSize = 30;
    public ProductsController(IProductService productService)
    {
        this._productService = productService;
    }
    public async Task<ViewResult> Index(int page = 1)
    {
        var products = await _productService.GetAllAsync(new PagenationParams(page, _pageSize));
        return View("Index", products);
    }
}
