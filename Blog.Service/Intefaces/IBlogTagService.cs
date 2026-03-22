using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Entities.Vo.Tag;
using Blog.Core.Interfaces;

namespace Blog.Service.Intefaces
{
    public interface IBlogTagService : IService<BlogTag, long>
    {
        Task<EditReponse<bool>> Add(TagAddOrEdit tag);

        Task<EditReponse<BlogTag>> Query(TagTableQueryVo queryVo);
    }
}
