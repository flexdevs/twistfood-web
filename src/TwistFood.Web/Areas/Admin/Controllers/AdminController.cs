using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TwistFood.Service.Dtos.Accounts;
using TwistFood.Service.Dtos.Products;
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
        [HttpGet("update")]
        public async Task<ViewResult> Update()
        {
            var admin = await _adminService.GetAsync(_identityServie.Id!.Value);
            if (admin != null)
            {
                AdminUpdateDto adminUpdateDto = new AdminUpdateDto()
                {
                    FirstName= admin.FirstName, 
                    LastName= admin.LastName,   
                    Email=admin.Email,
                };

                return View(adminUpdateDto);
            }
            return View();
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(AdminUpdateDto updateDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminService.AdminUpdateAsync(_identityServie.Id!.Value, updateDto);
                if (result)
                {
                    return RedirectToAction("profile", "admin", new { area = "admin" });
                }
                return await Update();
            }
            return View();
        }
    }
}
