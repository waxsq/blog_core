using Blog.Core.Entities;
using Blog.Repository.Interfaces;
using SqlSugar;

namespace Blog.Repository.Commons
{
    public class BlogSettingRepository : Repository<BlogSetting, long>, IBlogSettingRepository
    {
        private readonly ISqlSugarClient _db;
        public BlogSettingRepository(ISqlSugarClient db) : base(db) => _db = db;
    }
}
