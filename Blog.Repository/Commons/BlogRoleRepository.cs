using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogRoleRepository : Repository<BlogRole, long>, IBlogRoleRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogRoleRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
