using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Interfaces;
using SqlSugar;

namespace Blog.Service.Commons
{
    /// <summary>
    /// 通用服务层实现（基于 IRepository 实现）
    /// - 依赖注入 IRepository<TEntity, TKey>（项目中可注入 Blog.Core.Repositories.Repository）
    /// - 将数据操作委托给仓储层，提供 IService 定义的方法实现
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    public class Service<TEntity, TKey> : IService<TEntity, TKey>
        where TEntity : class, new()
    {
        protected readonly IRepository<TEntity, TKey> _repository;

        public Service(IRepository<TEntity, TKey> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // --- Create ---
        public virtual async Task<int> CreateAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return await _repository.InsertAsync(entity);
        }

        public virtual async Task<int> CreateAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            return await _repository.InsertAsync(entities);
        }

        // --- Read ---
        public virtual async Task<TEntity?> GetByIdAsync(TKey id)
        {
            if (id == null) return null;
            return await _repository.GetByIdAsync(id);
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return await _repository.QueryAsync(predicate);
        }

        public virtual async Task<PageReponse<TEntity>> QueryPagedAsync(PageRequest pageRequest,
            Expression<Func<TEntity, bool>>? predicate = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool isAsc = true)
        {
            if (pageRequest == null) throw new ArgumentNullException(nameof(pageRequest));
            return await _repository.QueryPagedAsync(pageRequest, predicate, orderBy, isAsc);
        }

        public virtual async Task<List<TEntity>> QueryByTreeAsync<TTree>(TTree tree, Expression<Func<TEntity, bool>>? predicate = null)
        {
            return await _repository.QueryByTreeAsync(tree, predicate);
        }

        // --- Update ---
        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return await _repository.UpdateAsync(entity);
        }

        public virtual async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (updateExpression == null) throw new ArgumentNullException(nameof(updateExpression));
            return await _repository.UpdateAsync(predicate, updateExpression);
        }

        // --- Delete ---
        public virtual async Task<int> DeleteByIdAsync(TKey id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            return await _repository.DeleteByIdAsync(id);
        }

        public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await _repository.DeleteAsync(predicate);
        }

        // --- 辅助方法 ---
        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            var list = await _repository.QueryAsync(predicate);
            return list != null && list.Count > 0;
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var list = await _repository.QueryAsync(predicate);
            return list?.Count ?? 0;
        }
    }
}
