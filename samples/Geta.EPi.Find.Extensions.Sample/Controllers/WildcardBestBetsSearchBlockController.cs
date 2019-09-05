using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Framework;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using Geta.EPi.Find.Extensions.Sample.Models.Blocks;
using Geta.EPi.Find.Extensions.Sample.Models.Pages;
using Geta.EPi.Find.Extensions.Sample.Models.ViewModels;

namespace Geta.EPi.Find.Extensions.Sample.Controllers
{
    public class WildcardBestBetsSearchBlockController : BlockController<WildcardBestBetsSearchBlock>
    {
        public override ActionResult Index(WildcardBestBetsSearchBlock currentBlock)
        {
            var searchQuery = currentBlock.QueryString;
            var articleResults = new List<ArticleModel>();

            var searchResult = SearchClient.Instance.Search<ArticlePage>()
                .ForWithWildcards(searchQuery, (x => x.Title, 1.5), (x => x.Name, 0.5))
                .GetContentResultSafe();

            foreach (var article in searchResult)
            {
                articleResults.Add(new ArticleModel
                {
                    Title = article.Title,
                    Description = article.MainBody
                });
            }

            var model = new WildcardBestBetsSearchBlockModel
            {
                SearchQuery = searchQuery,
                ArticleResults = articleResults
            };

            return PartialView(model);
        }
    }
}
