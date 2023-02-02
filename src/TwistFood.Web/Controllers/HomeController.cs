using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Interfaces.Discounts;
using TwistFood.Web.Models;

namespace TwistFood.Web.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IDiscountService _discountService;
        private readonly int _pageSize = 30;

        public HomeController(ILogger<HomeController> logger, IDiscountService discountService)
		{
			_logger = logger;
			this._discountService = discountService;

        }

        public async Task<ViewResult> Index(int page = 1)
        {
            var discounts = await _discountService.GetAllAsync(new PagenationParams(page, _pageSize));
            return View("Index", discounts);
        }

		public IActionResult Privacy()
		{
			return View();
		}

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}