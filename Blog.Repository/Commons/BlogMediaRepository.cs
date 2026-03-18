using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogMediaRepository : Repository<BlogMedia, long>, IBlogMediaRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogMediaRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
