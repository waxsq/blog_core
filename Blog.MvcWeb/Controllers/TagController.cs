using System.Threading.Tasks;
using Blog.Service.Commons;
using Blog.Service.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MvcWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController : Controller
    {
        private readonly ILogger<TagController> _logger;
        private readonly IBlogTagService _blogTagService;
        public TagController(IBlogTagService blogTagService)
        {
            _blogTagService = blogTagService;
        }

        
    }
}
