using Blog.Core.Commons;
using Blog.Core.Entities.Vo.Comment;
using Blog.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MvcWeb.Controllers
{
    [ApiController]
    [Route("Comment")]
    public class CommentController : Controller
    {
        private readonly IBlogCommentRepository _blogCommentRepository;
        public CommentController(IBlogCommentRepository blogCommentRepository)
        {
            _blogCommentRepository = blogCommentRepository;
        }

        [HttpPost]
        [Route("QueryPage")]
        public async Task<PageReponse<CommentTablePageVo>> QueryPage(CommentTableQueryVo vo)
        {
            return await _blogCommentRepository.QueryPage(vo);
        }


    }
}
