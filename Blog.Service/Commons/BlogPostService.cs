using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Entities.Vo.Post;
using Blog.Core.Exceptions;
using Blog.Core.Interfaces;
using Blog.Core.Utils;
using Blog.Repository.Interfaces;
using Blog.Service.Intefaces;
using Microsoft.IdentityModel.Tokens;
using SqlSugar;

namespace Blog.Service.Commons
{
    public class BlogPostService : Service<BlogPost, long>, IBlogPostService
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IBlogPostTagRepository _blogPostTagRepository;
        private readonly IMapper _mapper;
        public BlogPostService(IBlogPostRepository blogPostRepository, IBlogPostTagRepository blogPostTagRepository, IMapper mapper) : base(blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
            _blogPostTagRepository = blogPostTagRepository;
            _mapper = mapper;
        }

        public Task<PageReponse<PostTablePageVo>> QueryPage(PostTableQueryVo vo)
        {
            return _blogPostRepository.QueryPageAsync(vo);
        }

        public async Task<EditReponse<int>> Add(PostAddOrEditVo postAddOrEditVo)
        {
            BeanUtil.IsFieldNullOrMissing(postAddOrEditVo, "Title", "文章标题");
            BeanUtil.IsFieldNullOrMissing(postAddOrEditVo, "Summary", "文章摘要");
            BeanUtil.IsFieldNullOrMissing(postAddOrEditVo, "Content", "文章内容");
            BeanUtil.IsFieldNullOrMissing(postAddOrEditVo, "CategoryId", "文章分类");

            var newDto = _mapper.Map<BlogPost>(postAddOrEditVo);
            newDto.BlogPostId = SnowFlakeSingle.instance.NextId();
            int i = await _blogPostRepository.InsertAsync(newDto);

            newDto = await _repository.GetByIdAsync(newDto.BlogPostId);

            List<long> TagIds = postAddOrEditVo.TagIds;
            List<BlogPostTag> Tags = new List<BlogPostTag>();
            if (!TagIds.IsNullOrEmpty() && i > 0 && newDto != null)
            {
                foreach (var item in TagIds)
                {
                    var realtionDto = new BlogPostTag
                    {
                        BlogPostTagId = SnowFlakeSingle.instance.NextId(),
                        TagId = item,
                        PostId = newDto.BlogPostId
                    };
                    Tags.Add(realtionDto);
                }

                await _blogPostTagRepository.BulkInsertAsync(Tags);
            }
            return ResultUtil.Success(i);
        }
    }
}
