using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Entities.Vo.Category;
using Blog.Core.Entities.Vo.Tag;
using Blog.Core.Exceptions;
using Blog.Core.Interfaces;
using Blog.Core.Utils;
using Blog.Repository.Commons;
using Blog.Repository.Interfaces;
using Blog.Service.Intefaces;
using Dm.util;
using SqlSugar;

namespace Blog.Service.Commons
{
    public class CategoryService : Service<BlogCategory, long>, IBlogCategoryService
    {
        private readonly IBlogCategoryRepository _blogCategoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IBlogCategoryRepository repository, IMapper mapper) : base(repository)
        {
            _blogCategoryRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<EditReponse<bool>> Add(CategoryAddOrEdit tag)
        {
            if (!tag.Action.Equals("Add") && tag.BlogCategoryId != 0)
            {
                List<BlogCategory> queryResult = await _repository.QueryAsync(dto => dto.BlogCategoryId == tag.BlogCategoryId);
                if (queryResult != null && queryResult.Count != 0)
                {
                    throw new BusinessException("当前操作异常");
                }
            }
            var entity = _mapper.Map<BlogCategory>(tag);
            entity.BlogCategoryId = SnowFlakeSingle.instance.NextId();
            int result = await CreateAsync(entity);
            return ResultUtil.Success(result > 0, result > 0 ? "添加成功" : "添加失败");
        }

        public async Task<PageReponse<BlogCategory>> QueryPage(CategoryTableQueryVo queryVo)
        {
            var pageRequest = new PageRequest
            {
                PageIndex = queryVo.PageIndex,
                PageSize = queryVo.PageSize,
            };

            var whereExp = Expressionable.Create<BlogCategory>()
                .AndIF(!queryVo.CategoryName.isEmpty(), dto => dto.CategoryName.Equals(queryVo.CategoryName))
                .AndIF(queryVo.IsValid != -1, dto => dto.isValid == queryVo.IsValid)
                .ToExpression();

            var result = await QueryPagedAsync(queryVo, whereExp, null);
            return ResultUtil.SuccessPage<BlogCategory>(result);
        }

        public async Task<EditReponse<CategoryAddOrEdit>> GetById(CategoryAddOrEdit tagAddOrEdit)
        {
            if (tagAddOrEdit == null || tagAddOrEdit.BlogCategoryId == 0)
            {
                throw new BusinessException("请选择一笔数据");
            }
            var result = await base.GetByIdAsync(tagAddOrEdit.BlogCategoryId);
            var mapResult = _mapper.Map<CategoryAddOrEdit>(result);
            return ResultUtil.Success(mapResult);
        }

        public async Task<EditReponse<int>> DeleteById(CategoryAddOrEdit tagAddOrEdit)
        {
            if (tagAddOrEdit == null || tagAddOrEdit.BlogCategoryId == 0)
            {
                throw new BusinessException("请选择一笔数据");
            }
            var result = await base.DeleteByIdAsync(tagAddOrEdit.BlogCategoryId);
            return ResultUtil.Success(result);
        }

        public async Task<EditReponse<int>> EditById(CategoryAddOrEdit tagAddOrEdit)
        {
            if (tagAddOrEdit == null || tagAddOrEdit.BlogCategoryId == 0)
            {
                throw new BusinessException("请选择一笔数据");
            }
            var dto = await _blogCategoryRepository.GetByIdAsync(tagAddOrEdit.BlogCategoryId);
            if (dto == null)
            {
                throw new BusinessException("请检查数据是否存在");
            }
            var newDto = _mapper.Map<BlogCategory>(tagAddOrEdit);
            int result = await _repository.UpdateNotNullAsync(newDto);
            return ResultUtil.Success(result);
        }
    }
}
