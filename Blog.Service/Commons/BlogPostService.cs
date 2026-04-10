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
using Blog.Repository.Commons;
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
        private readonly IBlogCategoryRepository _blogCategoryRepository;
        private readonly IMapper _mapper;
        public BlogPostService(IBlogPostRepository blogPostRepository,
            IBlogPostTagRepository blogPostTagRepository,
            IMapper mapper,
            IBlogCategoryRepository blogCategoryRepository) : base(blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
            _blogPostTagRepository = blogPostTagRepository;
            _mapper = mapper;
            _blogCategoryRepository = blogCategoryRepository;
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

            //判断分类是否存在
            await CheckData(postAddOrEditVo);

            var newDto = _mapper.Map<BlogPost>(postAddOrEditVo);
            newDto.BlogPostId = SnowFlakeSingle.instance.NextId();
            int i = await _blogPostRepository.InsertAsync(newDto);

            newDto = await _repository.GetByIdAsync(newDto.BlogPostId);

            List<long> TagIds = postAddOrEditVo.TagIds.Split(',').Select(x => Convert.ToInt64(x)).ToList();
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

                await _blogPostTagRepository.BatchInsertAsync(Tags);
            }
            return ResultUtil.Success(i);
        }

        public async Task<EditReponse<PostAddOrEditVo>> GetById(long id)
        {
            return await _blogPostRepository.GetByIdAsync(id);
        }

        public async Task<EditReponse<int>> DeleteById(PostAddOrEditVo vo)
        {
            int deleteResult = await _blogPostRepository.DeleteByIdAsync(vo.BlogPostId);
            if (deleteResult == 0)
            {
                throw new BusinessException("删除异常");
            }
            await _blogPostTagRepository.DeleteAsync(pt => pt.PostId == vo.BlogPostId);
            return ResultUtil.Success(deleteResult);
        }

        public async Task<EditReponse<int>> Edit(PostAddOrEditVo postAddOrEditVo)
        {
            BeanUtil.IsFieldNullOrMissing(postAddOrEditVo, "BlogPostId", "文章");
            await CheckData(postAddOrEditVo);

            var updatePost = new BlogPost
            {
                BlogPostId = postAddOrEditVo.BlogPostId,
                Title = postAddOrEditVo.Title,
                Summary = postAddOrEditVo.Summary,
                Content = postAddOrEditVo.Content,
                CategoryId = postAddOrEditVo.CategoryId
            };

            var result = await _blogPostRepository.UpdateNotNullAsync(updatePost);

            if(result == 0)
            {
                throw new BusinessException("更新异常");
            }

            List<long> tagIds = postAddOrEditVo.TagIds?.Split(",").Select(x => Convert.ToInt64(x)).ToList() ?? new List<long>();

            var insertRecord = tagIds.Select(id => new BlogPostTag
            {
                BlogPostTagId = SnowFlakeSingle.instance.NextId(),
                PostId = postAddOrEditVo.BlogPostId,
                TagId = id
            }).ToList();

            await _blogPostTagRepository.DeleteAsync(pt => pt.PostId == postAddOrEditVo.BlogPostId);
            await _blogPostTagRepository.BatchInsertAsync(insertRecord);

            return ResultUtil.Success(result);


        }


        public async Task CheckData(PostAddOrEditVo postAddOrEditVo)
        {
            BeanUtil.IsFieldNullOrMissing(postAddOrEditVo, "Title", "文章标题");
            BeanUtil.IsFieldNullOrMissing(postAddOrEditVo, "Summary", "文章摘要");
            BeanUtil.IsFieldNullOrMissing(postAddOrEditVo, "Content", "文章内容");
            BeanUtil.IsFieldNullOrMissing(postAddOrEditVo, "CategoryId", "文章分类");
            //判断分类是否存在
            int count = _blogCategoryRepository.CountNumberByPk(postAddOrEditVo.CategoryId);
            if (count == 0)
            {
                throw new BusinessException("文章分类不存在");
            }

            List<string> tagIdList = postAddOrEditVo.TagIds?.Split(",").Select(i => i.Trim()).ToList() ?? new List<string>();
            var result = (await _blogPostTagRepository.QueryAsync(pt => tagIdList.Contains(pt.TagId.ToString()))).Count();
            if (result == 0 || tagIdList.Count() == 0)
            {
                throw new BusinessException("文章标签不存在");
            }
        }
    }
}
