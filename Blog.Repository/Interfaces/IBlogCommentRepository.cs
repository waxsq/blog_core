using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Entities.Vo.Comment;
using Blog.Core.Interfaces;

namespace Blog.Repository.Interfaces
{
    public interface IBlogCommentRepository : IRepository<BlogComment, long>
    {
        Task<PageReponse<CommentTablePageVo>> QueryPage(CommentTableQueryVo vo);
    }

}
