using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogPostTagRepository : Repository<BlogPostTag, long>, IBlogPostTagRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogPostTagRepository(ISqlSugarClient db) : base(db) => _db = db;

        public List<long> QueryTagIdsByPostId(long postId)
        {
            return _db.Queryable<BlogPostTag>().Where(pt => pt.PostId == postId).Select(pt =>  pt.PostId).ToList();
        }
    }
}
