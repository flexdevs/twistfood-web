using Microsoft.AspNetCore.Mvc;

namespace TwistFood.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
