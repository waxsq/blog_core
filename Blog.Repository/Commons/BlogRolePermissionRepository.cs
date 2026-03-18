using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogRolePermissionRepository : Repository<BlogRolePermission, long>, IBlogRolePermissionRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogRolePermissionRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
