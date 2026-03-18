using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SqlSugar;

namespace Blog.Core.Interfaces
{
    /// <summary>
    /// 通用仓储接口（针对 SqlSugar 实现）
    /// - CRUD：增、删、改、查（按 Id、按条件）
    /// - 分页查询：使用 SqlSugar 的 <see cref="PageModel"/> 进行分页
    /// - 树参数查询：接受一个任意的树形结构参数，具体如何根据树构建查询由实现方负责（建议在实现中使用 SqlSugar 的 Queryable 构建查询）
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    public interface IRepository<TEntity, TKey>
        where TEntity : class, new()
    {
        // --- Create ---
        Task<int> InsertAsync(TEntity entity);
        Task<int> InsertAsync(IEnumerable<TEntity> entities);

        // --- Read ---
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>>? predicate = null);

        /// <summary>
        /// 分页查询（使用 SqlSugar 的 PageModel）
        /// 实现建议：return db.Queryable<TEntity>().Where(predicate).OrderBy(...).ToPageList(pageModel);
        /// </summary>
        /// <param name="pageModel">分页参数（PageIndex、PageSize 等）</param>
        /// <param name="predicate">可选筛选表达式</param>
        /// <param name="orderBy">可选排序表达式</param>
        /// <param name="isAsc">排序方向，默认升序</param>
        Task<List<TEntity>> QueryPagedAsync(PageModel pageModel,
            Expression<Func<TEntity, bool>>? predicate = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool isAsc = true);

        /// <summary>
        /// 使用树形参数进行查询（参数类型由业务决定）
        /// 实现方应根据传入的 tree 参数构建 SqlSugar 查询（例如按树节点集合筛选、按父子关系递归等）。
        /// </summary>
        /// <typeparam name="TTree">树参数类型（可为自定义 DTO、节点集合等）</typeparam>
        /// <param name="tree">树形参数</param>
        /// <param name="predicate">可选额外筛选表达式</param>
        Task<List<TEntity>> QueryByTreeAsync<TTree>(TTree tree, Expression<Func<TEntity, bool>>? predicate = null);

        // --- Update ---
        Task<int> UpdateAsync(TEntity entity);
        Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression);

        // --- Delete ---
        Task<int> DeleteByIdAsync(TKey id);
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
