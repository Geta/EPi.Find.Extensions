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
    public class ConditionalSearchBlockController : BlockController<ConditionalSearchBlock>
    {
        public override ActionResult Index(ConditionalSearchBlock currentBlock)
        {
            var searchQuery = currentBlock.QueryString;
            var shouldNotReturnResults = currentBlock.ShouldNotReturnResults;
            var articleResults = new List<ArticleModel>();

            var searchResult = SearchClient.Instance.Search<ArticlePage>()
                .For(searchQuery)
                .Conditional(shouldNotReturnResults, y => y.Filter(z => z.Title.Match(searchQuery)))
                .GetContentResultSafe();

            foreach (var article in searchResult)
            {
                articleResults.Add(new ArticleModel
                {
                    Title = article.Title,
                    Description = article.MainBody
                });
            }

            var model = new ConditionalSearchBlockModel
            {
                SearchQuery = searchQuery,
                ArticleResults = articleResults
            };

            return PartialView(model);
        }
    }
}
