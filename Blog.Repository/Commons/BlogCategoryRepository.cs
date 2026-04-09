using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogCategoryRepository : Repository<BlogCategory, long>, IBlogCategoryRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogCategoryRepository(ISqlSugarClient db) : base(db) => _db = db;

        public int CountNumberByPk(long id)
        {
            return _db.Queryable<BlogCategory>().Where(x => x.BlogCategoryId == id).Count();
        }
    }
}
