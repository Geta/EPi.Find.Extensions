using EPiServer.Find.UnifiedSearch;

namespace Geta.EPi.Find.Extensions.Models
{
    public class EmptyUnifiedSearchResults : UnifiedSearchResults
    {
        public EmptyUnifiedSearchResults() : base(
            EmptySearchResultsFactory.CreateSearchResult<UnifiedSearchHit>()
        )
        { }
    }
}