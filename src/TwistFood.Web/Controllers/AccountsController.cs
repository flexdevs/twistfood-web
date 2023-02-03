using Microsoft.AspNetCore.Mvc;
using TwistFood.Service.Common.Exceptions;
using TwistFood.Service.Common.Helpers;
using TwistFood.Service.Dtos;
using TwistFood.Service.Dtos.Account;
using TwistFood.Service.Dtos.Accounts;
using TwistFood.Service.Interfaces.Accounts;

namespace TwistFood.Web.Controllers
{
    [Route("accounts")]
    public class AccountsController : Controller
    {
        private readonly IAccountService _service;
        private readonly IVerifyPhoneNumberService _verify;

        public AccountsController(IAccountService acccountService, IVerifyPhoneNumberService verifyPhoneNumberService)
        {
            this._service = acccountService;
            this._verify = verifyPhoneNumberService;
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
        public ViewResult Update()
        {
            return View("Update");
        }

        /*[HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(AccountUpdateDto accountUpdateDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var res = _service.AccountUpdateAsync(accountUpdateDto);
                    *//*SendToPhoneNumberDto sendToPhoneNumberDto = new SendToPhoneNumberDto()
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
                    }*//*
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
        }*/
    }
    
}
