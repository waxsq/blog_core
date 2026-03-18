using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogVisitRepository : Repository<BlogVisit, long>, IBlogVisitRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogVisitRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
