using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Dtos.Products;
using TwistFood.Service.Interfaces.Products;
using TwistFood.Service.ViewModels.Orders;
using TwistFood.Service.ViewModels.Products;

namespace TwistFood.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "head")]
    [Area("admin")]
    [Route("admins")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ViewResult> Index(string search,int page = 1)
        {
            PagedList<ProductViewModel> result;
            if (!String.IsNullOrEmpty(search))
            {
                ViewBag.search = search;
                result = await _productService.SearchByNameAsync(search, new PagenationParams(page, 10));
            }
            else
            {
                result = await _productService.GetAllAsync(new PagenationParams(page, 10));
            }
            return View(result);
        }

        [HttpGet("create")]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(CreateProductsDto productsDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProductAsync(productsDto);
                if (result)
                    return RedirectToAction("index", "products", new { area = "admin" });
                return Create();
            }
            return Create();
        }

        [HttpGet("update")]
        public async Task<ViewResult> Update(long productId)
        {
            var product = await _productService.GetForUpdateAsync(productId);
            if (product != null)
            {

                return View(product);
            }
            return View();
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(long Id, UpdateProductDto updateProductDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.UpdateAsync(Id, updateProductDto);
                if (result)
                {
                    return RedirectToAction("Index", "products", new { area = "admin" });
                }
                return await Update(Id);
            }
            return View();
        }


        [HttpGet("delete")]
        public async Task<ViewResult> Delete(long Id)
        {
            var product = await _productService.GetAsync(Id);
            if (product != null)
            {

                return View(product);
            }
            return View();
        }


        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync(long Id)
        {
            var res = await _productService.DeleteAsync(Id);
            if (res)
                return RedirectToAction("Index", "products", new { area = "admin" });
            return View();
        }

    }
}
