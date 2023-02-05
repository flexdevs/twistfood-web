using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwistFood.Domain.Entities.Users;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Interfaces.Accounts;
using TwistFood.Service.ViewModels.Products;

namespace TwistFood.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "head")]

    [Area("admin")]
    [Route("admins/users")]
    public class UsersController : Controller
    {
        private readonly IAccountService _accountService;
        public UsersController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index(string search,int page = 1)
        {

            PagedList<User> result;
            if (!String.IsNullOrEmpty(search))
            {
                ViewBag.search = search;
                result = await _accountService.GetAllForSearchAsync(search, new PagenationParams(page, 10));
            }
            else
            {
                result = await _accountService.GetAllAsync(new PagenationParams(page, 10));
            }
            return View(result);
        }
    }
}
