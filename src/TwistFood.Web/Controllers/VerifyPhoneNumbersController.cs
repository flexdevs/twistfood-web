using Microsoft.AspNetCore.Mvc;
using TwistFood.Service.Common.Exceptions;
using TwistFood.Service.Dtos.Accounts;
using TwistFood.Service.Interfaces.Accounts;

namespace TwistFood.Web.Controllers
{
    [Route("verify")]
    public class VerifyPhoneNumbersController : Controller
    {
        private readonly IAccountService _service;
        private readonly IVerifyPhoneNumberService _verify;

        public VerifyPhoneNumbersController(IAccountService acccountService,
            IVerifyPhoneNumberService verifyPhoneNumberService)
        {
            this._service = acccountService;
            this._verify = verifyPhoneNumberService;
        }
        [HttpGet("VerifyPhoneNumber")]
        public ViewResult VerifyPhoneNumber()
        {
            return View("VerifyPhoneNumber");
        }

        [HttpPost("VerifyPhoneNumber")]
        public async Task<IActionResult> VerifyAsync(VerifyPhoneNumberDto verifyPhoneNumberDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = await _verify.VerifyPhoneNumber(verifyPhoneNumberDto);
                    HttpContext.Response.Cookies.Append("X-Access-Token", token, new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    });
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                catch (ModelErrorException modelError)
                {
                    ModelState.AddModelError(modelError.Property, modelError.Message);
                    return VerifyPhoneNumber();
                }
                catch
                {
                    return VerifyPhoneNumber();
                }
            }
            else return VerifyPhoneNumber();
        }
    }
}
