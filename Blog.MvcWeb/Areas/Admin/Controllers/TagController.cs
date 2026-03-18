using Microsoft.AspNetCore.Mvc;

namespace Blog.MvcWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
