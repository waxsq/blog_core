using Microsoft.AspNetCore.Mvc;

namespace Blog.MvcWeb.Areas.Admin.ViewComponents
{
    [ViewComponent(Name = "Sidebar")]
    public class SidebarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View("_Sidebar"));
        }
    }
}
