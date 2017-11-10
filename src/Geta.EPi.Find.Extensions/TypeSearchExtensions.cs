using System;
using System.Linq.Expressions;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Helpers;
using EPiServer.Find.Helpers.Reflection;

namespace Geta.EPi.Find.Extensions
{
    public static class TypeSearchExtensions
    {
        /// <summary>
        /// Add a filter conditionally, makes it easier to write a fluent query.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="search">The search.</param>
        /// <param name="condition">if set to <c>true</c> the filterExpression will be added.</param>
        /// <param name="filterExpression">The filter expression.</param>
        /// <returns>Updated search.</returns>
        public static ITypeSearch<TSource> ConditionalFilter<TSource>(
            this ITypeSearch<TSource> search, bool condition, Expression<Func<TSource, Filter>> filterExpression)
        {
            if (!condition)
            {
                return search;
            }

            return search.Filter<TSource>(filterExpression);
        }

        /// <summary>
        /// Gets term facets for a given field.
        /// With parameter for setting result size
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="search">The search.</param>
        /// <param name="fieldSelector">The field selector.</param>
        /// <param name="size">The number of facets to be returned.</param>
        /// <returns>Updated search.</returns>
        public static ITypeSearch<TSource> TermsFacetFor<TSource>(
            this ITypeSearch<TSource> search, Expression<Func<TSource, string>> fieldSelector, int? size)
        {
            return search.AddTermsFacetFor(fieldSelector, null, size);
        }

        /// <summary>
        /// Gets term facets for a given field.
        /// Adds ability to create facets for int fields
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="search">The search.</param>
        /// <param name="fieldSelector">The field selector.</param>
        /// <param name="size">The number of facets to be returned.</param>
        /// <returns>Updated search.</returns>
        public static ITypeSearch<TSource> TermsFacetFor<TSource>(
            this ITypeSearch<TSource> search, Expression<Func<TSource, int>> fieldSelector, int? size)
        {
            return search.AddTermsFacetFor(fieldSelector, null, size);
        }

        /// <summary>
        /// Filters by page and page size
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="search">The search.</param>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Updated search.</returns>
        public static ITypeSearch<TSource> FilterPaging<TSource>(
            this ITypeSearch<TSource> search, int page, int pageSize)
        {
            var take = pageSize;
            var skip = pageSize * (page - 1);
            return search.Skip(skip).Take(take);
        }

        private static ITypeSearch<TSource> AddTermsFacetFor<TSource>(
            this ITypeSearch<TSource> search, Expression fieldSelector, Action<TermsFacetRequest> facetRequestAction, int? size)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            var fieldPath = fieldSelector.GetFieldPath();
            var fieldName = search.Client.Conventions.FieldNameConvention.GetFieldName(fieldSelector);
            var action = facetRequestAction;
            return search.TermsFacetFor(fieldPath, x =>
            {
                x.Field = fieldName;
                x.Size = size;
                if (!action.IsNotNull())
                    return;
                action(x);
            });
        }
    }
}