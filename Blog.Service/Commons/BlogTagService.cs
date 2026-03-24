using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Entities.Vo.Tag;
using Blog.Core.Exceptions;
using Blog.Core.Interfaces;
using Blog.Core.Utils;
using Blog.Repository.Interfaces;
using Blog.Service.Intefaces;
using Dm.util;
using SqlSugar;

namespace Blog.Service.Commons
{
    public class BlogTagService : Service<BlogTag, long>, IBlogTagService
    {
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly IMapper _mapper;

        public BlogTagService(IBlogTagRepository repository, IMapper mapper) : base(repository)
        {
            _blogTagRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<EditReponse<bool>> Add(TagAddOrEdit tag)
        {
            if (!tag.Action.Equals("Add") && tag.BlogTagId != 0)
            {
                List<BlogTag> queryResult = await _repository.QueryAsync(dto => dto.BlogTagId == tag.BlogTagId);
                if (queryResult != null && queryResult.Count != 0)
                {
                    throw new BusinessException("当前操作异常");
                }
            }
            var entity = _mapper.Map<BlogTag>(tag);
            entity.BlogTagId = SnowFlakeSingle.instance.NextId();
            int result = await CreateAsync(entity);
            return ResultUtil.Success(result > 0, result > 0 ? "添加成功" : "添加失败");
        }

        public async Task<PageReponse<BlogTag>> QueryPage(TagTableQueryVo queryVo)
        {
            var pageRequest = new PageRequest
            {
                PageIndex = queryVo.PageIndex,
                PageSize = queryVo.PageSize,
            };

            var whereExp = Expressionable.Create<BlogTag>()
                .AndIF(!queryVo.TagName.isEmpty(), dto => dto.TagName.Equals(queryVo.TagName))
                .AndIF(queryVo.IsValid != -1, dto => dto.IsValid.Equals(queryVo.IsValid))
                .ToExpression();

            var result = await QueryPagedAsync(queryVo, whereExp, null);
            return ResultUtil.SuccessPage<BlogTag>(result);
        }

        public async Task<EditReponse<TagAddOrEdit>> GetById(TagAddOrEdit tagAddOrEdit)
        {
            if (tagAddOrEdit == null || tagAddOrEdit.BlogTagId == 0)
            {
                throw new BusinessException("请选择一笔数据");
            }
            var result = await base.GetByIdAsync(tagAddOrEdit.BlogTagId);
            var mapResult = _mapper.Map<TagAddOrEdit>(result);
            return ResultUtil.Success(mapResult);
        }

        public async Task<EditReponse<int>> DeleteById(TagAddOrEdit tagAddOrEdit)
        {
            if (tagAddOrEdit == null || tagAddOrEdit.BlogTagId == 0)
            {
                throw new BusinessException("请选择一笔数据");
            }
            var result = await base.DeleteByIdAsync(tagAddOrEdit.BlogTagId);
            return ResultUtil.Success(result);
        }

        public async Task<EditReponse<int>> EditById(TagAddOrEdit tagAddOrEdit)
        {
            if (tagAddOrEdit == null || tagAddOrEdit.BlogTagId == 0)
            {
                throw new BusinessException("请选择一笔数据");
            }
            var dto = await _blogTagRepository.GetByIdAsync(tagAddOrEdit.BlogTagId);
            if (dto == null)
            {
                throw new BusinessException("请检查数据是否存在");
            }
            var newDto = _mapper.Map<BlogTag>(tagAddOrEdit);
            int result = await _repository.UpdateNotNullAsync(newDto);
            return ResultUtil.Success(result);
        }
    }
}
