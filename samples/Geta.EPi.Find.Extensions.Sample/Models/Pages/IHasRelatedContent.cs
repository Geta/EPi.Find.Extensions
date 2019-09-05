using EPiServer.Core;

namespace Geta.EPi.Find.Extensions.Sample.Models.Pages
{
    public interface IHasRelatedContent
    {
        ContentArea RelatedContentArea { get; }
    }
}
