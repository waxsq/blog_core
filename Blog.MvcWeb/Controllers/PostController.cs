using Blog.Core.Commons;
using Blog.Core.Entities.Vo.Post;
using Blog.Service.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MvcWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : Controller
    {
        private readonly ILogger<PostController> _logger;
        private readonly IBlogPostService _blogPostService;
        public PostController(ILogger<PostController> logger, IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
            _logger = logger;
        }

        [HttpPost]
        [Route("QueryPage")]
        public async Task<PageReponse<PostTablePageVo>> QueryPage(PostTableQueryVo postTableQueryVo)
        {
            return await _blogPostService.QueryPage(postTableQueryVo);
        }

    }
}
