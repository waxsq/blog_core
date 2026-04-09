using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogPostTagRepository : Repository<BlogPostTag, long>, IBlogPostTagRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogPostTagRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
