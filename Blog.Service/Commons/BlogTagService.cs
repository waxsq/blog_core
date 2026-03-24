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
using Blog.Service.Intefaces;
using Microsoft.IdentityModel.Tokens;
using SqlSugar;

namespace Blog.Service.Commons
{
    public class BlogTagService : Service<BlogTag, long>, IBlogTagService
    {
        private readonly IRepository<BlogTag, long> _repository;
        private readonly IMapper _mapper;

        public BlogTagService(IRepository<BlogTag, long> repository, IMapper mapper) : base(repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
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
                .AndIF(!queryVo.TagName.IsNullOrEmpty(), dto => dto.TagName.Equals(queryVo.TagName))
                .AndIF(queryVo.IsValid != -1, dto => dto.IsValid.Equals(queryVo.IsValid))
                .ToExpression();
           
            var result = await QueryPagedAsync(queryVo, whereExp, null);
            return ResultUtil.SuccessPage<BlogTag>(result);
        }
    }
}
