using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogSubscriptionRepository : Repository<BlogSubscription, long>, IBlogSubscriptionRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogSubscriptionRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
