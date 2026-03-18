using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogPageRepository : Repository<BlogPage, long>, IBlogPageRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogPageRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
