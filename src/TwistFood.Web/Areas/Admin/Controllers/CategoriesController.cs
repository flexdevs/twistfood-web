using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Dtos;
using TwistFood.Service.Interfaces.Categories;
using TwistFood.Service.ViewModels.Categories;

namespace TwistFood.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "head")]

    [Area("admin")]
    [Route("admins/categories")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var res = await _categoryService.GetAllAsync(new PagenationParams(page, 2));

            return View(res);
        }

        [HttpGet("create")]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(CategoryDto categoryDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.CreateCategoryAsync(categoryDto);
                if (result)
                    return RedirectToAction("index", "categories", new { area = "admin" });
                return Create();
            }
            return Create();
        }

        [HttpGet("update")]
        public async Task<ViewResult> Update(long Id)
        {
            var categroy = await _categoryService.GetAsync(Id);
            if (categroy != null)
            {

                return View(categroy);
            }
            return View();
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(long Id, CategoryViewModels categoryView)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.UpdateAsync(Id, categoryView);
                if (result)
                {
                    return RedirectToAction("Index", "categories", new { area = "admin" });
                }
                return await Update(Id);
            }
            return View();
        }


        [HttpGet("delete")]
        public async Task<ViewResult> Delete(long Id)
        {
            var categroy = await _categoryService.GetAsync(Id);
            if (categroy != null)
            {

                return View(categroy);
            }
            return View();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync(long Id)
        {
            var res = await _categoryService.DeleteAsync(Id);
            if (res)
                return RedirectToAction("Index", "categories", new { area = "admin" });
            return View();
        }
    }
}
