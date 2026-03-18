using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogOauthAccountRepository : Repository<BlogOauthAccount, long>, IBlogOauthAccountRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogOauthAccountRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
