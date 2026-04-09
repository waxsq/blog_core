using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogUserRepository : Repository<BlogUser, long>, IBlogUserRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogUserRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
