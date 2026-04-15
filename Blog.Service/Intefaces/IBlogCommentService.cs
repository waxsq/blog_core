using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Entities.Vo.Comment;
using Blog.Core.Interfaces;

namespace Blog.Service.Intefaces
{
    public interface IBlogCommentService : IService<BlogComment,long>
    {
        public Task<EditReponse<int>> UpdateStatus(CommentUpdateStatusVo updateStatusVo);
        public Task<PageReponse<CommentTablePageVo>> QueryPage(CommentTableQueryVo vo);
    }
}
