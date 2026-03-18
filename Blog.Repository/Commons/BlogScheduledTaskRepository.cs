using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogScheduledTaskRepository : Repository<BlogScheduledTask, long>, IBlogScheduledTaskRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogScheduledTaskRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
