using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogUserRoleRepository : Repository<BlogUserRole, long>, IBlogUserRoleRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogUserRoleRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
