using Microsoft.AspNetCore.Mvc;

namespace Blog.MvcWeb.Areas.Admin.ViewComponents
{
    [ViewComponent(Name = "PageTab")]
    public class PageTabViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View("_PageTab"));
        }
    }
}
