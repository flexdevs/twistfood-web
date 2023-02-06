using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwistFood.Domain.Entities.Users;
using TwistFood.Service.Common.Utils;
using TwistFood.Service.Dtos.Accounts;
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

        [HttpGet("update")]

        public async Task<ViewResult> Update(long Id)
        {
            var user = await _accountService.GetAsync(Id);
            var dto = new AccountUpdateDto()
            {
                Id = Id,
                FullName = user.FullName
            };
            return View(dto);   
        }
        [HttpPost("update")]

        public async Task<IActionResult> UpdateAsync(AccountUpdateDto dto)
        {
            if(dto is not null)
            {  
                var res = await _accountService.UserUpdateAsync(dto);
            }
          
            return RedirectToAction("Index", "users",new {area = "admin"});
        }

        [HttpGet("delete")] 
        public async Task<ViewResult> Delete(long Id)
        {
            var user = await _accountService.GetAsync(Id);
            if (user != null)
            {

                return View(user);
            }
            return View();
        }


        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync(long Id)
        {
            var res = await _accountService.DeleteAsync(Id);
            if (res)
                return RedirectToAction("Index", "users", new { area = "admin" });
            return View();
        }
    }
}
