using System.Linq;
using EPiServer.Find;
using EPiServer.Find.Api;

namespace Geta.EPi.Find.Extensions
{
    public static class EmptySearchResultsFactory
    {
        public static SearchResults<T> CreateSearchResults<T>()
        {
            return new SearchResults<T>(CreateSearchResult<T>());
        }

        public static SearchResult<T> CreateSearchResult<T>()
        {
            return new SearchResult<T>()
            {
                Facets = new FacetResults(),
                Hits = new HitCollection<T>
                {
                    Hits = Enumerable.Empty<SearchHit<T>>().ToList(),
                    Total = 0
                },
                Shards = new Shards()
            };
        }
    }
}