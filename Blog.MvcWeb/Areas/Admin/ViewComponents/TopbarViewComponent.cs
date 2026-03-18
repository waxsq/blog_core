using Microsoft.AspNetCore.Mvc;

namespace Blog.MvcWeb.Areas.Admin.ViewComponents
{
    [ViewComponent(Name = "Topbar")]
    public class TopbarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View("_Topbar"));
        }
    }
}
