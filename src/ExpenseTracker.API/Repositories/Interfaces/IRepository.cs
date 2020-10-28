using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using ExpenseTracker.API.Infrastructure.Models;
using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> Get(int id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate);
        Task Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        Task<TEntity> GetWithInclude(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<PaginatedResult<TDto>> GetPagedData<TDto>(PagedRequest request) where TDto : class;
        Task SaveChangesAsync();
        void Update(TEntity entity);
    }
}
