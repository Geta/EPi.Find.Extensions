using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Geta.EPi.Find.Extensions.Sample.Models.ViewModels
{
    public class ConditionalSearchBlockModel
    {
        public string SearchQuery { get; set; }

        public List<ArticleModel> ArticleResults { get; set; }
    }
}