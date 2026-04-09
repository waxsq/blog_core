using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogPermissionRepository : Repository<BlogPermission, long>, IBlogPermissionRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogPermissionRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
