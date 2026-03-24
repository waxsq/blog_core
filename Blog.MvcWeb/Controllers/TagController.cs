using System.Threading.Tasks;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Entities.Vo.Tag;
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
        public TagController(IBlogTagService blogTagService, ILogger<TagController> logger)
        {
            _blogTagService = blogTagService;
            _logger = logger;
        }

        [HttpPost]
        [Route("QueryPage")]
        public async Task<PageReponse<BlogTag>> QueryPage(TagTableQueryVo tagTableQueryVo)
        {
            return await _blogTagService.QueryPage(tagTableQueryVo);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<EditReponse<bool>> Add(TagAddOrEdit tagAddOrEdit)
        {
            return await _blogTagService.Add(tagAddOrEdit);
        }

    }
}
