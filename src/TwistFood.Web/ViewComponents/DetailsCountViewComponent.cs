using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using TwistFood.Domain.Enums;
using TwistFood.Service.Interfaces.Common;
using TwistFood.Service.Interfaces.Orders;

namespace TwistFood.Web.ViewComponents
{
    public class DetailsCountViewComponent : ViewComponent
    {
        private IOrderService _orderService;
        private IMemoryCache _cache;
        private IIdentityService _identityService;

        public DetailsCountViewComponent(IOrderService orderService,
                                         IMemoryCache cache, IIdentityService identityService)
        {
            this._orderService = orderService;
            this._cache = cache;
            this._identityService = identityService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if(_cache.TryGetValue(_identityService.Id!.Value, out long orderId))
            {
                var res = await _orderService.GetOrderWithOrderDetailsAsync(orderId);
                if(res.Status == "New")
                {
                    return View(res.OrderDetails.Count);
                }
                return View(0);
            }
            else
            {
                return View(0);
            }
        }
    }
}
