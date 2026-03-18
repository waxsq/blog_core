using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogPostRepository : Repository<BlogPost, long>, IBlogPostRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogPostRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
