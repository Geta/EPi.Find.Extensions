using System.Linq;
using EPiServer.Core;
using EPiServer.Find.Cms;

namespace Geta.EPi.Find.Extensions.Models
{
    public class EmptyContentResult<T> : ContentResult<T> where T : IContentData
    {
        public EmptyContentResult() : base(
            Enumerable.Empty<T>(),
            EmptySearchResultsFactory.CreateSearchResults<ContentInLanguageReference>()
        )
        {

        }
    }
}