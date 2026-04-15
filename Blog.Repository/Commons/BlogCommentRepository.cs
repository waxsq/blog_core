using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Entities.Vo.Comment;
using Blog.Core.Entities.Vo.Post;
using Blog.Core.Utils;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogCommentRepository : Repository<BlogComment, long>, IBlogCommentRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogCommentRepository(ISqlSugarClient db) : base(db) => _db = db;

        public async Task<PageReponse<CommentTablePageVo>> QueryPage(CommentTableQueryVo query)
        {
            var pageReponse = new PageReponse<CommentTablePageVo>
            {
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                TotalCount = 0
            };

            RefAsync<int> totalNumber = new RefAsync<int>(pageReponse.TotalCount);
            var result = await _db.Queryable<BlogComment>()
                .LeftJoin<BlogPost>((c, p) => c.PostId == p.BlogPostId)
                .WhereIF(query.Status != -1, c => c.Status == query.Status)
                .WhereIF(query.CreateBeginAt != null, c => c.CreateAt >= query.CreateBeginAt)
                .WhereIF(query.CreateEndAt != null, c => c.CreateAt <= query.CreateEndAt)
                .WhereIF(!string.IsNullOrEmpty(query.PostTitle), (c, p) => p.Title.Contains(query.PostTitle))
                .Select((c, p) => new CommentTablePageVo
                {
                    PostId = p.BlogPostId,
                    PostTitle = p.Title,
                }, true)
                .ToPageListAsync(pageReponse.PageIndex,pageReponse.PageSize,totalNumber);
            pageReponse.Datas = result;
            return ResultUtil.SuccessPage(pageReponse);
        }
    }
}
