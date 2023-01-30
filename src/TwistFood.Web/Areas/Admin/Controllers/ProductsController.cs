using Microsoft.AspNetCore.Mvc;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Dtos.Products;
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

        [HttpGet("create")]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(CreateProductsDto productsDto)
        {
            if(ModelState.IsValid) 
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
        public async Task<IActionResult> UpdateAsync(long Id,UpdateProductDto updateProductDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.UpdateAsync(Id, updateProductDto);
                if (result)
                {
                    return RedirectToAction("Index","products",new {area= "admin"});
                }
                return await Update(Id);
            }
            return View();
        }
    }
}
