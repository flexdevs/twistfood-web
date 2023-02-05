using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TwistFood.Service.Interfaces.Accounts;
using TwistFood.Service.Interfaces.Admins;
using TwistFood.Service.Interfaces.Common;

namespace TwistFood.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "head")]

    [Area("admin")]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IIdentityService _identityServie;

        public AdminController(IAdminService adminService, IIdentityService identityService)
        {
            _adminService = adminService;   
            _identityServie = identityService;
        }
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var admin = await _adminService.GetAsync((long)_identityServie.Id!);
            return View(admin);
        }
    }
}
