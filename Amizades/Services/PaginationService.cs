using Amizades.Models;
using Amizades.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Amizades.Services
{
    public class PaginationService
    {
        public PaginationViewModel<TResult> Pagination<TResult, TBaseQuery>(
            int pageSize,
            int page,
            IQueryable<TBaseQuery> baseQuery,
            Expression<Func<TBaseQuery, bool>> filter = null,
            Expression<Func<TBaseQuery, TResult>> selector = null,
            IEnumerable<Func<IQueryable<TBaseQuery>, IIncludableQueryable<TBaseQuery, object>>> includes = null,
            Expression<Func<TBaseQuery, object>> orderBy = null,
            bool orderByDescending = false
        )
        {

            IQueryable<TBaseQuery> query = baseQuery;

            if (includes != null && includes.Count() > 0)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }
            int count;

            if (filter != null)
            {
                query = query.Where(filter);
                count = query.Count(filter);
            }
            else
            {
                count = query.Count();
            }

            if (orderBy != null)
            {
                query = orderByDescending
                    ? query.OrderByDescending(orderBy)
                    : query.OrderBy(orderBy);
            }

            var paginationResult = new PaginationViewModel<TResult>
            {
                Items = query.Select(selector).Skip(page * pageSize).Take(pageSize).ToList(),
                HasNextPage = (page + 1) * pageSize < count
            };
            return paginationResult;
        }
    }
}
