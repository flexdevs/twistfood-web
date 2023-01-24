using Microsoft.AspNetCore.Mvc;
using TwistFood.Service.Dtos.Account;
using TwistFood.Service.Interfaces.Accounts;

namespace TwistFood.Web.Controllers
{
    [Route("accounts")]
    public class AccountsController : Controller
    {
        private readonly IAccountService _service;
        public AccountsController(IAccountService acccountService)
        {
            this._service = acccountService;
        }

        [HttpGet("login")]
        public ViewResult Login()
        {
            return View("Login");
        }

        [HttpGet("register")]
        public ViewResult Register()
        {
            return View("Register");
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(AccountRegisterDto accountRegisterDto)
        {
            if (ModelState.IsValid)
            {
                bool result = await _service.AccountRegisterAsync(accountRegisterDto);
                if (result)
                {
                    return RedirectToAction("login", "accounts", new { area = "" });
                }
                else
                {
                    return Register();
                }
            }
            else return Register();
        }
    }
}
