using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogCommentRepository : Repository<BlogComment, long>, IBlogCommentRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogCommentRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
