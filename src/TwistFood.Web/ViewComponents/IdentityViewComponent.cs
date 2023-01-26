using Microsoft.AspNetCore.Mvc;
using TwistFood.Service.Interfaces.Common;
using TwistFood.Service.ViewModels.Accounts;

namespace TwistFood.Web.ViewComponents;

public class IdentityViewComponent : ViewComponent
{
    private readonly IIdentityService _identityService;
    public IdentityViewComponent(IIdentityService identity)
    {
        this._identityService = identity;
    }
    public IViewComponentResult Invoke()
    {
        AccountBaseViewModel accountBaseViewModel = new AccountBaseViewModel()
        {
            Id = _identityService.Id!.Value,
            FullName = _identityService.FullName,
            PhoneNumber = _identityService.PhoneNumber,
        };
        return View(accountBaseViewModel);
    }
}
