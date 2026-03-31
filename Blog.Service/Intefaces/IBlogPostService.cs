using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Entities.Vo.Post;
using Blog.Core.Interfaces;

namespace Blog.Service.Intefaces
{
    public interface IBlogPostService : IService<BlogPost, long>
    {
        Task<PageReponse<PostTablePageVo>> QueryPage(PostTableQueryVo vo);
    }
}
