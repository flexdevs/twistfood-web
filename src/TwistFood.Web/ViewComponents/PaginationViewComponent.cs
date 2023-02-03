using Microsoft.AspNetCore.Mvc;
using TwistFood.Service.Common.Utils;

namespace TwistFood.Web.ViewComponents
{
    public class PaginationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Tuple<string, string, string, PaginationMetaData> tuple)
        {
            return View(tuple);
        }
    }
}
