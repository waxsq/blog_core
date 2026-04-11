using AutoMapper;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Entities.Vo.Post;
using Blog.Core.Exceptions;
using Blog.Core.Utils;
using Blog.Repository.Interfaces;
using Dm.util;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogPostRepository : Repository<BlogPost, long>, IBlogPostRepository
    {
        private readonly ISqlSugarClient _db;
        private readonly IMapper _mapper;
        public BlogPostRepository(ISqlSugarClient db, IMapper mapper) : base(db)
        {
            _db = db;
            _mapper = mapper;
        }

        public new async Task<EditReponse<PostAddOrEditVo>> GetByIdAsync(long id)
        {
            BlogPost postDto = _db.Queryable<BlogPost>()
                .LeftJoin<BlogCategory>((p,c) => p.CategoryId == c.BlogCategoryId)
                .Where(p => p.BlogPostId == id)
                .Select(p => new BlogPost
                {
                    BlogPostId = p.BlogPostId,
                    Title = p.Title,
                    Summary = p.Summary,
                    CategoryId = p.CategoryId,
                    IsTop = p.IsTop,
                    IsFeatured = p.IsFeatured,
                    Status = p.Status,
                    ViewsCount = p.ViewsCount,
                    CommentsCount = p.CommentsCount,
                    LikesCount = p.LikesCount,
                    Content = p.Content,
                })
                .First();
            if (postDto == null)
            {
                throw new BusinessException("请检查当前对象是否存在");
            }

            List<BlogTag> tags = await _db.Queryable<BlogPostTag>()
                .LeftJoin<BlogTag>((pt, t) => pt.TagId == t.BlogTagId)
                .Where((pt, t) => pt.PostId == postDto.BlogPostId)
                .Select((pt, t) => new BlogTag
                {
                    TagName = t.TagName,
                    BlogTagId = t.BlogTagId
                })
                .ToListAsync();
            PostAddOrEditVo vo = _mapper.Map<PostAddOrEditVo>(postDto);

            BlogCategory category = await _db.Queryable<BlogCategory>()
                .InSingleAsync(vo.CategoryId);
               

            vo.CategoryName = category.CategoryName;

            vo.Tags = tags;
            return ResultUtil.Success(vo);

        }

        public async Task<PageReponse<PostTablePageVo>> QueryPageAsync(PostTableQueryVo query)
        {
            var pageReponse = new PageReponse<PostTablePageVo>
            {
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                TotalCount = 0
            };
            RefAsync<int> totalNumber = new RefAsync<int>(pageReponse.TotalCount);
            List<PostTablePageVo> result = await _db.Queryable<BlogPost>()
                .LeftJoin<BlogCategory>((p, c) => p.CategoryId == c.BlogCategoryId)
                .WhereIF(!query.Title.isEmpty(), (p, c) => p.Title.Equals(query.Title))
                .WhereIF(query.Status != -1, (p, c) => p.Status.Equals(query.Status))
                .WhereIF(query.IsFeatured != -1, (p, c) => p.IsFeatured.Equals(query.IsFeatured))
                .WhereIF(query.IsTop != -1, (p, c) => p.IsTop.Equals(query.IsTop))
                .WhereIF(query.PublishedBeginAt != null , (p, c) => p.PublishedAt >= query.PublishedBeginAt)
                .WhereIF(query.PublishedEndAt != null, (p, c) =>  p.PublishedAt <= query.PublishedEndAt)
                .WhereIF(!query.CategoryName.isEmpty(), (p, c) => c.CategoryName.Equals(query.CategoryName))
                .Select((p, c) => new PostTablePageVo
                {
                    CategoryName = c.CategoryName,
                    Title = p.Title,
                    Summary = p.Summary,
                    Status = p.Status,
                    IsFeatured = p.IsFeatured,
                    IsTop = p.IsTop,
                    ViewsCount = p.ViewsCount,
                    CommentsCount = p.CommentsCount,
                    LikesCount = p.LikesCount,
                    UpdateAt = p.UpdateAt,
                    BlogPostId = p.BlogPostId
                })
                .OrderBy($"p.{query.Field} {query.Order}")
                .ToPageListAsync(pageReponse.PageIndex, pageReponse.PageSize, totalNumber);

            var postIds = result.Select(t => t.BlogPostId).ToList();


            if (postIds.Any())
            {
                var tags = await _db.Queryable<BlogPost>()
                    .LeftJoin<BlogPostTag>((p, pt) => p.BlogPostId == pt.PostId)
                    .LeftJoin<BlogTag>((p, pt, t) => pt.TagId == t.BlogTagId)
            .Where((p, pt, t) => postIds.Contains(pt.PostId))
            .Select((p, pt, t) => new
            {
                PostId = p.BlogPostId,
                TagName = t.TagName,
                TagId = t.BlogTagId,
            })
            .ToListAsync();

                Dictionary<long, List<BlogTag>> postMap = tags.GroupBy(x => x.PostId).ToDictionary(g => g.Key, g => g.Select(x => new BlogTag
                {
                    BlogTagId = x.TagId,
                    TagName = x.TagName,
                }).ToList());

                foreach (var post in result)
                {
                    if (postMap.ContainsKey(post.BlogPostId))
                    {
                        post.Tags = postMap[post.BlogPostId];
                    }
                }
            }

            pageReponse.Datas = result;
            return ResultUtil.SuccessPage(pageReponse);
        }
    }
}
