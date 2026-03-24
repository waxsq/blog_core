using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogTagRepository : Repository<BlogTag, long>, IBlogTagRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogTagRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
