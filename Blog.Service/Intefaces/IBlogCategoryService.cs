using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Entities.Vo.Category;
using Blog.Core.Entities.Vo.Tag;
using Blog.Core.Interfaces;

namespace Blog.Service.Intefaces
{
    public interface IBlogCategoryService : IService<BlogCategory, long>
    {
        Task<EditReponse<bool>> Add(CategoryAddOrEdit tag);
        Task<PageReponse<BlogCategory>> QueryPage(CategoryTableQueryVo queryVo);
        Task<EditReponse<CategoryAddOrEdit>> GetById(CategoryAddOrEdit tagAddOrEdit);
        Task<EditReponse<int>> DeleteById(CategoryAddOrEdit tagAddOrEdit);
        Task<EditReponse<int>> EditById(CategoryAddOrEdit tagAddOrEdit);
    }
}
