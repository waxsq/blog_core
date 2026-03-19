using System.Threading.Tasks;
using Blog.Core.Entities.Vo.Tag;
using Blog.Service.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MvcWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly IBlogTagService _blogTagService;
        public TagController(IBlogTagService blogTagService)
        {
            this._blogTagService = blogTagService;
        }

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

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TagAddOrEdit tag){
            return Json( await _blogTagService.Add(tag));
        }

    }
}
