using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogLikeRepository : Repository<BlogLike, long>, IBlogLikeRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogLikeRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
