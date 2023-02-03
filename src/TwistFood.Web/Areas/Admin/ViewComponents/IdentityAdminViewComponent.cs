using Microsoft.AspNetCore.Mvc;
using TwistFood.Service.Interfaces.Common;
using TwistFood.Service.ViewModels.Accounts;

namespace TwistFood.Web.Areas.Admin.ViewComponents
{
    public class IdentityAdminViewComponent : ViewComponent
    {
        private readonly IIdentityService _identityService;

        public IdentityAdminViewComponent(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public IViewComponentResult Invoke()
        {
            AdminAccountBaseViewModel adminAccountBaseViewModel = new AdminAccountBaseViewModel()
            {
                Id = _identityService.Id!.Value,
                FullName = _identityService.FullName,
                PhoneNumber = _identityService.PhoneNumber,
                Role = _identityService.Role,

            };
            return View(adminAccountBaseViewModel);
        }
    }
}
