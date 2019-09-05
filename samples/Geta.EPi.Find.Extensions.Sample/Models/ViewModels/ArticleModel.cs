using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Geta.EPi.Find.Extensions.Sample.Models.ViewModels
{
    public class ArticleModel
    {
        public string Title { get; set; }
        public XhtmlString Description { get; set; }
    }
}