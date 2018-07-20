using System;
using System.Linq.Expressions;
using System.Text;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Api.Querying.Queries;
using EPiServer.Find.Cms;
using EPiServer.Find.Helpers;
using EPiServer.Find.Helpers.Reflection;
using EPiServer.Find.UnifiedSearch;
using EPiServer.Logging;
using Geta.EPi.Find.Extensions.Models;

namespace Geta.EPi.Find.Extensions
{
    public static class TypeSearchExtensions
    {
        private static readonly ILogger Log = LogManager.GetLogger(typeof(TypeSearchExtensions));
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

        /// <summary>
        /// Search with wildcards
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="typeSearch"></param>
        /// <param name="query"></param>
        /// <param name="allowLeadingWildcard"></param>
        /// <param name="analyzeWildCard"></param>
        /// <param name="fuzzyMinSim"></param>
        /// <returns></returns>
        public static IQueriedSearch<TSource, QueryStringQuery> ForWildcardSearch<TSource>(this ITypeSearch<TSource> typeSearch, string query, bool allowLeadingWildcard = true, bool analyzeWildCard = true, double fuzzyMinSim = 0.9)
        {
            return typeSearch.For(query, stringQuery =>
            {
                stringQuery.Query = AddWildcards(stringQuery.Query.ToString());
                stringQuery.AllowLeadingWildcard = allowLeadingWildcard;
                stringQuery.AnalyzeWildcard = analyzeWildCard;
                stringQuery.FuzzyMinSim = fuzzyMinSim;
            });
        }
        
        /// <summary>
        /// Adds wildcards in at the *front and at the end* of the string.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        private static string AddWildcards(string query)
        {
            var sb = new StringBuilder();
            if (!query.StartsWith("*"))
            {
                sb.Append("*");
            }

            sb.Append(query);

            if (!query.EndsWith("*"))
            {
                sb.Append("*");
            }
            return sb.ToString();
        }

        /// Catches <see cref="ServiceException"/> and <see cref="ClientException"/> and returns an <see cref="EmptyContentResult"/>
        /// </summary>
        public static IContentResult<TContentData> GetContentResultSafe<TContentData>(
            this ITypeSearch<TContentData> search,
            int cacheForSeconds = 60,
            bool cacheForEditorsAndAdmins = false) where TContentData : IContentData
        {
            IContentResult<TContentData> contentResult;
            try
            {
                contentResult = search
                    .GetContentResult(cacheForSeconds, cacheForEditorsAndAdmins);
            }
            catch (Exception ex) when (ex is ClientException || ex is ServiceException)
            {
                Log.Error("Could not retrieve data from find, returning empty contentresult", ex);
                contentResult = new EmptyContentResult<TContentData>();
            }
            return contentResult;
        }

        /// <summary>
        /// Catches <see cref="ServiceException"/> and <see cref="ClientException"/> and returns an <see cref="EmptyUnifiedSearchResults"/>
        /// </summary>
        public static UnifiedSearchResults GetResultSafe(
            this ITypeSearch<ISearchContent> search,
            HitSpecification hitSpecification = null,
            bool filterForPublicSearch = true)
        {
            UnifiedSearchResults contentResult = null;
            try
            {
                contentResult =
                    search.GetResult(hitSpecification, filterForPublicSearch);
            }
            catch (Exception ex) when (ex is ClientException || ex is ServiceException)
            {
                Log.Error("Could not retrieve data from find, returning empty UnifiedSearchResults", ex);
                contentResult = new EmptyUnifiedSearchResults();
            }
            return contentResult;
        }
    }
}