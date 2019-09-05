using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Geta.EPi.Find.Extensions.Sample.Models.Blocks
{
    [SiteContentType(
        DisplayName = "Wildcard Best Bets Search Block", 
        GUID = "450d37a2-fa19-44e7-bfdb-fa01d0ff0772",
        Description = "Used to display wildcard search results with best bets applied")]
    [SiteImageUrl]
    public class WildcardBestBetsSearchBlock : SiteBlockData
    {
        [CultureSpecific]
        [Required(AllowEmptyStrings = false)]
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual string QueryString { get; set; }
    }
}