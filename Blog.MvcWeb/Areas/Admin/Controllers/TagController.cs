using System.Threading.Tasks;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Entities.Vo.Tag;
using Blog.Service.Intefaces;
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

        public IActionResult AddOrEdit([FromQuery] long id, [FromQuery] string action)
        {
            ViewData["action"] = action;
            ViewData["id"] = id;
            return View();
        }
    }
}
