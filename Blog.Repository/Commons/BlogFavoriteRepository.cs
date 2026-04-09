using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogFavoriteRepository : Repository<BlogFavorite, long>, IBlogFavoriteRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogFavoriteRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
