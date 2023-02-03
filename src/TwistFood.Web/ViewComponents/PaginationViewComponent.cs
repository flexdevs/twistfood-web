using Microsoft.AspNetCore.Mvc;
using TwistFood.Service.Common.Utils;

namespace TwistFood.Web.ViewComponents
{
    public class PaginationViewComponent<T>:ViewComponent
    {
        public IViewComponentResult Invoke(PagedList<T> values)
        {
            return View(values.MetaData);
        }
    }
}
