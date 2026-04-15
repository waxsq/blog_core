using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Entities.Vo.Comment;
using Blog.Core.Exceptions;
using Blog.Core.Interfaces;
using Blog.Core.Utils;
using Blog.Repository.Interfaces;
using Blog.Service.Intefaces;

namespace Blog.Service.Commons
{
    public class BlogCommentService : Service<BlogComment, long>, IBlogCommentService
    {
        private readonly IBlogCommentRepository _blogCommentRepository;
        public BlogCommentService(IBlogCommentRepository blogCommentRepository) : base(blogCommentRepository)
        {
            _blogCommentRepository = blogCommentRepository;
        }

        public async Task<EditReponse<int>> UpdateStatus(CommentUpdateStatusVo updateStatusVo)
        {
            List<long> ids = updateStatusVo.Ids;
            int status = updateStatusVo.Status;
            if (!ids.Any())
            {
                throw new BusinessException("请选择数据");
            }
            var result = await _blogCommentRepository.UpdateAsync(c => ids.Contains(c.BlogCommentId), c => new BlogComment
            {
                Status = status,
            });

            return ResultUtil.Success(result);
        }



        public Task<PageReponse<CommentTablePageVo>> QueryPage(CommentTableQueryVo vo)
        {
            return _blogCommentRepository.QueryPage(vo);
        }
    }
}
