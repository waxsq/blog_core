using Microsoft.AspNetCore.Mvc;

namespace Blog.MvcWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddOrEdit([FromQuery] long id, [FromQuery] string action)
        {
            ViewData["action"] = action;
            ViewData["id"] = id;
            return View();
        }
    }
}
