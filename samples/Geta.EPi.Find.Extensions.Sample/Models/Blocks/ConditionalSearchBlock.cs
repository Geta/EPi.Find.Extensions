using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;

namespace Geta.EPi.Find.Extensions.Sample.Models.Blocks
{
    [SiteContentType(
        DisplayName = "Conditional Search Block", 
        GUID = "fed240f3-76bf-4878-823a-0addeb79351c", 
        Description = "Used to display conditional filtering results")]
    [SiteImageUrl]
    public class ConditionalSearchBlock : SiteBlockData
    {
        [CultureSpecific]
        [Required(AllowEmptyStrings = false)]
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual string QueryString { get; set; }

        [Display(
            Name = "Should not search return results?",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        public virtual bool ShouldNotReturnResults { get; set; }
    }
}