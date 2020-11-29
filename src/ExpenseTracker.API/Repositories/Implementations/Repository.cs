using AutoMapper;
using ExpenseTracker.API.Infrastructure.Extensions;
using ExpenseTracker.API.Infrastructure.Models;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Repositories.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ExpenseTrackerDbContext _context;
        protected readonly IMapper _mapper;

        public Repository(ExpenseTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Add(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }

        public async Task<TEntity> Get(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<PaginatedResult<TDto>> GetPagedData<TDto>(PagedRequest request) where TDto : class
        {
            return await _context.Set<TEntity>().CreatePaginatedResultAsync<TEntity, TDto>(request, _mapper);
        }

        public async Task<TEntity> GetWithInclude(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = IncludeProperties(includeProperties);
            return await query.FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await _context.Set<TEntity>().ToListAsync();
            return entities.Where(predicate.Compile());
        }

        private IQueryable<TEntity> IncludeProperties(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> entities = _context.Set<TEntity>();
            foreach(var includeProperty in includeProperties)
            {
                entities = entities.Include(includeProperty).Take(10);
            }

            return entities;
        }
    }
}
