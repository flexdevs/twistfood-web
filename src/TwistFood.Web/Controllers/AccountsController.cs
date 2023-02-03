using Microsoft.AspNetCore.Mvc;
using TwistFood.Service.Common.Exceptions;
using TwistFood.Service.Common.Helpers;
using TwistFood.Service.Dtos;
using TwistFood.Service.Dtos.Account;
using TwistFood.Service.Dtos.Accounts;
using TwistFood.Service.Interfaces.Accounts;
using TwistFood.Service.Interfaces.Common;

namespace TwistFood.Web.Controllers
{
    [Route("accounts")]
    public class AccountsController : Controller
    {
        private readonly IAccountService _service;
        private readonly IVerifyPhoneNumberService _verify;
        private readonly IIdentityService _identityService;

        public AccountsController(IAccountService acccountService, 
                                  IVerifyPhoneNumberService verifyPhoneNumberService,
                                  IIdentityService identityService)
        {
            this._service = acccountService;
            this._verify = verifyPhoneNumberService;
            this._identityService = identityService;
        }

        [HttpGet("login")]
        public ViewResult Login()
        {
            return View("Login");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(AccountLoginDto accountLoginDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SendToPhoneNumberDto sendToPhoneNumberDto = new SendToPhoneNumberDto()
                    {
                        PhoneNumber = accountLoginDto.PhoneNumber,
                    };
                    bool res = await _verify.SendCodeAsync(sendToPhoneNumberDto);
                    if (res)
                    {
                        TempData["tel"] = accountLoginDto.PhoneNumber;

                        return RedirectToAction("VerifyPhoneNumber", "verify", new { area = "" });
                    }
                    else
                    {
                        return Login();
                    }
                }
                catch (ModelErrorException modelError)
                {
                    ModelState.AddModelError(modelError.Property, modelError.Message);
                    return Login();
                }
                catch
                {
                    return Login();
                }
            }
            else return Login();
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
                try
                {
                    bool result = await _service.AccountRegisterAsync(accountRegisterDto);
                    if (result)
                    {
                        SendToPhoneNumberDto sendToPhoneNumberDto = new SendToPhoneNumberDto()
                        {
                            PhoneNumber = accountRegisterDto.PhoneNumber,
                        };
                        bool res = await _verify.SendCodeAsync(sendToPhoneNumberDto);
                        if (res)
                        {
                            TempData["tel"] = accountRegisterDto.PhoneNumber;
                            return RedirectToAction("VerifyPhoneNumber", "verify", new { area = "" });
                        }
                        else
                        {
                            return Register();
                        }

                    }
                    else
                    {
                        return Register();
                    }
                }
                catch (ModelErrorException modelError)
                {
                    ModelState.AddModelError(modelError.Property, modelError.Message);
                    return Register();
                }
            }
            else return Register();
        }

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Append("X-Access-Token", "", new CookieOptions()
            {
                Expires = TimeHelper.GetCurrentServerTime().AddDays(-1)
            });
            return RedirectToAction("Login", "Accounts", new { area = "" });
        }

        [HttpGet("update")]
        public async Task<ViewResult> Update()
        {
            var user = await _service.GetAsync(_identityService.Id!.Value);
            var AccountUpdateDto = new AccountUpdateDto()
            {
                FullName = user.FullName
            };
            return View("Update", AccountUpdateDto);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(AccountUpdateDto accountUpdateDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var res = await _service.AccountUpdateAsync(accountUpdateDto);
                    if (res == "false")
                    {
                        return await Update();
                    }
                    else
                    {
                        HttpContext.Response.Cookies.Append("X-Access-Token", res, new CookieOptions()
                        {
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict
                        });
                        return RedirectToAction("index", "home", new { area = "" });
                    }
                }
                catch (ModelErrorException modelError)
                {
                    ModelState.AddModelError(modelError.Property, modelError.Message);
                    return Login();
                }
                catch
                {
                    return Login();
                }
            }
            else return Login();
        }
    }

}
