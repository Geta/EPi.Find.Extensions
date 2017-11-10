using System.Linq;
using EPiServer.Find.Api;
using EPiServer.Find.Api.Facets;

namespace Geta.EPi.Find.Extensions
{
    public static class FacetResultsExtensions
    {
        public static readonly TermsFacet EmptyTermsFacet = new TermsFacet { Name = string.Empty, Terms = Enumerable.Empty<TermCount>() };

        /// <summary>
        /// Returns a terms facet by name from facet results. If not found, returns empty terms facet.
        /// </summary>
        /// <param name="results">Facet results.</param>
        /// <param name="name">Facet name.</param>
        /// <returns>Found terms facet.</returns>
        public static TermsFacet GetTermsFacetByName(this FacetResults results, string name)
        {
            return results.OfType<TermsFacet>().FirstOrDefault(x => x.Name.Equals(name)) ?? EmptyTermsFacet;
        }
    }
}