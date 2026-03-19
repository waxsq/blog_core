using Blog.Core.Entities;
using Blog.Core.Interfaces;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogAuditLogRepository : Repository<BlogAuditLog, long>, IBlogAuditLogRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogAuditLogRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
