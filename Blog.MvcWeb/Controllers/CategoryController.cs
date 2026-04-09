using System.Threading.Tasks;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Entities.Vo.Category;
using Blog.Core.Entities.Vo.Tag;
using Blog.Service.Commons;
using Blog.Service.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MvcWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IBlogCategoryService _blogCategoryService;
        public CategoryController(IBlogCategoryService blogCategoryService, ILogger<CategoryController> logger)
        {
            _blogCategoryService = blogCategoryService;
            _logger = logger;
        }

        [HttpPost]
        [Route("QueryPage")]
        public async Task<PageReponse<BlogCategory>> QueryPage(CategoryTableQueryVo categoryTableQueryVo)
        {
            return await _blogCategoryService.QueryPage(categoryTableQueryVo);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<EditReponse<bool>> Add(CategoryAddOrEdit categoryAddOrEdit)
        {
            return await _blogCategoryService.Add(categoryAddOrEdit);
        }

        [HttpPost]
        [Route("GetById")]
        public async Task<EditReponse<CategoryAddOrEdit>> GetById(CategoryAddOrEdit categoryAddOrEdit)
        {
            return await _blogCategoryService.GetById(categoryAddOrEdit);
        }

        [HttpPost]
        [Route("DeleteById")]
        public async Task<EditReponse<int>> DeleteById(CategoryAddOrEdit categoryAddOrEdit)
        {
            return await _blogCategoryService.DeleteById(categoryAddOrEdit);
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<EditReponse<int>> Edit(CategoryAddOrEdit categoryAddOrEdit)
        {
            return await _blogCategoryService.EditById(categoryAddOrEdit);
        }

    }
}
