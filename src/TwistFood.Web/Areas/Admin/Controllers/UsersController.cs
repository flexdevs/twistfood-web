using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Interfaces.Accounts;

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

        public async Task<IActionResult> Index(int page = 1)
        {

            var users = await _accountService.GetAllAsync(new PagenationParams(page, 10));
            return View("index", users);
        }
    }
}
