using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.API.Infrastructure.Models;
using ExpenseTracker.Domain;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Text;

namespace ExpenseTracker.API.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public async static Task<PaginatedResult<TDto>> CreatePaginatedResultAsync<TEntity, TDto>(this IQueryable<TEntity> query, 
            PagedRequest request, IMapper mapper) where TEntity : BaseEntity where TDto : class
        {
            query = query.ApplyFilters(request);

            var total = await query.CountAsync();

            query = query.Paginate(request);

            var projectionResult = query.ProjectTo<TDto>(mapper.ConfigurationProvider);

            projectionResult = projectionResult.Sort(request);

            var listResult = await projectionResult.ToListAsync();

            return new PaginatedResult<TDto>()
            {
                Items = listResult,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Total = total
            };
        }

        private static IQueryable<T> Paginate<T>(this IQueryable<T> query, PagedRequest request)
        {
            var entities = query.Skip((request.PageIndex) * request.PageSize).Take(request.PageSize);
            return entities;
        }

        private static IQueryable<T> Sort<T>(this IQueryable<T> query, PagedRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.ColumnNameForSorting))
            {
                query = query.OrderBy(request.ColumnNameForSorting + " " + request.SortDirection);
            }
            return query;
        }

        private static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, PagedRequest request)
        {
            var predicate = new StringBuilder();
            var requestFilters = request.RequestFilters;
            for(int i = 0; i < requestFilters.Filters.Count; i++)
            {
                if(i > 0)
                {
                    predicate.Append($" {requestFilters.LogicalOperators} ");
                }
                predicate.Append(requestFilters.Filters[i].Path + $".{nameof(string.Contains)}(@{i})");
            }

            if (requestFilters.Filters.Any())
            {
                var properyValues = requestFilters.Filters.Select(filter => filter.Value).ToArray();

                query = query.Where(predicate.ToString(), properyValues);
            }

            return query;
        }
    }
}
