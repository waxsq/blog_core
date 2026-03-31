using AutoMapper;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Entities.Vo.Post;
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

        public async Task<PageReponse<PostTablePageVo>> QueryPageAsync(PostTableQueryVo query)
        {
            var pageReponse = new PageReponse<PostTablePageVo>
            {
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                TotalCount = 0
            };
            RefAsync<int> totalNumber = new RefAsync<int>(pageReponse.TotalCount);
            var result = await _db.Queryable<BlogPost>()
                .LeftJoin<BlogCategory>((p, c) => p.CategoryId == c.BlogCategoryId)
                .WhereIF(!query.Title.isEmpty(), (p, c) => p.Title.Equals(query.Title))
                .WhereIF(query.Status != -1, (p, c) => p.Status.Equals(query.Status))
                .WhereIF(query.IsFeatured != -1, (p, c) => p.IsFeatured.Equals(query.IsFeatured))
                .WhereIF(query.IsTop != -1, (p, c) => p.IsTop.Equals(query.IsTop))
                .WhereIF(query.PublishedBeginAt != null && query.PublishedEndAt != null, (p, c) => p.PublishedAt >= query.PublishedBeginAt && p.PublishedAt <= query.PublishedEndAt)
                .WhereIF(!query.CategoryName.isEmpty(), (p, c) => c.CategoryName.Equals(query.CategoryName))
                .Select((p, c) => new PostTablePageVo
                {
                    CategoryName = c.CategoryName
                }, true)
                .OrderBy($"p.{query.Field} {query.Order}")
                .ToPageListAsync(pageReponse.PageIndex, pageReponse.PageSize, totalNumber);
            pageReponse.Datas = result;
            return ResultUtil.SuccessPage(pageReponse);
        }
    }
}
