using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SqlSugar;

namespace Blog.Core.Interfaces
{
    /// <summary>
    /// 通用服务层接口（业务层抽象）
    /// - 将数据访问（Repository）上层封装为业务服务接口，便于在 Razor Pages / DI 中注入使用。
    /// - 约定使用 SqlSugar 的 <see cref="PageModel"/> 作为分页参数。
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    public interface IService<TEntity, TKey>
        where TEntity : class, new()
    {
        // --- Create ---
        Task<int> CreateAsync(TEntity entity);
        Task<int> CreateAsync(IEnumerable<TEntity> entities);

        // --- Read ---
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>>? predicate = null);

        /// <summary>
        /// 分页查询（PageModel 会被填充 Total）
        /// 实现建议：调用 Repository 的分页方法或直接使用 ISqlSugarClient.Queryable.ToPageListAsync(pageModel)
        /// </summary>
        Task<List<TEntity>> QueryPagedAsync(PageModel pageModel,
            Expression<Func<TEntity, bool>>? predicate = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool isAsc = true);

        /// <summary>
        /// 使用树形参数进行查询（业务层根据树形参数解析查询条件）
        /// </summary>
        Task<List<TEntity>> QueryByTreeAsync<TTree>(TTree tree, Expression<Func<TEntity, bool>>? predicate = null);

        // --- Update ---
        Task<int> UpdateAsync(TEntity entity);
        Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression);

        // --- Delete ---
        Task<int> DeleteByIdAsync(TKey id);
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        // --- 辅助方法 ---
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
    }
}
