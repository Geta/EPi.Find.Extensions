using System;
using System.Linq.Expressions;
using System.Text;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Api.Querying.Queries;
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
        /// <returns></returns>
        public static ITypeSearch<TSource> ConditionalFilter<TSource>(this ITypeSearch<TSource> search, bool condition, Expression<Func<TSource, Filter>> filterExpression)
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
        /// <returns></returns>
        public static ITypeSearch<TSource> TermsFacetFor<TSource>(this ITypeSearch<TSource> search, Expression<Func<TSource, string>> fieldSelector, int? size)
        {
            return search.AddTermsFacetFor<TSource>((Expression)fieldSelector, (Action<TermsFacetRequest>)null, size);
        }

        /// <summary>
        /// Gets term facets for a given field.
        /// Adds ability to create facets for int fields
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="search">The search.</param>
        /// <param name="fieldSelector">The field selector.</param>
        /// <param name="size">The number of facets to be returned.</param>
        /// <returns></returns>
        public static ITypeSearch<TSource> TermsFacetFor<TSource>(this ITypeSearch<TSource> search, Expression<Func<TSource, int>> fieldSelector, int? size)
        {
            return search.AddTermsFacetFor<TSource>((Expression)fieldSelector, (Action<TermsFacetRequest>)null, size);
        }

        private static ITypeSearch<TSource> AddTermsFacetFor<TSource>(this ITypeSearch<TSource> search, Expression fieldSelector, Action<TermsFacetRequest> facetRequestAction, int? size)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");
            string fieldPath = fieldSelector.GetFieldPath();
            string fieldName = search.Client.Conventions.FieldNameConvention.GetFieldName(fieldSelector);
            Action<TermsFacetRequest> action = facetRequestAction;
            return search.TermsFacetFor<TSource>(fieldPath, (Action<TermsFacetRequest>)(x =>
            {
                x.Field = fieldName;
                x.Size = size;
                if (!action.IsNotNull())
                    return;
                action(x);
            }));
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

    }
}