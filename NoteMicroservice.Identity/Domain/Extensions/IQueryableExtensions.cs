using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NoteMicroservice.Identity.Domain.Dto.BaseDtos;

namespace NoteMicroservice.Identity.Domain.Extensions
{
    public static class IQueryableExtensions
    {
        public static async Task<PaginatedListDto<TDto>> CreatePaginationAsync<TModel, TSelector, TDto>(this IQueryable<TModel> query, int pageIndex, int pageSize,
            Expression<Func<TModel, TSelector>> selector,
            Func<TSelector, TDto> mapper)
        {
            var paginationDto = await query.CreatePaginationWithoutItemAsync<TModel, TDto>(pageIndex, pageSize);

            var paginatedQuery = query
                    .Skip((paginationDto.PageIndex - 1) * paginationDto.PageSize)
                    .Take(paginationDto.PageSize);

            List<TDto> items = (await paginatedQuery
                    .Select(selector)
                    .ToListAsync())
                    .Select(e => mapper(e)).ToList();

            paginationDto.Items = items;
            return paginationDto;
        }

        public static async Task<PaginatedListDto<TDto>> CreatePaginationAsync<TModel, TDto>(this IQueryable<TModel> query, int pageIndex, int pageSize,
            Func<TModel, TDto> mapper, bool serverSideEvaluation = false)
        {
            var paginationDto = await query.CreatePaginationWithoutItemAsync<TModel, TDto>(pageIndex, pageSize);

            var paginatedQuery = query
                    .Skip((paginationDto.PageIndex - 1) * paginationDto.PageSize)
                    .Take(paginationDto.PageSize);

            List<TDto> items = new List<TDto>();
            if (serverSideEvaluation)
            {
                items = await paginatedQuery
                    .Select(e => mapper(e))
                    .ToListAsync();
            }
            else
            {
                items = (await paginatedQuery
                    .ToListAsync())
                    .Select(e => mapper(e)).ToList();
            }

            paginationDto.Items = items;
            return paginationDto;
        }

        public static async Task<PaginatedListDto<TDto>> CreatePaginationWithoutItemAsync<TModel, TDto>(this IQueryable<TModel> searchQuery, int pageIndex, int pageSize)
        {
            var itemsCount = await searchQuery.CountAsync();
            var pageCount = (int)Math.Ceiling(itemsCount / (double)pageSize);
            if (pageCount < 1)
            {
                pageCount = 1;
            }
            if (pageIndex > pageCount)
            {
                pageIndex = pageCount;
            }

            return new PaginatedListDto<TDto>(itemsCount, pageCount, pageIndex, pageSize);
        }

        public static IOrderedQueryable<T> OrderBy<T, TKey>(this IQueryable<T> query, bool desc, Expression<Func<T, TKey>> keySelector)
        {
            if (desc)
            {
                return query.OrderByDescending(keySelector);
            }
            else
            {
                return query.OrderBy(keySelector);
            }
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName)
        {
            return query.OrderBy(ToLambda<T>(propertyName));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName)
        {
            return query.OrderByDescending(ToLambda<T>(propertyName));
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }
    }
}
