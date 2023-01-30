using Microsoft.AspNetCore.Mvc;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Interfaces.Products;

namespace TwistFood.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<ViewResult> Index()
        {
            var res = await _productService.GetAllAsync(new PagenationParams(1));
            return View(res);
        }
    }
}
